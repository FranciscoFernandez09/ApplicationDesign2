using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.AssemblyManagement;
using SmartHome.DataAccess;

namespace SmartHome.WebApi.Filters;

public sealed class ExceptionFilter : IExceptionFilter
{
    private static readonly Dictionary<Type, Func<Exception, ObjectResult>> Errors = new()
    {
        {
            typeof(ArgumentNullException), exception =>
            {
                var concreteException = (ArgumentNullException)exception;
                return new ObjectResult(new
                {
                    InnerCode = "ArgumentNull",
                    Message = "Argument cannot be null or empty",
                    Argument = concreteException.ParamName,
                    Details = exception.Message
                }) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        },
        {
            typeof(InvalidOperationException), exception =>
                new ObjectResult(new
                {
                    InnerCode = "InvalidOperation", Message = "Cannot continue", Details = exception.Message
                }) { StatusCode = (int)HttpStatusCode.BadRequest }
        },
        {
            typeof(UnauthorizedAccessException), exception =>
                new ObjectResult(new
                {
                    InnerCode = "UnauthorizedAccess", Message = "Unauthorized access", Details = exception.Message
                }) { StatusCode = (int)HttpStatusCode.Unauthorized }
        },
        {
            typeof(ArgumentException), exception =>
                new ObjectResult(new
                {
                    InnerCode = "InvalidArgument", Message = "Invalid argument", Details = exception.Message
                }) { StatusCode = (int)HttpStatusCode.BadRequest }
        },
        {
            typeof(DataAccessException), exception =>
                new ObjectResult(new
                {
                    InnerCode = "DataAccessError", Message = "Error accessing data", Details = exception.Message
                }) { StatusCode = (int)HttpStatusCode.InternalServerError }
        },
        {
            typeof(AssemblyException), exception =>
                new ObjectResult(new
                {
                    InnerCode = "AssemblyError", Message = "Error loading assembly", Details = exception.Message
                }) { StatusCode = (int)HttpStatusCode.InternalServerError }
        }
    };

    public void OnException(ExceptionContext context)
    {
        Func<Exception, ObjectResult>? response = Errors.GetValueOrDefault(context.Exception.GetType());

        if (response == null)
        {
            context.Result =
                new ObjectResult(new
                {
                    InnerCode = "InternalServerError",
                    Message = "There was an error when processing the request"
                })
                { StatusCode = (int)HttpStatusCode.InternalServerError };
            return;
        }

        context.Result = response(context.Exception);
    }
}
