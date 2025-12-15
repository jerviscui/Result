using Microsoft.Extensions.DependencyInjection;
using Orleans.Serialization;
using Shouldly;
using System.Buffers;

namespace ResultCore.Serialization.Tests;

public class OrleansSerializationTest
{

    #region Methods

    [Fact]
    public void Test()
    {
        var serviceProvider = new ServiceCollection()
            .AddSerializer()
            .BuildServiceProvider();
        var serializer = serviceProvider.GetRequiredService<Serializer<Result<MyData, FileError>>>();
        var buffer = ArrayPool<byte>.Shared.Rent(1024);

        Result<MyData, FileError> result = FileError.Result(FileErrorCode.A);
        var len = serializer.Serialize(result, buffer);
        var tmp = serializer.Deserialize(buffer.AsMemory(0, len));
        tmp.IsError().ShouldBeTrue();

        result = new MyData("aaa");
        len = serializer.Serialize(result, buffer);
        tmp = serializer.Deserialize(buffer.AsMemory(0, len));
        tmp.Data!.Name.ShouldBe("aaa");
    }

    #endregion

}
