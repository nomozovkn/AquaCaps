using AquaCaps.Core.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AquaCaps.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext actionExecutedContext)
    {
        var code = 500;

        if (actionExecutedContext.Exception is EntityNotFoundException)
        {
            code = 404; // Not found
        }

        if (actionExecutedContext.Exception is UnauthorizedException)
        {
            code = 401; // Not found
        }

        if (actionExecutedContext.Exception is AuthException)
        {
            code = 401; // Not found
        }

        if (actionExecutedContext.Exception is NotAllowedException)
        {
            code = 403; // Not found
        }






        actionExecutedContext.HttpContext.Response.StatusCode = code;

        actionExecutedContext.Result = new JsonResult(new
        {
            error = actionExecutedContext.Exception.Message
        });
    }
}
