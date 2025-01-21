using Shouldly;

namespace ResultCore.Tests;

public class ResultTest
{

    #region Constants & Statics

    private static Result<BaseError> Result_Error_Test()
    {
        //var result = new Result<Error>(new Error(1));
        var result = BaseError.Result(BaseErrorCode.NotFound);

        //fail
        if (result.IsError())
        {
            return result;
        }

        //success
        return Result.Ok;
    }

    private static Result<MyClass, BaseError> ResultData_Success_Test()
    {
        //var result = new Result<MyClass, Error>(new Error(1));
        var result = BaseError.Result();

        var i = 1;
        if (i == 1)
        {
            //success
            return new MyClass();
        }

        //fail
        return result;
    }

    #endregion

    #region Methods

    [Fact]
    public void Test()
    {
        var result = Result_Error_Test();
        if (result.IsError())
        {
            result.UnwrapError().Code.ShouldBe(BaseErrorCode.NotFound);
        }

        var data = ResultData_Success_Test();
        data.IsError(out var err, out var my).ShouldBe(false);
        err.ShouldBeNull();
        _ = my.ShouldNotBeNull();
    }

    #endregion

}
