using MemoryPack;
using MessagePack;
using Orleans;

namespace ResultCore.Serialization.Tests;

public enum FileErrorCode
{
    Failure = 0,

    A = 100,

    B = 101
}

//struct 16byte
[MemoryPackable]
[GenerateSerializer]
[Immutable]
[Alias("ResultCore.Serialization.Tests.FileError")]
[MessagePackObject]
public readonly partial record struct FileError(
    [property: Key(0)] FileErrorCode Code,
    [property: Key(1)] string? Reason = null)
    : IError<FileError, FileErrorCode>
{
    [MemoryPackConstructor]
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
