using Autofac;
using NLog;
using Ponos.API;
using Ponos.API.Commands;
using Ponos.API.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Ponos
{
    class MockScheduler : IApplicationEventListener
    {
        class FixedRateStage
        {
            public double TimeRemaining;
            public ICommandStage Stage;
            public bool RemoveAfterExecute;
        }

        public bool ShouldLoop = true;
       
        Channel<Action> ThreadCommandQueue = Channel.CreateUnbounded<Action>();

        

        Stopwatch stopwatch = new();

        Thread SingleThread;

        private readonly IComponentContext componentContext;
        private readonly ILogger logger;
        public MockScheduler(IComponentContext componentContext, ILogger logger)
        {
            this.componentContext = componentContext;
            this.logger = logger;
        }

        public void Start()
        {
            SingleThread = new Thread(RunLoop);
            SingleThread.Start();
        }

        public void ScheduleStage(ICommandStage stage)
        {
            ThreadCommandQueue.Writer.TryWrite(() =>
            {
                Thead_ScheduleStage(stage);
            });          
        }
         ThreadLocal<List<FixedRateStage>> Thread_Stages = new(() => new());


        private void Thead_ScheduleStage(ICommandStage stage)
        {
            logger.Info("Scheduing Stage {0}", stage.ToString());

            var Stages = Thread_Stages.Value;
           
            //If we have this stage, don't bother scheduling it
            if(Stages.Select(x => x.Stage).Contains(stage))
            {
                return;
            }


            Stages.Add(new FixedRateStage()
            {
                Stage = stage,
                TimeRemaining = stage.RunMode == CommandStageRunMode.Fixed ? stage.RunRate : 0,
                RemoveAfterExecute = stage.RunMode == CommandStageRunMode.Custom,
            });
        }

        public void RunLoop()
        {
            logger.Info("Starting Loop");
            stopwatch.Start();
            while(ShouldLoop)
            {
                long FrameStart = stopwatch.ElapsedTicks;

                //If we have any commands that need doing, lets do them now
                while(ThreadCommandQueue.Reader.TryRead(out Action command))
                {
                    command();
                }

                long FrameDelta = stopwatch.ElapsedTicks - FrameStart;
                double FrameDeltaMs = (double)(FrameDelta) / (double)TimeSpan.TicksPerMillisecond;

                var Stages = Thread_Stages.Value;
                //Run any stages we have scheduled
                for(int i = 0; i < Stages.Count; i++)
                {
                    var frs = Stages[i];

                    frs.TimeRemaining = frs.TimeRemaining - FrameDeltaMs;

                    if (frs.TimeRemaining <= 0)
                    {
                        frs.Stage.Execute();
                        foreach (var action in frs.Stage.RunAfter)
                        {
                            action();
                        }
                        frs.TimeRemaining = frs.Stage.RunMode == CommandStageRunMode.Fixed ? frs.Stage.RunRate : 0;
                    }

                    if(frs.RemoveAfterExecute)
                    {
                        Stages.RemoveAt(i);
                        i--;
                    }

                }               

                Thread.Sleep(1);
            }
            logger.Warn("Terminating Update Loop");
        }

        public void OnApplicationEvent(ApplicationEvent Stage)
        {
           
        }
    }
}
