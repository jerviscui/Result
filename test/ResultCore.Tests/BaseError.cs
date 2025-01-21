namespace ResultCore.Tests;

public enum BaseErrorCode
{
    Failed = 10,

    NotFound = 99
}

public record BaseError : BasicError, IError<BaseError, BaseErrorCode>
{

    #region Constants & Statics

    public static implicit operator BaseError(BaseErrorCode errorCode) => new(errorCode);

    #endregion

    public BaseError() : this(BaseErrorCode.Failed)
    {
    }

    public BaseError(BaseErrorCode code, string? reason = null, Exception? exception = null)
        : base((int)code, reason, exception)
    {
        Code = code;
    }

    #region IError

    /// <inheritdoc/>
    public new BaseErrorCode Code
    {
        get => (BaseErrorCode)base.Code;
        internal set => base.Code = (int)value;
    }

    /// <inheritdoc/>
    public static Result<BaseError> Result()
    {
        return new BaseError();
    }

    /// <inheritdoc/>
    public static Result<BaseError> Result(BaseErrorCode errorCode, string? reason = null, Exception? exception = null)
    {
        return new BaseError(errorCode, reason, exception);
    }

    #endregion

}
