using MemoryPack;

namespace ResultCore.Serialization.Tests;

[MemoryPackable]
public readonly partial record struct PureValueError(int Code, int Detail);
