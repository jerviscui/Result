using System;

namespace ResultCore;

public record BasicError(int Code, string? Reason = null, Exception? Exception = null) : IError<BasicError, int>
{
    public BasicError() : this((int)BasicErrorCode.Failure)
    {
    }

    #region Properties

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public Exception? Exception { get; internal set; } = Exception;

    /// <summary>
    /// Gets the reason.
    /// </summary>
    public string? Reason { get; internal set; } = Reason;

    #endregion

    #region IError

    /// <inheritdoc/>
    public int Code { get; protected internal set; } = Code;

    /// <inheritdoc/>
    public static Result<BasicError> Result()
    {
        return new BasicError();
    }

    /// <inheritdoc/>
    public static Result<BasicError> Result(int code, string? reason = null, Exception? exception = null)
    {
        return new BasicError(code, reason, exception);
    }

    #endregion

}
