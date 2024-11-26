using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Authorization.Core.Filters;

public class ExceptionHandleFilter: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        int statusCode = context.Exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Result = new JsonResult(context.Exception.Message)
        {
            StatusCode = statusCode
        };
        
        context.ExceptionHandled = true;
    }
}