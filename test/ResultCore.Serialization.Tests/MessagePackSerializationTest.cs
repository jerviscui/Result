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
        MessagePackSerializer.Serialize(bufferWriter, result, cancellationToken: TestContext.Current.CancellationToken);
        var tmp = MessagePackSerializer.Deserialize<Result<MyData, FileError>>(
            bufferWriter.WrittenMemory,
            cancellationToken: TestContext.Current.CancellationToken);
        tmp.IsError().ShouldBeTrue();

        bufferWriter.Clear();

        result = new MyData("aaa");
        MessagePackSerializer.Serialize(bufferWriter, result, cancellationToken: TestContext.Current.CancellationToken);
        tmp = MessagePackSerializer.Deserialize<Result<MyData, FileError>>(
            bufferWriter.WrittenMemory,
            cancellationToken: TestContext.Current.CancellationToken);
        tmp.Data!.Name.ShouldBe("aaa");
    }

    #endregion

}
