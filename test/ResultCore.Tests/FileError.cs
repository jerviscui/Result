namespace ResultCore.Tests;

public enum FileErrorCode
{
    Failure = 0,

    A = 100,

    B = 101
}

public readonly record struct FileError(FileErrorCode Code, string? Reason = null, Exception? Exception = null)
    : IError<FileError, FileErrorCode>
{
    public FileError() : this(FileErrorCode.A)
    {
    }

    #region IError implementations

    public static Result<FileError> Result()
    {
        return new();
    }

    public static Result<FileError> Result(FileErrorCode code, string? reason = null, Exception? exception = null)
    {
        return new FileError(code, reason, exception);
    }

    #endregion

    public static implicit operator FileError(FileErrorCode errorCode) => new(errorCode);
}
