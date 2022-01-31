using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Console.Interfaces;

internal interface IConsoleService
{
    /// <summary>
    /// Method to display the console interface.
    /// </summary>
    /// <returns></returns>
    Task DisplayConsoleInterface();

    /// <summary>
    /// Method to add a save work from the console interface.
    /// </summary>
    /// <param name="name">The save name.</param>
    /// <param name="transferType">The transfer type.</param>
    /// <param name="sourcePath">The source path.</param>
    /// <param name="destinationPath">The destination path.</param>
    void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath);


    /// <summary>
    /// Method to start the saves.
    /// </summary>
    /// <param name="executionType">The execution type (nullable)</param>
    /// <returns>The job results list.</returns>
    Task<IReadOnlyCollection<JobResult>> Start(ExecutionType? executionType = null);
}