namespace ResultCore.Tests;

internal enum FileErrorCode
{
    A = 100,

    B = 101
}

internal record FileError : Error, IError<FileError, FileErrorCode>
{
    public FileError(FileErrorCode code, string? reason = null, Exception? exception = null)
        : base((int)code, reason, exception)
    {
        Code = code;
    }

    public FileError() : this(FileErrorCode.A)
    {
    }

    public static implicit operator FileError(FileErrorCode errorCode)
    {
        return new FileError(errorCode);
    }

    #region IError

    private FileErrorCode _code;

    /// <inheritdoc />
    public new FileErrorCode Code
    {
        get => _code;
        internal set
        {
            _code = value;
            base.Code = (int)value;
        }
    }

    /// <inheritdoc />
    public static Result<FileError> Result()
    {
        return new FileError();
    }

    /// <inheritdoc />
    public static Result<FileError> Result(FileErrorCode errorCode, string? reason = null, Exception? exception = null)
    {
        return new FileError(errorCode, reason, exception);
    }

    #endregion
}
