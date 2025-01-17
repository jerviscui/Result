using Shouldly;

namespace ResultCore.Tests;

public class ResultTest
{

    #region Constants & Statics

    private static Result<MyClass, BasicError> Result_Data_Test()
    {
        //var result = new Result<MyClass, Error>(new Error(1));
        var result = BasicError.Result();

        var i = 1;
        if (i == 1)
        {
            //success
            return new MyClass();
        }

        //fail
        return result;
    }

    private static Result<BasicError> Result_Test()
    {
        //var result = new Result<Error>(new Error(1));
        var result = BasicError.Result(1);

        //fail
        if (result.IsError())
        {
            return result;
        }

        //success
        return Result.Ok;
    }

    #endregion

    #region Methods

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

    #endregion

}
