namespace ResultCore;

public static class ErrorExtensions
{
    public static TError From<TError, TFError>(this TError error, TFError other)
        where TFError : Error where TError : Error
    {
        error.Code = other.Code;
        error.Reason = other.Reason;
        error.Exception = other.Exception;

        return error;
    }

    public static Error Into<TError>(this TError error)
        where TError : Error
    {
        return new Error(error.Code, error.Reason, error.Exception);
    }
}
