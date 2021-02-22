using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Hosting;
using Ponos.API.Tasks;
using Ponos.API.Threading;

namespace Ponos.Threading
{
    class ThreadService : TaskScheduler, IThreadService, ITaskService, IDisposable
    {
        public string Name => "Default Thread Service";

        private Thread[] _threads;
        private int _concurrencyLevel;

        private readonly IComponentContext componentContext;
        public ThreadService(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        public void Startup(int concurrencyLevel)
        {
            _concurrencyLevel = concurrencyLevel;
            _threads = new Thread[_concurrencyLevel];
            for (int i = 0; i < _threads.Length; i++)
            {
                _threads[i] = new Thread(DispatchLoop) { IsBackground = true };
                _threads[i].Start();
            }

            //Walk through all the services that are thread locked, and then assign them a thread. 
            //TODO: Try to balance this better.
            var lockedThreadResources = componentContext.Resolve<IEnumerable<IThreadLocked>>();
            for(int i = 0; i < lockedThreadResources.Count(); i++)
            {
                int threadIndex = i % _threads.Length;
                lockedThreadResources.ElementAt(i).LockToThread(_threads[threadIndex]);
            }
        }

        private void DispatchLoop()
        {
            while(true)
            {
                foreach(var task in taskCollection.GetConsumingEnumerable())
                {
                    //Quick and simple task graph.  The threads are greedy
                    if(task is PonosTask)
                    {                        
                        PonosTask pt = task as PonosTask;
                        if(pt.RequiredThread != null && pt.RequiredThread != Thread.CurrentThread)
                        {
                            continue;
                        }                      
                    }
                    TryExecuteTask(task);
                }

                Thread.Sleep(1);
            }
        }

        BlockingCollection<Task> taskCollection = new BlockingCollection<Task>();

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return taskCollection.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            taskCollection.Add(task);
            
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        public Task Enqueue(Action action)
        {
            var task = new PonosTask(action);
            task.Start(this);
            return task;
        }

        public Task Enqueue(Action action, Thread requiredThread)
        {
            var task = new PonosTask(action, requiredThread);
            task.Start(this);
            return task;
        }

        public Task Enqueue(Action action, params object[] Dependencies)
        {
            throw new NotImplementedException();
        }

        public Task Enqueue(Action action, Thread requireThread, params object[] Dependencies)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            taskCollection.CompleteAdding();
            for (int i = 0; i < _threads.Length; i++)
            {
                //Todo: Kill the threads
            }
        }
    }
}
