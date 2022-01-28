using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Exceptions;
using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Application.Models;

public static class Workflow
{
    public static async Task<T> StartAsync<T>(this Task<JobResult> tasks, ExecutionType executionType,
        CancellationToken cancellationToken = default) where T : class
    {
        return executionType switch
        {
            ExecutionType.SINGLE => await tasks.StartSingleJobAsync(cancellationToken) as T,
            _ => throw new ExecutionTypeNotSupportedException(
                "It is only possible to copy files sequentially when there is only one task to perform.")
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
        var jobsResults = new List<JobResult>();
        await Parallel.ForEachAsync(tasks,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = (int)maxTasks
            },
            async (x, cancellationToken) =>
            {
                x.Start();
                jobsResults.Add(await x.WaitAsync(cancellationToken));
            });
        return jobsResults;
    }

    private static async Task<IReadOnlyCollection<JobResult>> StartSequentialJobsAsync(
        this IEnumerable<Task<JobResult>> jobContexts, CancellationToken cancellationToken = default)
    {
        List<JobResult> results = new();
        foreach (var jobContext in jobContexts.Where(x => !x.IsCompleted))
        {
            jobContext.Start();
            results.Add(await jobContext.WaitAsync(cancellationToken));
        }

        return results;
    }

    private static async Task<JobResult> StartSingleJobAsync(this IEnumerable<Task<JobResult>> tasks,
        CancellationToken cancellationToken = default)
    {
        var task = tasks.First(x => !x.IsCompleted);
        task.Start();
        return await task.WaitAsync(cancellationToken);
    }

    private static async Task<JobResult> StartSingleJobAsync(this Task<JobResult> task,
        CancellationToken cancellationToken = default)
    {
        task.Start();
        return await task.WaitAsync(cancellationToken);
    }
}