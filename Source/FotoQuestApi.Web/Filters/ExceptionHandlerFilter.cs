using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FotoQuestApi.Web.Filters;

public class ExceptionHandlerFilter : ExceptionFilterAttribute
{
    public override Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        return Task.CompletedTask;
    }

    public override void OnException(ExceptionContext context)
    {
        context.Result = PrepareErrorResult(context);
        context.ExceptionHandled = true;
    }

    private IActionResult PrepareErrorResult(ExceptionContext context)
    {
        if (context.Exception is NotFoundException notFoundException)
        {
            return new ObjectResult(notFoundException.Message) { StatusCode = StatusCodes.Status404NotFound };
        }
        if (context.Exception is BadRequestException badRequestException)
        {
            return new ObjectResult(badRequestException.Message) { StatusCode = StatusCodes.Status400BadRequest };
        }

        return context.Exception is null
               ? new StatusCodeResult(StatusCodes.Status500InternalServerError) as IActionResult
               : new ObjectResult(new FilePersistanceFailedResponse
               {
                   Message = context.Exception.Message,
                   Details = context.Exception.ToString()
               })
               {
                   StatusCode = StatusCodes.Status500InternalServerError
               };
    }
}