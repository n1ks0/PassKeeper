using System.Net;
using Microsoft.AspNetCore.Mvc;
using PassKeeper.Core.Operation;

namespace PassKeeper.AppWebApi;

public readonly struct HttpResult(IOperationResult operationResult) : IActionResult
{
    public async Task ExecuteResultAsync(ActionContext context)
    {
        switch (operationResult.Result)
        {
            case Result.Fail:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.HttpContext.Response.ContentType = "application/json";;
                await context.HttpContext.Response.WriteAsJsonAsync(operationResult.Message);
                break;
            case Result.Success:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public static class HttpResultExtensions
{
    public static HttpResult Result(this IOperationResult operationResult)
    {
        return new HttpResult(operationResult);
    }
}