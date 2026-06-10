namespace ResultCore;

public class NotInitializeException : Exception
{
    public NotInitializeException()
    {
    }

    public NotInitializeException(string message) : base(message)
    {
    }

    public NotInitializeException(string message, Exception inner) : base(message, inner)
    {
    }
}
