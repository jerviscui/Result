namespace ResultCore.Tests;

public enum FileErrorCode
{
    A = 100,

    B = 101
}

public record FileError : BasicError, IError<FileError, FileErrorCode>
{

    #region Constants & Statics

    public static implicit operator FileError(FileErrorCode errorCode) => new(errorCode);

    #endregion

    public FileError() : this(FileErrorCode.A)
    {
    }

    public FileError(FileErrorCode code, string? reason = null, Exception? exception = null)
        : base((int)code, reason, exception)
    {
        Code = code;
    }

    #region IError

    /// <inheritdoc/>
    public new FileErrorCode Code
    {
        get => (FileErrorCode)base.Code;
        internal set => base.Code = (int)value;
    }

    /// <inheritdoc/>
    public static Result<FileError> Result()
    {
        return new FileError();
    }

    /// <inheritdoc/>
    public static Result<FileError> Result(FileErrorCode errorCode, string? reason = null, Exception? exception = null)
    {
        return new FileError(errorCode, reason, exception);
    }

    /// <inheritdoc/>
    public static Result<TData, FileError> Result<TData>(
        FileErrorCode errorCode,
        string? reason = null,
        Exception? exception = null)
    {
        return new FileError(errorCode, reason, exception);
    }

    #endregion

}
