namespace ProSoft.EasySave.Application.Enums;

public enum ExecutionType
{
    /// <summary>
    /// Sequentially run all backup jobs.
    /// </summary>
    SEQUENTIAL,

    /// <summary>
    /// Launches all backup work in parallel.
    /// </summary>
    CONCURRENT,

    /// <summary>
    /// Starts only 1 backup job.
    /// </summary>
    SINGLE
}