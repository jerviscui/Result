namespace ResultCore.Tests;

internal class ImpossibleExcption : Exception
{
    public static readonly ImpossibleExcption Instance = new();
}
