using MessagePack;
using Orleans;

namespace ResultCore.Serialization.Tests;

[GenerateSerializer]
[Immutable]
[Alias("ResultCore.Serialization.Tests.MyData")]
[MessagePackObject]
public sealed class MyData
{
    public MyData(string name)
    {
        Name = name;
    }

    #region Properties

    [Id(0)]
    [Key(0)]
    public string Name { get; set; }

    #endregion

}
