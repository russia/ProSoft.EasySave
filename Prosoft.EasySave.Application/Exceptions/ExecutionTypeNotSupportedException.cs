namespace ProSoft.EasySave.Application.Exceptions;


/// <summary>
/// Easy save not supported execution type exception.
/// </summary>
public class ExecutionTypeNotSupportedException : Exception
{
    public ExecutionTypeNotSupportedException()
    {
    }

    public ExecutionTypeNotSupportedException(string message)
        : base(message)
    {
    }

    public ExecutionTypeNotSupportedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}