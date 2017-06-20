using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Plants.API.Models;

namespace Plants.API.Middlewares
{
    public class ApiInterceptorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;

        public ApiInterceptorMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var memoryResponse = new MemoryStream())
            {
                var originalResponse = httpContext.Response.Body;
                try
                {
                    httpContext.Response.Body = memoryResponse;

                    try
                    {
                        await _next(httpContext);
                    }
                    catch (Exception ex)
                    {
                        httpContext.Response.StatusCode = ex is UnauthorizedAccessException ? 401 : 500;
                        await httpContext.Response.WriteAsync($"Exception: {(_env.IsDevelopment() ? ex.ToString() : ex.GetType().ToString())}");
                    }
                    var statusCode = httpContext.Response.StatusCode;
                    if (statusCode >= 400 && statusCode <= 599)
                    {
                        memoryResponse.Seek(0, SeekOrigin.Begin);
                        var jsonResponse = JsonConvert.SerializeObject(new ErrorModel { Code = statusCode, Description = new StreamReader(memoryResponse).ReadToEnd() }) + new string(' ', 100);
                        using (var writer = new StreamWriter(originalResponse, Encoding.UTF8, 1024, true))
                            writer.Write(jsonResponse);
                    }
                    else
                    {
                        memoryResponse.Seek(0, SeekOrigin.Begin);
                        memoryResponse.CopyTo(originalResponse);
                    }
                }
                finally
                {
                    httpContext.Response.Body = originalResponse;
                }
            }
        }
    }
}