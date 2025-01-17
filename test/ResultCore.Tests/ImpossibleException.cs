namespace ResultCore.Tests;

public class ImpossibleException : Exception
{
    public static readonly ImpossibleException Instance = new();
}
