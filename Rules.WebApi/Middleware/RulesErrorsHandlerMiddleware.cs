using Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Rules.WebApi.Middleware
{
    public class RulesErrorsHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public RulesErrorsHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionsAsync(context, ex);
            }
        }
        public static Task HandleExceptionsAsync(HttpContext context, Exception ex)
        {
            var code = new HttpStatusCode();
            var errorMessage = "";
            if (ex is LoanNotFoundException)
            {
                code = HttpStatusCode.BadRequest;
                context.Response.StatusCode = (int)code;
                errorMessage = "Loan Id is wrong. Please try again";
                Log.Error("Someone try to update loan with wrong Id");
            }           
            //write to log db the exception
            var result = JsonConvert.SerializeObject(new { error = errorMessage });
            return context.Response.WriteAsync(result);
        }
    }
}
