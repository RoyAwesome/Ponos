using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ponos.API.Threading
{
    public interface IThreadLocked
    {
        public Thread RequiredThread
        {
            get;
            set;
        }
    }

    public static class ThreadLockedUtils
    {
        public static void LockToThread(this IThreadLocked service, Thread thread)
        {
            if(service.RequiredThread != null)
            {
                throw new InvalidOperationException("Cannot lock a thread that is already locked");
            }

            service.RequiredThread = thread;
        }

        public static void CheckThreadExecution(this IThreadLocked threadLocked)
        {
            //TODO: #if DEBUG
            if(threadLocked.RequiredThread != null && threadLocked.RequiredThread != Thread.CurrentThread)
            {
                throw new InvalidOperationException("Trying to execute function on a thread we are not locked to");
            }
        }
    }
}
