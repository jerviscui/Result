using System;

namespace ResultCore;

public record Error(int Code, string? Reason = null, Exception? Exception = null) : IError<Error, int>
{
    public Error() : this((int)BasicErrorCode.Failure)
    {
    }

    /// <summary>
    /// Gets the reason.
    /// </summary>
    public string? Reason { get; internal set; } = Reason;

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public Exception? Exception { get; internal set; } = Exception;

    #region IError

    /// <inheritdoc />
    public int Code { get; protected internal set; } = Code;

    public static Result<Error> Result()
    {
        return new Error();
    }

    /// <inheritdoc />
    public static Result<Error> Result(int code, string? reason = null, Exception? exception = null)
    {
        return new Error(code, reason, exception);
    }

    #endregion
}
