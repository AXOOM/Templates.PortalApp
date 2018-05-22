using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyVendor.MyApp.Infrastructure
{
    /// <summary>
    /// Reports exceptions with appropriate HTTP status codes.
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly bool _returnStacktrace;

        public ApiExceptionFilterAttribute(IHostingEnvironment env)
        {
            _returnStacktrace = env.IsDevelopment();
        }

        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)GetStatusCode(context.Exception);
            context.Result = GetResult(context.Exception);

            base.OnException(context);
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            switch (exception)
            {
                case InvalidDataException _:
                    return HttpStatusCode.BadRequest;
                case AuthenticationException _:
                    return HttpStatusCode.Unauthorized;
                case UnauthorizedAccessException _:
                    return HttpStatusCode.Forbidden;
                case KeyNotFoundException _:
                    return HttpStatusCode.NotFound;
                case InvalidOperationException _:
                    return HttpStatusCode.Conflict;
                case TimeoutException _:
                    return HttpStatusCode.RequestTimeout;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }

        private ContentResult GetResult(Exception exception) => new ContentResult
        {
            Content = _returnStacktrace ? exception.ToString() : exception.Message
        };
    }
}
