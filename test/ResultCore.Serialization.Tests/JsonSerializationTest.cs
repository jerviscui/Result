using Shouldly;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ResultCore.Serialization.Tests;

[JsonSerializable(typeof(Result<MyData, FileError>))]
[JsonSerializable(typeof(Result<FileError>))]
[JsonSerializable(typeof(MyData))]
[JsonSerializable(typeof(FileError))]
internal sealed partial class ResultSerializerContext : JsonSerializerContext;

public class JsonSerializationTest
{

    #region Constants & Statics

    private static readonly JsonSerializerOptions CaseInsensitivePascalOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(ResultSerializerContext.Default)
    };

    private static readonly JsonSerializerOptions CaseSensitiveCamelCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(ResultSerializerContext.Default)
    };

    private static ReadOnlySequence<byte> CreateMultiSegmentSequence(params string[] segments)
    {
        BufferSegment? first = null;
        BufferSegment? last = null;

        foreach (var segment in segments)
        {
            if (first is null)
            {
                first = new BufferSegment(Encoding.UTF8.GetBytes(segment));
                last = first;
                continue;
            }

            last = last!.Append(Encoding.UTF8.GetBytes(segment));
        }

        return new ReadOnlySequence<byte>(first!, 0, last!, last!.Memory.Length);
    }

    #endregion

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
    public void Deserialize_Should_Leave_Result_Uninitialized_When_HasError_Is_Missing()
    {
        const string json = """{"data":{"name":"aaa"},"error":{"code":0}}""";

        var result = JsonSerializer.Deserialize<Result<MyData, FileError>>(json, _options);

        Should.Throw<NotInitializeException>(() => result.IsError());
    }

    [Fact]
    public void Deserialize_Should_Skip_Unknown_Properties()
    {
        const string json = """
                            {
                              "trace": { "id": "abc", "items": [1, 2, 3] },
                              "hasError": false,
                              "data": { "name": "aaa" },
                              "error": { "code": 0 }
                            }
                            """;

        var result = JsonSerializer.Deserialize<Result<MyData, FileError>>(json, _options);

        result.IsError().ShouldBeFalse();
        result.Data.ShouldNotBeNull();
        result.Data.Name.ShouldBe("aaa");
    }

    [Fact]
    public void Deserialize_Should_Support_MultiSegment_PropertyNames()
    {
        var jsonSequence = CreateMultiSegmentSequence(
            "{\"has",
            "Error\":false,\"da",
            "ta\":{\"name\":\"aaa\"},\"er",
            "ror\":{\"code\":0}}");
        var jsonReader = new Utf8JsonReader(jsonSequence, true, default);

        var result = JsonSerializer.Deserialize<Result<MyData, FileError>>(ref jsonReader, _options);

        result.IsError().ShouldBeFalse();
        result.Data.ShouldNotBeNull();
        result.Data.Name.ShouldBe("aaa");
    }

    [Fact]
    public void Deserialize_Should_Use_PropertyNamingPolicy_When_CaseSensitive()
    {
        const string json = """{"error":{"code":101},"hasError":true}""";

        var result = JsonSerializer.Deserialize<Result<FileError>>(json, CaseSensitiveCamelCaseOptions);

        result.IsError(out var error).ShouldBeTrue();
        error.Value.Code.ShouldBe(FileErrorCode.B);
    }

    [Fact]
    public async Task DeserializeAsync_Should_Support_MultiSegment_PropertyNames_From_PipeReader()
    {
        var pipe = new Pipe();
        var jsonSegments = new[]
        {
            "{\"has", "Error\":false,\"da", "ta\":{\"name\":\"aaa\"},\"er", "ror\":{\"code\":0}}"
        };

        foreach (var segment in jsonSegments)
        {
            await pipe.Writer.WriteAsync(Encoding.UTF8.GetBytes(segment), TestContext.Current.CancellationToken);
        }

        await pipe.Writer.CompleteAsync();

        var result = await JsonSerializer.DeserializeAsync<Result<MyData, FileError>>(
            pipe.Reader,
            _options,
            TestContext.Current.CancellationToken);
        await pipe.Reader.CompleteAsync();

        result.IsError().ShouldBeFalse();
        result.Data.ShouldNotBeNull();
        result.Data.Name.ShouldBe("aaa");
    }

    [Fact]
    public void Serialize_Should_Use_PropertyNamingPolicy_Instead_Of_PropertyNameCaseInsensitive()
    {
        var result = FileError.Result(FileErrorCode.B);

        var json = JsonSerializer.Serialize(result, CaseInsensitivePascalOptions);

        json.ShouldBe(/*lang=json,strict*/"""{"Error":{"Code":101,"Reason":null},"HasError":true}""");
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

    private sealed class BufferSegment : ReadOnlySequenceSegment<byte>
    {
        public BufferSegment(ReadOnlyMemory<byte> memory)
        {
            Memory = memory;
        }

        #region Methods

        public BufferSegment Append(ReadOnlyMemory<byte> memory)
        {
            var segment = new BufferSegment(memory) { RunningIndex = RunningIndex + Memory.Length };
            Next = segment;
            return segment;
        }

        #endregion
    }
}
