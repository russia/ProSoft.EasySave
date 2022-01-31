namespace ProSoft.EasySave.Application.Enums;


/// <summary>
/// Allows to create custom enum types for the execution type.
/// </summary>
public enum ExecutionType
{
    /// <summary>
    ///     Sequentially run all backup jobs.
    /// </summary>
    SEQUENTIAL,

    /// <summary>
    ///     Launches all backup work in parallel.
    /// </summary>
    CONCURRENT,

    /// <summary>
    ///     Starts only 1 backup job.
    /// </summary>
    SINGLE
}