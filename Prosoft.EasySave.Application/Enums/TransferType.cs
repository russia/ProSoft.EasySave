namespace ProSoft.EasySave.Application.Enums;

/// <summary>
/// Allows to create custom enums for the transfer type of the save work.
/// </summary>
public enum TransferType
{
    /// <summary>
    ///     Copy and replace the files if they exist
    /// </summary>
    FULL = 0,

    /// <summary>
    ///     Copies only files that do not exist in the destination folder
    /// </summary>
    DIFFERENTIAL = 1
}