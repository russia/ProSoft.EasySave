namespace ProSoft.EasySave.Application.Exceptions;

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