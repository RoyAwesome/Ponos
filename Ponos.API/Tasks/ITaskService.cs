using Ponos.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ponos.API.Tasks
{
    public interface ITaskService : INamed
    {
        public Task Enqueue(Action action);
        public Task Enqueue(Action action, Thread requiredThread);

        public Task Enqueue(Action action, params object[] Dependencies);

        public Task Enqueue(Action action, Thread requireThread, params object[] Dependencies);

    }
}
