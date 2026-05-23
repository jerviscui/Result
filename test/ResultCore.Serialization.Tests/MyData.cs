using MemoryPack;
using MessagePack;
using Orleans;

namespace ResultCore.Serialization.Tests;

[MemoryPackable]
[GenerateSerializer]
[Immutable]
[Alias("ResultCore.Serialization.Tests.MyData")]
[MessagePackObject]
public sealed partial class MyData
{
    [MemoryPackConstructor]
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
