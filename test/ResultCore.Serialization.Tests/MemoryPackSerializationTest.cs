using MemoryPack;
using Shouldly;

namespace ResultCore.Serialization.Tests;

public class MemoryPackSerializationTest
{

    #region Methods

    [Fact]
    public void PureValueTypeErrorResultRoundTrips()
    {
        var result = new Result<PureValueError>(new PureValueError(7, 9));

        var bytes = MemoryPackSerializer.Serialize(result);
        var tmp = MemoryPackSerializer.Deserialize<Result<PureValueError>>(bytes);

        tmp.IsError().ShouldBeTrue();
        tmp.IsError(out var error).ShouldBeTrue();
        _ = error.ShouldNotBeNull();
        error.Value.Code.ShouldBe(7);
        error.Value.Detail.ShouldBe(9);
    }

    [Fact]
    public void ReferenceErrorStructResultRoundTrips()
    {
        Result<MyData, FileError> result = FileError.Result(FileErrorCode.A);
        var bytes = MemoryPackSerializer.Serialize(result);
        var tmp = MemoryPackSerializer.Deserialize<Result<MyData, FileError>>(bytes);
        tmp.IsError().ShouldBeTrue();

        result = new MyData("aaa");
        bytes = MemoryPackSerializer.Serialize(result);
        tmp = MemoryPackSerializer.Deserialize<Result<MyData, FileError>>(bytes);
        tmp.IsError(out var _, out var data).ShouldBeFalse();
        data.Name.ShouldBe("aaa");
    }

    #endregion

}
