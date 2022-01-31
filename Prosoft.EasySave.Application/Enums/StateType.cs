namespace ProSoft.EasySave.Application.Enums;

/// <summary>
/// Allows to create custom enum for the state of the save work.
/// </summary>
public enum StateType
{
    /// <summary>
    /// The save work was completed.
    /// </summary>
    COMPLETED,

    /// <summary>
    /// The save work is currently running.
    /// </summary>
    PROCESSING,

    /// <summary>
    /// The save work is waiting.
    /// </summary>
    WAITING
}