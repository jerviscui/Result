using Shouldly;
using System.Buffers;
using System.Text.Json;

namespace ResultCore.Serialization.Tests;

public class JsonSerializationTest
{
    private readonly JsonSerializerOptions? _options;

    public JsonSerializationTest()
    {
        _options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    #region Methods

    [Fact]
    public void Byte_Test()
    {
        var bufferWriter = new ArrayBufferWriter<byte>(1024);
        using var jsonWriter = new Utf8JsonWriter(bufferWriter);

        Result<MyData, FileError> result = FileError.Result(FileErrorCode.A);
        JsonSerializer.Serialize(jsonWriter, result, _options);
        jsonWriter.Flush();
        var jsonReader = new Utf8JsonReader(bufferWriter.WrittenSpan, default);
        var tmp = JsonSerializer.Deserialize<Result<MyData, FileError>>(ref jsonReader, _options);
        tmp.IsError(out var error).ShouldBeTrue();
        error.Value.Code.ShouldBe(FileErrorCode.A);

        jsonWriter.Flush();
        bufferWriter.Clear();
        jsonWriter.Reset(bufferWriter);

        result = new MyData("aaa");
        JsonSerializer.Serialize(jsonWriter, result, _options);
        jsonWriter.Flush();
        jsonReader = new Utf8JsonReader(bufferWriter.WrittenSpan, default);
        tmp = JsonSerializer.Deserialize<Result<MyData, FileError>>(ref jsonReader, _options);
        tmp.Data!.Name.ShouldBe("aaa");
    }

    [Fact]
    public void Str_Test()
    {
        Result<MyData, FileError> result = FileError.Result(FileErrorCode.A);
        var str = JsonSerializer.Serialize(result, _options);
        var tmp = JsonSerializer.Deserialize<Result<MyData, FileError>>(str, _options);
        tmp.IsError(out var error).ShouldBeTrue();
        error.Value.Code.ShouldBe(FileErrorCode.A);

        result = new MyData("aaa");
        str = JsonSerializer.Serialize(result, _options);
        tmp = JsonSerializer.Deserialize<Result<MyData, FileError>>(str, _options);
        tmp.Data!.Name.ShouldBe("aaa");
    }

    #endregion

}
