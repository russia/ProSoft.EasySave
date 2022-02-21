using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Exceptions;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Infrastructure.Models
{
    public static class Workflow
    {
        public static async Task<T> StartAsync<T>(this Func<Task<JobResult>> task, ExecutionType executionType,
            CancellationToken cancellationToken = default) where T : class
        {
            return executionType switch
            {
                ExecutionType.SINGLE => await task.StartSingleJobAsync(cancellationToken) as T,
                _ => throw new ExecutionTypeNotSupportedException("It is only possible to copy files sequentially when there is only one task to perform.")
            };
        }

        public static async Task<T> StartAsync<T>(this List<Func<Task<JobResult>>> tasks, ExecutionType executionType,
            CancellationToken cancellationToken = default) where T : class
        {
            return executionType switch
            {
                ExecutionType.SEQUENTIAL => await tasks.StartSequentialJobsAsync(cancellationToken) as T,
                ExecutionType.CONCURRENT => await tasks.StartParallelJobsAsync(cancellationToken: cancellationToken) as T,
                ExecutionType.SINGLE => await tasks.StartSingleJobAsync(cancellationToken) as T,
                _ => throw new NotImplementedException()
            };
        }

        private static async Task<IReadOnlyCollection<JobResult>> StartParallelJobsAsync(
            this List<Func<Task<JobResult>>> tasks, uint maxTasks = 5, CancellationToken cancellationToken = default)
        {
            //var jobResults = new List<JobResult>();
            ////var tasksList = new List<Task<JobResult>>();
            //Parallel.ForEach(tasks,
            //    new ParallelOptions
            //    {
            //        MaxDegreeOfParallelism = (int)maxTasks
            //    },
            //    async (x, cancellationToken) =>
            //    {
            //        jobResults.Add(await x());
            //        //tasksList.Add(test);
            //    });

            var jobResults = await Task.WhenAll(tasks.Select(async t => await t()));
            return jobResults.ToList();
        }

        private static async Task<IReadOnlyCollection<JobResult>> StartSequentialJobsAsync(
            this List<Func<Task<JobResult>>> jobContexts, CancellationToken cancellationToken = default)
        {
            List<JobResult> results = new();
            foreach (var jobContext in jobContexts) // .Where(x => !x.IsCompleted)
            {
                results.Add(await jobContext());
            }

            return results;
        }

        private static async Task<IReadOnlyCollection<JobResult>> StartSingleJobAsync(this IList<Func<Task<JobResult>>> tasks,
            CancellationToken cancellationToken = default)
        {
            var task = tasks.First(); // x => !x.IsCompleted
            return new List<JobResult>() { await task() };
        }

        private static async Task<IReadOnlyCollection<JobResult>> StartSingleJobAsync(this Func<Task<JobResult>> task,
            CancellationToken cancellationToken = default)
        {
            return new List<JobResult>() { await task() };
        }
    }
}