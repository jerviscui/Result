using System.Runtime.CompilerServices;

namespace ResultCore.Tests;

public interface IBaseError<TError, TCode> : IError<TError, TCode>
    where TError : struct
{

    #region Constants & Statics

    /// <summary>
    /// Create a result with the specified code, reason and exception.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="reason">The reason.</param>
    /// <param name="exception">The exception.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static abstract Result<TError> Result(TCode code, string? reason = null, Exception? exception = null);

    #endregion

    #region Properties

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Gets the reason.
    /// </summary>
    public string? Reason { get; }

    #endregion

}

public enum BaseErrorCode
{
    Failure = 0,

    Failed = 10,

    NotFound = 99
}

//struct 24byte
public readonly record struct BaseError(BaseErrorCode Code, string? Reason = null, Exception? Exception = null)
    : IBaseError<BaseError, BaseErrorCode>
{
    public BaseError() : this(BaseErrorCode.Failed)
    {
    }

    #region IBaseError implementations

    public static Result<BaseError> Result(BaseErrorCode code, string? reason = null, Exception? exception = null)
    {
        return new Result<BaseError>(new BaseError(code, reason, exception));
    }

    #endregion

    #region IError implementations

    public static Result<BaseError> Result()
    {
        return new Result<BaseError>(new BaseError());
    }

    public static Result<BaseError> Result(BaseErrorCode code)
    {
        return new Result<BaseError>(new BaseError(code));
    }

    #endregion

    public static implicit operator BaseError(BaseErrorCode errorCode) => new(errorCode);
}
