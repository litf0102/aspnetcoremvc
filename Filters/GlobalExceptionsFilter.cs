using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using aspnetcoremvc.Log4net;

namespace aspnetcoremvc.Filters
{
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _env;
        private readonly ILoggerHelper _loggerHelper;

        public GlobalExceptionsFilter(IHostEnvironment env, ILoggerHelper loggerHelper)
        {
            _env = env;
            _loggerHelper = loggerHelper;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            json.Message = context.Exception.Message;
            if (_env.IsDevelopment())
            {
                json.DevelopmentMessage = context.Exception.StackTrace;
            }
            context.Result = new InternalServerErrorObjectResult(json);

            // ログ出力
            _loggerHelper.Error(json.Message, "例外が発生しました", context.Exception);

        }

        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
        // エラー情報
        public class JsonErrorResponse
        {
            /// <summary>
            /// メッセージ内容
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// スタック内容
            /// </summary>
            public string DevelopmentMessage { get; set; }
        }

    }
}
