using Shouldly;
using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ResultCore.Serialization.Tests;

[JsonSerializable(typeof(Result<MyData, FileError>))]
[JsonSerializable(typeof(Result<FileError>))]
[JsonSerializable(typeof(MyData))]
[JsonSerializable(typeof(FileError))]
internal sealed partial class ResultSerializerContext : JsonSerializerContext
{
}

public class JsonSerializationTest
{
    private readonly JsonSerializerOptions? _options;

    public JsonSerializationTest()
    {
        _options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            TypeInfoResolver = JsonTypeInfoResolver.Combine(ResultSerializerContext.Default)
        };
    }

    #region Methods

    [Fact]
    public void Byte_Test()
    {
        var bufferWriter = new ArrayBufferWriter<byte>(1024);
        using var jsonWriter = new Utf8JsonWriter(bufferWriter);

        Result<MyData, FileError> result = FileError.Result(FileErrorCode.B);
        JsonSerializer.Serialize(jsonWriter, result, _options);
        jsonWriter.Flush();
        var jsonReader = new Utf8JsonReader(bufferWriter.WrittenSpan, default);
        var tmp = JsonSerializer.Deserialize<Result<MyData, FileError>>(ref jsonReader, _options);
        tmp.IsError(out var error).ShouldBeTrue();
        error.Value.Code.ShouldBe(FileErrorCode.B);

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
        var result = FileError.Result(FileErrorCode.B);
        var str = JsonSerializer.Serialize(result, _options);
        var tmp = JsonSerializer.Deserialize<Result<FileError>>(str, _options);
        tmp.IsError(out var error).ShouldBeTrue();
        error.Value.Code.ShouldBe(FileErrorCode.B);
    }

    #endregion

}
