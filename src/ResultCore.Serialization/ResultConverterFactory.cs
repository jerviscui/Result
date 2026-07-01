using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ResultCore;

public class ResultConverterFactory : JsonConverterFactory
{

    #region Methods

    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        var genericType = typeToConvert.GetGenericTypeDefinition();
        return genericType == typeof(Result<>) || genericType == typeof(Result<,>);
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

    // Result<object, int>.Data
    private const string DataProp = "Data";
    // Result<object, int>.error
    private const string ErrorProp = "Error";
    // Result<object, int>.hasError
    private const string HasErrorProp = "HasError";

    internal static PropertyNameMatcher CreateDataMatcher(JsonSerializerOptions options)
    {
        return new PropertyNameMatcher(DataProp, options);
    }

    internal static PropertyNameMatcher CreateErrorMatcher(JsonSerializerOptions options)
    {
        return new PropertyNameMatcher(ErrorProp, options);
    }

    internal static PropertyNameMatcher CreateHasErrorMatcher(JsonSerializerOptions options)
    {
        return new PropertyNameMatcher(HasErrorProp, options);
    }

    #endregion

    internal sealed class PropertyNameMatcher
    {
        private readonly byte[]? _alternateNameUtf8;
        private readonly byte[] _expectedNameUtf8;
        private readonly bool _propertyNameCaseInsensitive;

        internal PropertyNameMatcher(string propertyName, JsonSerializerOptions options)
        {
            ExpectedName = options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName;

            _expectedNameUtf8 = Encoding.UTF8.GetBytes(ExpectedName);
            _alternateNameUtf8 = null;
            _propertyNameCaseInsensitive = options.PropertyNameCaseInsensitive;
            if (_propertyNameCaseInsensitive && !string.Equals(ExpectedName, propertyName, StringComparison.Ordinal))
            {
                _alternateNameUtf8 = Encoding.UTF8.GetBytes(propertyName);
            }
        }

        #region Properties

        internal string ExpectedName { get; }

        #endregion

        #region Methods

