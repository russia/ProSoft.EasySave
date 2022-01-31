using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Application.Interfaces.Services;

public interface IJobFactoryService
{
    /// <summary>
    /// Method allowing to load the configuration stored in the configuration file.
    /// </summary>
    void LoadConfiguration();


    /// <summary>
    /// Method allowing to add a new save job inside of the Job list.
    /// </summary>
    /// <param name="name">The job name.</param>
    /// <param name="transferType">The job transfer type.</param>
    /// <param name="sourcePath">The source path.</param>
    /// <param name="destinationPath">The destination path.</param>
    void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath);


    /// <summary>
    /// Method allowing to start the save jobs if the execution type is null.
    /// </summary>
    /// <param name="executionType">The execution type.</param>
    /// <returns>Returns a list containing the job results.</returns>
    Task<IReadOnlyCollection<JobResult>> StartJobsAsync(ExecutionType? executionType = null);
}