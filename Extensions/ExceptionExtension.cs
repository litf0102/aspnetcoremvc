using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcoremvc.Utils;
using aspnetcoremvc.Log4net;
using aspnetcoremvc.Models;

namespace aspnetcoremvc.Extensions
{
    public class ExceptionExtension
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerHelper _logger;
        public ExceptionExtension(RequestDelegate next, ILoggerHelper logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                await HandleExceptionAsync(httpContext, ex.Message);
            }
            finally
            {
                var statusCode = httpContext.Response.StatusCode;
                var msg = "";
                        
                switch (statusCode)
                {
                    case 401:
                        msg = "拒绝访问Web APIのアクセスが拒否されました。";
                        break;
                    case 403:
                        msg = "Web APIのアクセス権限がありません。";
                        break;
                    case 404:
                        msg = "Web APIが見つかりません。";
                        break;
                    case 405:
                        msg = "405 Method Not Allowed";
                        break;
                    case 502:
                        msg = "サーバエラー";
                        break;
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(httpContext, msg);
                }
            }
        }
        ///
        private async Task HandleExceptionAsync(HttpContext httpContext, string msg)
        {
            ErrorModel error = new ErrorModel
            {
                code = httpContext.Response.StatusCode,
                msg = msg
            };
            var result = JsonHelper.toJson(error);
            httpContext.Response.ContentType = "application/json;charset=utf-8";
            await httpContext.Response.WriteAsync(result).ConfigureAwait(false);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomExceptionExtensions
    {
        public static IApplicationBuilder UseCustomExceptionExtensions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionExtension>();
        }
    }
}
