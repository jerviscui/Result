namespace ResultCore.Tests;

public class ImpossibleException : Exception
{

    #region Constants & Statics

    public static readonly ImpossibleException Instance = new(string.Empty);

    #endregion

    public ImpossibleException(string message) : base(message)
    {
    }

    public ImpossibleException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
