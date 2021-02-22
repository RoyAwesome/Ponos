using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ponos.API.Jobs
{
    public class Job
    {

        Action<Job> ExecutingAction
        {
            get;
            set;
        }

        Job ParentJob
        {
            get;
            set;
        }

        /// <summary>
        /// If not null, this job must run on this thread
        /// </summary>
        public Thread RequiredThread
        {
            get;
            private set;
        }

        /// <summary>
        /// All of our dependencies.  This is used to create a job graph
        /// TODO: Dependency object that lets us create a Directed Acyclic Graph
        /// </summary>
        public object[] Dependencies
        {
            get;
            private set;
        }

        [ThreadStatic]
        internal static Job CurrentlyExecutingJob;
       

        public Job(Action<Job> action, Job parent, Thread requiredThread, params object[] dependencies)
        {
            ExecutingAction = action;
            ParentJob = parent;
            RequiredThread = requiredThread;
            Dependencies = dependencies;
        }

        #region Constructor Variations
        public Job(Action<Job> action)
            : this(action, null, null, null)
        {

        }

        public Job(Action<Job> action, Job parent)
            : this(action, parent, null, null)
        {

        }
        public Job(Action<Job> action, Job parent, Thread requiredThread)
            : this(action, parent, requiredThread, null)
        {

        }

        public Job(Action<Job> action, Thread requiredThread)
            : this(action, null, requiredThread, null)
        {

        }

        public Job(Action<Job> action, params object[] dependencies)
           : this(action, null, null, dependencies)
        {

        }

        public Job(Action<Job> action, Job parent, params object[] dependencies)
           : this(action, parent, null, dependencies)
        {

        }
       
        public Job(Action<Job> action, Thread requiredThread, params object[] dependencies)
            : this(action, null, requiredThread, dependencies)
        {

        }
        #endregion

        public void MarkReadyToStart()
        {

        }

    }
}
