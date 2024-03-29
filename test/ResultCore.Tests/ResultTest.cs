using Shouldly;

namespace ResultCore.Tests;

public class ResultTest
{
    private Result<Error> Result_Test()
    {
        var result = new Result<Error>(new Error(1));
        result = Error.Result(1);
        result = Error.Result();

        //fail
        if (result.IsError())
        {
            return result;
        }

        //success
        return Result.Ok;
    }

    private Result<MyClass, Error> Result_Data_Test()
    {
        //var result = new Result<MyClass, Error>(new Error(1));
        //result = Error.Result();
        var result = Error.Result();

        var i = 1;
        if (i == 1)
        {
            //success
            return new MyClass();
        }

        //fail
        return result;
    }

    [Fact]
    public void Test()
    {
        var result = Result_Test();
        if (result.IsError())
        {
            result.UnwrapError().Code.ShouldBe((int)BasicErrorCode.Failure);
        }

        var data = Result_Data_Test();
        data.IsError(out var err, out var my).ShouldBe(false);
        err.ShouldBeNull();
        my.ShouldNotBeNull();
    }
}
