namespace ResultCore.Tests;

public sealed record RefError(BaseErrorCode Code, string? Reason = null, Exception? Exception = null)
{
    public RefError() : this(BaseErrorCode.NotFound)
    {
    }

    public static implicit operator RefError(BaseErrorCode errorCode) => new(errorCode);
}
