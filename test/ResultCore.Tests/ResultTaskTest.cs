using Shouldly;

namespace ResultCore.Tests;

public class ResultTaskTest
{

    #region Constants & Statics

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

    private static ResultTask<BaseError> ResultTask_Error_Sync_Test()
    {
        return BaseError.Result(BaseErrorCode.NotFound, null, null);
    }

    private static async ResultTask<BaseError> ResultTask_Error_TestAsync()
    {
        await Task.Delay(1);

        return new BaseError();

        //return new Result<BaseError>();
    }

    private static async ValueTask<BaseError> ResultTask_Error2_TestAsync()
    {
        await Task.Delay(1);

        return new BaseError();

        //return new ValueTask<BaseError>(new BaseError()); //cs4016
    }

    #endregion

    #region Methods

    [Fact]
    public async Task TestAsync()
    {
        //var a = await ResultTask_Error2_TestAsync();

        //var syncResult = await ResultTask_Error_Sync_Test();
        //if (syncResult.IsError())
        //{
        //    syncResult.UnwrapError().Code.ShouldBe(BaseErrorCode.NotFound);
        //}

        var result = await ResultTask_Error_TestAsync();

        if (result.IsError())
        {
            result.UnwrapError().Code.ShouldBe(BaseErrorCode.NotFound);
        }

        //var data = ResultData_Success_Test();
        //data.IsError(out var err, out var my).ShouldBe(false);
        //err.ShouldBeNull();
        //_ = my.ShouldNotBeNull();
    }

    #endregion

}
