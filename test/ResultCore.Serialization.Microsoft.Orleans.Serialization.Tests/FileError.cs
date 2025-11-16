using Orleans;

namespace ResultCore.Serialization.Microsoft.Orleans.Serialization.Tests;

public enum FileErrorCode
{
    Failure = 0,

    A = 100,

    B = 101
}

//struct 16byte
[GenerateSerializer]
[Alias("ResultCore.Serialization.Microsoft.Orleans.Serialization.FileError")]
[Immutable]
public readonly record struct FileError(FileErrorCode Code, string? Reason = null) : IError<FileError, FileErrorCode>
{

    #region Constants & Statics

    public static Result<FileError> Result(FileErrorCode code, string? reason = null)
    {
        return new Result<FileError>(new FileError(code, reason));
    }
    #endregion

    public FileError() : this(FileErrorCode.A)
    {
    }

    #region IError implementations

    public static Result<FileError> Result()
    {
        return new Result<FileError>(new FileError());
    }

    public static Result<FileError> Result(FileErrorCode code)
    {
        return new Result<FileError>(new FileError(code));
    }
    #endregion

    public static implicit operator FileError(FileErrorCode errorCode) => new(errorCode);

}
