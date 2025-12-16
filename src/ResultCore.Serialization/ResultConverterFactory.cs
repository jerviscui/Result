using System.Text.Json;
using System.Text.Json.Serialization;

namespace ResultCore;

public class ResultConverterFactory : JsonConverterFactory
{

    #region Methods

    public override bool CanConvert(Type typeToConvert)
    {
        return true;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var args = typeToConvert.GetGenericArguments();

        if (args.Length == 1)
        {
            var errorType = args[0];
            var converterType = typeof(ResultConverter<>).MakeGenericType(errorType);

            return (JsonConverter?)Activator.CreateInstance(converterType, options);
        }
        else if (args.Length == 2)
        {
            var dataType = args[0];
            var errorType = args[1];
            var converterType = typeof(ResultConverter<,>).MakeGenericType(dataType, errorType);

            // 将 options 传给 Converter 构造函数，以便预先获取子转换器
            return (JsonConverter?)Activator.CreateInstance(converterType, options);
        }
        else
        {
            throw new JsonException($"ResultConverterFactory unsupported type: {typeToConvert}");
        }
    }

    #endregion

}

internal static class PropNames
{

    #region Constants & Statics

    // C# 11+ 支持 "string"u8 语法直接生成 ReadOnlySpan<byte>
    internal static ReadOnlySpan<byte> DataCamelProp => "data"u8;

    internal static ReadOnlySpan<byte> DataProp => "Data"u8;

    internal static ReadOnlySpan<byte> ErrorCamelProp => "error"u8;

    internal static ReadOnlySpan<byte> ErrorProp => "Error"u8;

    internal static ReadOnlySpan<byte> HasErrorCamelProp => "hasError"u8;

    internal static ReadOnlySpan<byte> HasErrorProp => "HasError"u8;

    internal static ReadOnlySpan<byte> GetDataProp(JsonSerializerOptions options)
    {
        return options.PropertyNameCaseInsensitive ? DataCamelProp : DataProp;
    }

    internal static ReadOnlySpan<byte> GetErrorProp(JsonSerializerOptions options)
    {
        return options.PropertyNameCaseInsensitive ? ErrorCamelProp : ErrorProp;
    }

    internal static ReadOnlySpan<byte> GetHasErrorProp(JsonSerializerOptions options)
    {
        return options.PropertyNameCaseInsensitive ? HasErrorCamelProp : HasErrorProp;
    }

    #endregion

}

public class ResultConverter<TData, TError> : JsonConverter<Result<TData, TError>>
    where TData : class?
    where TError : struct
{
    private readonly JsonConverter<TData> _dataConverter;
    private readonly JsonConverter<TError> _errorConverter;
    private readonly Type _dataType;
    private readonly Type _errorType;

    public ResultConverter(JsonSerializerOptions options)
    {
        _dataType = typeof(TData);
        _errorType = typeof(TError);
        _dataConverter = (JsonConverter<TData>)options.GetConverter(_dataType);
        _errorConverter = (JsonConverter<TError>)options.GetConverter(_errorType);
    }

    #region Methods

    public override Result<TData, TError> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Read failed.");
        }

        TData? data = null;
        TError error = default;
        var hasError = false;
        var dataProp = PropNames.GetDataProp(options);
        var errorProp = PropNames.GetErrorProp(options);
        var hasErrorProp = PropNames.GetHasErrorProp(options);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                // ValueSpan 指向内部缓冲区的原始字节
                var propertyName = reader.ValueSpan;
                // get next token
                if (!reader.Read())
                {
                    break;
                }

                if (propertyName.SequenceEqual(dataProp))
                {
                    data = _dataConverter.Read(ref reader, _dataType, options);
                }
                else if (propertyName.SequenceEqual(errorProp))
                {
                    error = _errorConverter.Read(ref reader, _errorType, options);
                }
                else if (propertyName.SequenceEqual(hasErrorProp))
                {
                    hasError = reader.GetBoolean();
                }
                else
                {
                    reader.Skip();
                }
            }
            else if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Result<TData, TError>(error, hasError, data);
            }
            else
            {
                //continue
            }
        }

        throw new JsonException("Read failed.");
    }

    public override void Write(Utf8JsonWriter writer, Result<TData, TError> value, JsonSerializerOptions options)
    {
        var dataProp = PropNames.GetDataProp(options);
        var errorProp = PropNames.GetErrorProp(options);
        var hasErrorProp = PropNames.GetHasErrorProp(options);

        writer.WriteStartObject();

        if (value.Data == null)
        {
            writer.WriteNull(dataProp);
        }
        else
        {
            writer.WritePropertyName(dataProp);
            _dataConverter.Write(writer, value.Data, options);
        }

        writer.WritePropertyName(errorProp);
        _errorConverter.Write(writer, value.error, options);

        writer.WriteBoolean(hasErrorProp, value.hasError);

        writer.WriteEndObject();
    }

    #endregion

}

public class ResultConverter<TError> : JsonConverter<Result<TError>>
    where TError : struct
{
    private readonly JsonConverter<TError> _errorConverter;
    private readonly Type _errorType;

    public ResultConverter(JsonSerializerOptions options)
    {
        _errorType = typeof(TError);
        _errorConverter = (JsonConverter<TError>)options.GetConverter(_errorType);
    }

    #region Methods

    public override Result<TError> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Read failed.");
        }

        TError error = default;
        var hasError = false;

        var errorProp = PropNames.GetErrorProp(options);
        var hasErrorProp = PropNames.GetHasErrorProp(options);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                // ValueSpan 指向内部缓冲区的原始字节
                var propertyName = reader.ValueSpan;
                // get next token
                if (!reader.Read())
                {
                    break;
                }

                if (propertyName.SequenceEqual(errorProp))
                {
                    error = _errorConverter.Read(ref reader, _errorType, options);
                }
                else if (propertyName.SequenceEqual(hasErrorProp))
                {
                    hasError = reader.GetBoolean();
                }
                else
                {
                    reader.Skip();
                }
            }
            else if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Result<TError>(error, hasError);
            }
            else
            {
                //continue
            }
        }

        throw new JsonException("Read failed.");
    }

    public override void Write(Utf8JsonWriter writer, Result<TError> value, JsonSerializerOptions options)
    {
        var errorProp = PropNames.GetErrorProp(options);
        var hasErrorProp = PropNames.GetHasErrorProp(options);

        writer.WriteStartObject();

        writer.WritePropertyName(errorProp);
        _errorConverter.Write(writer, value.error, options);

        writer.WriteBoolean(hasErrorProp, value.hasError);

        writer.WriteEndObject();
    }

    #endregion

}
