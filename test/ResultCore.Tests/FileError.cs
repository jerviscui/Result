namespace ResultCore.Tests;

public enum FileErrorCode
{
    Failure = 0,

    A = 100,

    B = 101
}

public readonly record struct FileError(FileErrorCode Code) : IError<FileError, FileErrorCode>
{
    public FileError() : this(FileErrorCode.A)
    {
    }

    #region IError implementations

    public static Result<FileError> Result()
    {
        return new FileError();
    }

    public static Result<FileError> Result(FileErrorCode code)
    {
        return new FileError(code);
    }

    #endregion

    public static implicit operator FileError(FileErrorCode errorCode) => new(errorCode);
}
