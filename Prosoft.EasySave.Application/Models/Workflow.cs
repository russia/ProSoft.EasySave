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

    /// <summary>
    /// Method allowing to call the method to launch save works according to their execution type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tasks">The tasks list.</param>
    /// <param name="executionType">The execution type.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the T result.</returns>
    /// <exception cref="NotImplementedException">This exception is throw if the execution type is not supported.</exception>
    public static async Task<T> StartAsync<T>(this IEnumerable<Task<JobResult>> tasks, ExecutionType executionType, CancellationToken cancellationToken = default) where T : class
    {
        return executionType switch
        {
            ExecutionType.SEQUENTIAL => await tasks.StartSequentialJobsAsync(cancellationToken) as T,
            ExecutionType.CONCURRENT => await tasks.StartParallelJobsAsync(cancellationToken: cancellationToken) as T,
            ExecutionType.SINGLE => await tasks.StartSingleJobAsync(cancellationToken) as T,
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Method to launch the save works in parallel.
    /// </summary>
    /// <param name="tasks">The tasks list.</param>
    /// <param name="maxTasks">The concurrency tasks limit.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the T result.</returns>
    private static async Task<IReadOnlyCollection<JobResult>> StartParallelJobsAsync(this IEnumerable<Task<JobResult>> tasks, uint maxTasks = 5, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Method allowing to launch the save works sequentially one by one until 5 save works were launched.
    /// </summary>
    /// <param name="jobContexts">The task list.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task containing the job results list.</returns>
    private static async Task<IReadOnlyCollection<JobResult>> StartSequentialJobsAsync(this IEnumerable<Task<JobResult>> jobContexts, CancellationToken cancellationToken = default)
    {
        List<JobResult> results = new();
        foreach (var jobContext in jobContexts.Where(x => !x.IsCompleted))
        {
            jobContext.Start();
            results.Add(await jobContext.WaitAsync(cancellationToken));
        }

        return results;
    }

    /// <summary>
    /// Method allowing to launch one save work. 
    /// </summary>
    /// <param name="tasks">The task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the job result.</returns>
    private static async Task<JobResult> StartSingleJobAsync(this IEnumerable<Task<JobResult>> tasks, CancellationToken cancellationToken = default)
    {
        var task = tasks.First(x => !x.IsCompleted);
        task.Start();
        return await task.WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Same method as the one above.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the job result.</returns>
    private static async Task<JobResult> StartSingleJobAsync(this Task<JobResult> task, CancellationToken cancellationToken = default)
    {
        task.Start();
        return await task.WaitAsync(cancellationToken);
    }
}