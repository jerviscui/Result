using Orleans;

namespace ResultCore.Serialization.Microsoft.Orleans.Serialization.Tests;

[GenerateSerializer]
[Alias("ResultCore.Serialization.Microsoft.Orleans.Serialization.MyData")]
[Immutable]
public sealed class MyData
{
    public MyData(string name)
    {
        Name = name;
    }

    #region Properties

    [Id(0)]
    public string Name { get; set; }

    #endregion

}
