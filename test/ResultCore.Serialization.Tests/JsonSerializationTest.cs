using System.Buffers;
using System.Text.Json;

namespace ResultCore.Serialization.Tests;

public class JsonSerializationTest
{

    #region Methods

    [Fact]
    public void Test()
    {
        var bufferWriter = new ArrayBufferWriter<byte>(1024);
        using var jsonWriter = new Utf8JsonWriter(bufferWriter);

        Result<MyData, FileError> result = FileError.Result(FileErrorCode.A);
        JsonSerializer.Serialize(jsonWriter, result);
        var str = JsonSerializer.Serialize(result);
        var tmp = JsonSerializer.Deserialize<Result<MyData, FileError>>(str);
        //var tmp = JsonSerializer.Deserialize<Result<MyData, FileError>>();
        //tmp.IsError().ShouldBeTrue();

        jsonWriter.Flush();
        bufferWriter.Clear();
        jsonWriter.Reset(bufferWriter);

        result = new MyData("aaa");
        JsonSerializer.Serialize(jsonWriter, result);
        str = JsonSerializer.Serialize(result);
        tmp = JsonSerializer.Deserialize<Result<MyData, FileError>>(str);
        //tmp = JsonSerializer.Deserialize<Result<MyData, FileError>>(bufferWriter.WrittenMemory);
        //tmp.Data!.Name.ShouldBe("aaa");
    }

    #endregion

}
