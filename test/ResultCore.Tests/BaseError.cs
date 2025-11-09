namespace ResultCore.Tests;

public enum BaseErrorCode
{
    Failure = 0,

    Failed = 10,

    NotFound = 99
}

public readonly record struct BaseError(BaseErrorCode Code, string? Reason = null, Exception? Exception = null)
    : IError<BaseError, BaseErrorCode>
{
    public BaseError() : this(BaseErrorCode.Failed)
    {
    }

    #region IError implementations

    public static Result<BaseError> Result()
    {
        return new BaseError();
    }

    public static Result<BaseError> Result(BaseErrorCode code, string? reason = null, Exception? exception = null)
    {
        return new BaseError(code, reason, exception);
    }

    #endregion

    public static implicit operator BaseError(BaseErrorCode errorCode) => new(errorCode);
}
