using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ponos.API.Tasks
{
    public class PonosTask : Task
    {
        public Thread RequiredThread
        {
            get;
            private set;
        }

        public PonosTask(Action action) : base(action)
        {
        }

        public PonosTask(Action action, CancellationToken cancellationToken) : base(action, cancellationToken)
        {
        }

        public PonosTask(Action action, TaskCreationOptions creationOptions) : base(action, creationOptions)
        {
        }
        public PonosTask(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(action, cancellationToken, creationOptions)
        {
        }

        public PonosTask(Action<object> action, object state) : base(action, state)
        {
        }


        public PonosTask(Action<object> action, object state, CancellationToken cancellationToken) : base(action, state, cancellationToken)
        {
        }

        public PonosTask(Action<object> action, object state, TaskCreationOptions creationOptions) : base(action, state, creationOptions)
        {
        }

        public PonosTask(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(action, state, cancellationToken, creationOptions)
        {
        }

        public PonosTask(Action action, Thread thread)
            : this(action, thread, default(CancellationToken), TaskCreationOptions.None)
        {
        }

        public PonosTask(Action action, Thread thread, CancellationToken cancellationToken) 
            : this(action, thread, cancellationToken, TaskCreationOptions.None)
        {
        }

        public PonosTask(Action action, Thread thread, TaskCreationOptions creationOptions) 
            : this(action, thread, default(CancellationToken), creationOptions)
        {         
        }

        public PonosTask(Action action, Thread thread, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(action, cancellationToken, creationOptions)
        {
            RequiredThread = thread;
        }

        public PonosTask(Action<object> action, object state, Thread thread, CancellationToken cancellationToken, TaskCreationOptions creationOptions) 
            : base(action, state, cancellationToken, creationOptions)
        {
            RequiredThread = thread;
        }

        public PonosTask(Action<object> action, object state, Thread thread) 
            : this(action, state, thread, default(CancellationToken), TaskCreationOptions.None)
        {
        }


        public PonosTask(Action<object> action, object state, Thread thread, CancellationToken cancellationToken)
            : this(action, state, thread, cancellationToken, TaskCreationOptions.None)
        {
        }

        public PonosTask(Action<object> action, object state, Thread thread, TaskCreationOptions creationOptions)
            : this(action, state, thread, default(CancellationToken), creationOptions)
        {
        }
    }
}
