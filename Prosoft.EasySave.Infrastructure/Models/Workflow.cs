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
        public static async Task<T> StartAsync<T>(this Task<JobResult> tasks, ExecutionType executionType,
            CancellationToken cancellationToken = default) where T : class
        {
            return executionType switch
            {
                ExecutionType.SINGLE => await tasks.StartSingleJobAsync(cancellationToken) as T,
                _ => throw new ExecutionTypeNotSupportedException("It is only possible to copy files sequentially when there is only one task to perform.")
            };
        }

        public static async Task<T> StartAsync<T>(this IEnumerable<Task<JobResult>> tasks, ExecutionType executionType,
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
            this IEnumerable<Task<JobResult>> tasks, uint maxTasks = 5, CancellationToken cancellationToken = default)
        {
            var tasksList = new List<Task<JobResult>>();
            Parallel.ForEach(tasks,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = (int)maxTasks
                },
                (x, cancellationToken) =>
                {
                    tasksList.Add(x);
                    x.Start();
                });

            var jobResults = await Task.WhenAll(tasksList);
            return jobResults;
        }

        private static async Task<IReadOnlyCollection<JobResult>> StartSequentialJobsAsync(
            this IEnumerable<Task<JobResult>> jobContexts, CancellationToken cancellationToken = default)
        {
            List<JobResult> results = new();
            foreach (var jobContext in jobContexts.Where(x => !x.IsCompleted))
            {
                jobContext.Start();
                results.Add(await jobContext);
            }

            return results;
        }

        private static async Task<IReadOnlyCollection<JobResult>> StartSingleJobAsync(this IEnumerable<Task<JobResult>> tasks,
            CancellationToken cancellationToken = default)
        {
            var task = tasks.First(x => !x.IsCompleted);
            task.Start();
            var result = await task;
            return new List<JobResult>() { result };
        }

        private static async Task<IReadOnlyCollection<JobResult>> StartSingleJobAsync(this Task<JobResult> task,
            CancellationToken cancellationToken = default)
        {
            task.Start();
            var result = await task;
            return new List<JobResult>() { result };
        }
    }
}