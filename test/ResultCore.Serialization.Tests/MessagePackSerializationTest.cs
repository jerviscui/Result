using MessagePack;
using Shouldly;
using System.Buffers;

namespace ResultCore.Serialization.Tests;

public class MessagePackSerializationTest
{

    #region Methods

    [Fact]
    public void Test()
    {
        var bufferWriter = new ArrayBufferWriter<byte>(1024);

        Result<MyData, FileError> result = FileError.Result(FileErrorCode.A);
        MessagePackSerializer.Serialize(bufferWriter, result, cancellationToken: default);
        var tmp = MessagePackSerializer.Deserialize<Result<MyData, FileError>>(
            bufferWriter.WrittenMemory,
            cancellationToken: default);
        tmp.IsError().ShouldBeTrue();

        bufferWriter.Clear();

        result = new MyData("aaa");
        MessagePackSerializer.Serialize(bufferWriter, result, cancellationToken: default);
        tmp = MessagePackSerializer.Deserialize<Result<MyData, FileError>>(
            bufferWriter.WrittenMemory,
            cancellationToken: default);
        tmp.Data!.Name.ShouldBe("aaa");
    }

    #endregion

}
