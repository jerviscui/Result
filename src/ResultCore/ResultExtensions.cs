using System;

namespace ResultCore;

public static class ResultExtensions
{
    public static Result<TData, TError> From<TData, TError, TFError>(this Result<TData, TError> result,
        Result<TFError> other) where TFError : Error where TError : Error, new()
    {
        if (other.Error is null)
        {
            throw new NullReferenceException("other is Null.");
        }

        if (result.IsError(out var error))
        {
            error.From(other.Error);
        }

        return result;
    }

    public static Result<TError> From<TError, TFError>(this Result<TError> result,
        Result<TFError> other) where TFError : Error where TError : Error
    {
        if (other.Error is null)
        {
            throw new NullReferenceException("other is Null.");
        }

        if (result.IsError(out var error))
        {
            error.From(other.Error);
        }

        return result;
    }
}
