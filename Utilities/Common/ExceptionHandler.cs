using LoggerService;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Common
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionHandler(RequestDelegate next, ILoggerManager logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {         
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            objResponse.ResponseStatus = false;
            objResponse.ResponseMessage = (ex.InnerException != null) ? ex.GetBaseException().Message : ex.Message;
            _logger.LogError(ex);
            string res = JsonConvert.SerializeObject(objResponse);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(res.ToString());
        }
    }
}
