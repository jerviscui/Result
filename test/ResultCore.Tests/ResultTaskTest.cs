using Shouldly;

namespace ResultCore.Tests;

public class ResultTaskTest
{

    #region Constants & Statics

    private static async ResultTask<BaseError> ResultTask_Error_Async()
    {
        await Task.Yield();

        return new BaseError(BaseErrorCode.NotFound); //return BaseError
    }

    private static ResultTask<BaseError> ResultTask_Error_New()
    {
        //return new BaseError(BaseErrorCode.NotFound);
        return BaseError.Result(BaseErrorCode.NotFound); //return ResultTask<BaseError>
    }

    private static async ResultTask<BaseError> ResultTask_Error_Sync()
    {
        await Task.Yield();

        return new BaseError(BaseErrorCode.NotFound); //return BaseError
    }

    #endregion

    #region Methods

    // TODO: can't use, must return BaseError, CS0029
    //private static async ResultTask<BaseError> ResultTask_Ok_Sync()
    //{
    //    await Task.CompletedTask;

    //    // return new ResultTask<BaseError>(Result.Ok);
    //    return Result.Ok; //return BaseError
    //}

    [Fact]
    public async Task ResultTask_ErrorAsync_TestAsync()
    {
        var result = await ResultTask_Error_Async();

        if (result.IsError())
        {
            result.UnwrapError().Code.ShouldBe(BaseErrorCode.NotFound);
        }
    }

    [Fact]
    public async Task ResultTask_ErrorNew_TestAsync()
    {
        var result = await ResultTask_Error_New();

        if (result.IsError())
        {
            result.UnwrapError().Code.ShouldBe(BaseErrorCode.NotFound);
        }
    }

    [Fact]
    public async Task ResultTask_ErrorSync_TestAsync()
    {
        var result = await ResultTask_Error_Sync();

        if (result.IsError())
        {
            result.UnwrapError().Code.ShouldBe(BaseErrorCode.NotFound);
        }
    }

    #endregion

}
