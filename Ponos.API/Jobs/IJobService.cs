using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ponos.API.Jobs
{
    public interface IJobService
    {
        public Job Enqueue(Action action, JobCreateOptions options = JobCreateOptions.None);
        public Job Enqueue(Action action, Thread requiredThread, JobCreateOptions options = JobCreateOptions.None);

        public Job Enqueue(Action action, params object[] Dependencies);

        public Job Enqueue(Action action, JobCreateOptions options, params object[] dependencies);

        public Job Enqueue(Action action, Thread requireThread, JobCreateOptions options, params object[] dependencies);
    }

    /// <summary>
    /// Extensions for IJobService, creating default functionality
    /// </summary>
    public static class JobServiceExt
    {
        public static void EnqueueAndStart(this IJobService service, Action action, JobCreateOptions options = JobCreateOptions.None)
        {
            Job job = service.Enqueue(action, options);
            job.MarkReadyToStart();
        }
        public static void EnqueueAndStart(this IJobService service, Action action, Thread requiredThread, JobCreateOptions options = JobCreateOptions.None)
        {
            Job job = service.Enqueue(action, requiredThread, options);
            job.MarkReadyToStart();
        }
        public static void EnqueueAndStart(this IJobService service, Action action, params object[] dependencies)
        {
            Job job = service.Enqueue(action, dependencies);
            job.MarkReadyToStart();
        }
        public static void EnqueueAndStart(this IJobService service, Action action, JobCreateOptions options, params object[] dependencies)
        {
            Job job = service.Enqueue(action, options, dependencies);
            job.MarkReadyToStart();
        }
        public static void EnqueueAndStart(this IJobService service, Action action, Thread requireThread, JobCreateOptions options, params object[] dependencies)
        {
            Job job = service.Enqueue(action, options, requireThread, dependencies);
            job.MarkReadyToStart();
        }
    }
}