        internal bool IsMatch(ref Utf8JsonReader reader)
        {
            if (reader.ValueTextEquals(_expectedNameUtf8))
            {
                return true;
            }

            if (_alternateNameUtf8 is not null && reader.ValueTextEquals(_alternateNameUtf8))
            {
                return true;
            }

            if (!_propertyNameCaseInsensitive)
            {
                return false;
            }

            var actualName = reader.GetString();
            return string.Equals(actualName, ExpectedName, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}

internal enum ResultProperty
{
    Unknown,
    Data,
    Error,
    HasError
}

internal static class JsonExceptionMessages
{

    #region Constants & Statics

    internal const string ExpectedObject = "Expected JSON object for Result.";
    internal const string InvalidResultJson = "Invalid Result JSON.";
    internal const string UnexpectedPropertyState = "Unexpected Result property state.";

    #endregion

}

public class ResultConverter<TData, TError> : JsonConverter<Result<TData, TError>>
    where TData : class?
    where TError : struct
{

    #region Constants & Statics

    private static bool? ReadHasError(ref Utf8JsonReader reader)
    {
        return reader.TokenType == JsonTokenType.Null ? null : reader.GetBoolean();
    }

    #endregion

    private readonly JsonConverter<TData> _dataConverter;
    private readonly JsonConverter<TError> _errorConverter;
    private readonly PropNames.PropertyNameMatcher _dataPropMatcher;
    private readonly PropNames.PropertyNameMatcher _errorPropMatcher;
    private readonly PropNames.PropertyNameMatcher _hasErrorPropMatcher;
    private readonly Type _dataType;
    private readonly Type _errorType;

    public ResultConverter(JsonSerializerOptions options)
    {
        _dataType = typeof(TData);
        _errorType = typeof(TError);
        _dataConverter = (JsonConverter<TData>)options.GetConverter(_dataType);
        _errorConverter = (JsonConverter<TError>)options.GetConverter(_errorType);
        _dataPropMatcher = PropNames.CreateDataMatcher(options);
        _errorPropMatcher = PropNames.CreateErrorMatcher(options);
        _hasErrorPropMatcher = PropNames.CreateHasErrorMatcher(options);
    }

    #region Methods

    private ResultProperty GetProperty(ref Utf8JsonReader reader)
    {
        if (_dataPropMatcher.IsMatch(ref reader))
        {
            return ResultProperty.Data;
        }

        if (_errorPropMatcher.IsMatch(ref reader))
        {
            return ResultProperty.Error;
        }

        return _hasErrorPropMatcher.IsMatch(ref reader) ? ResultProperty.HasError : ResultProperty.Unknown;
    }

    public override Result<TData, TError> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException(JsonExceptionMessages.ExpectedObject);
        }

        TData? data = null;
        TError error = default;
        bool? hasError = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var property = GetProperty(ref reader);

                if (!reader.Read())
                {
                    break;
                }

                switch (property)
                {
                    case ResultProperty.Data:
                        data = _dataConverter.Read(ref reader, _dataType, options);
                        break;

                    case ResultProperty.Error:
                        error = _errorConverter.Read(ref reader, _errorType, options);
                        break;

                    case ResultProperty.HasError:
                        hasError = ReadHasError(ref reader);
                        break;

                    case ResultProperty.Unknown:
                        reader.Skip();
                        break;

                    default:
                        throw new JsonException(JsonExceptionMessages.UnexpectedPropertyState);
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

        throw new JsonException(JsonExceptionMessages.InvalidResultJson);
    }

    public override void Write(Utf8JsonWriter writer, Result<TData, TError> value, JsonSerializerOptions options)
    {
        var ignoreNull = options.DefaultIgnoreCondition is JsonIgnoreCondition.WhenWritingNull;

        writer.WriteStartObject();

        if (value.Data == null)
        {
            if (!ignoreNull)
            {
                writer.WriteNull(_dataPropMatcher.ExpectedName);
            }
        }
        else
        {
            writer.WritePropertyName(_dataPropMatcher.ExpectedName);
            _dataConverter.Write(writer, value.Data, options);
        }

        writer.WritePropertyName(_errorPropMatcher.ExpectedName);
        _errorConverter.Write(writer, value.error, options);

        if (value.hasError == null)
        {
            if (!ignoreNull)
            {
                writer.WriteNull(_hasErrorPropMatcher.ExpectedName);
            }
        }
        else
        {
            writer.WriteBoolean(_hasErrorPropMatcher.ExpectedName, value.hasError.Value);
        }

        writer.WriteEndObject();
    }

    #endregion

}

public class ResultConverter<TError> : JsonConverter<Result<TError>>
    where TError : struct
{

    #region Constants & Statics

    private static bool? ReadHasError(ref Utf8JsonReader reader)
    {
        return reader.TokenType == JsonTokenType.Null ? null : reader.GetBoolean();
    }

    #endregion

    private readonly JsonConverter<TError> _errorConverter;
    private readonly PropNames.PropertyNameMatcher _errorPropMatcher;
    private readonly PropNames.PropertyNameMatcher _hasErrorPropMatcher;
    private readonly Type _errorType;

    public ResultConverter(JsonSerializerOptions options)
    {
        _errorType = typeof(TError);
        _errorConverter = (JsonConverter<TError>)options.GetConverter(_errorType);
        _errorPropMatcher = PropNames.CreateErrorMatcher(options);
        _hasErrorPropMatcher = PropNames.CreateHasErrorMatcher(options);
    }

    #region Methods

    private ResultProperty GetProperty(ref Utf8JsonReader reader)
    {
        if (_errorPropMatcher.IsMatch(ref reader))
        {
            return ResultProperty.Error;
        }

        return _hasErrorPropMatcher.IsMatch(ref reader) ? ResultProperty.HasError : ResultProperty.Unknown;
    }

    public override Result<TError> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException(JsonExceptionMessages.ExpectedObject);
        }

        TError error = default;
        bool? hasError = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var property = GetProperty(ref reader);

                if (!reader.Read())
                {
                    break;
                }

                switch (property)
                {
                    case ResultProperty.Error:
                        error = _errorConverter.Read(ref reader, _errorType, options);
                        break;

                    case ResultProperty.HasError:
                        hasError = ReadHasError(ref reader);
                        break;

                    case ResultProperty.Unknown:
                        reader.Skip();
                        break;

                    case ResultProperty.Data:
                    default:
                        throw new JsonException(JsonExceptionMessages.UnexpectedPropertyState);
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

        throw new JsonException(JsonExceptionMessages.InvalidResultJson);
    }

    public override void Write(Utf8JsonWriter writer, Result<TError> value, JsonSerializerOptions options)
    {
        var ignoreNull = options.DefaultIgnoreCondition is JsonIgnoreCondition.WhenWritingNull;

        writer.WriteStartObject();

        writer.WritePropertyName(_errorPropMatcher.ExpectedName);
        _errorConverter.Write(writer, value.error, options);

        if (value.hasError == null)
        {
            if (!ignoreNull)
            {
                writer.WriteNull(_hasErrorPropMatcher.ExpectedName);
            }
        }
        else
        {
            writer.WriteBoolean(_hasErrorPropMatcher.ExpectedName, value.hasError.Value);
        }

        writer.WriteEndObject();
    }

    #endregion

}
