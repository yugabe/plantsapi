using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plants.API.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApiInterceptorBuilderExtensions
    {
        public static IApplicationBuilder UseApiInterceptor(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiInterceptorMiddleware>();
        }
    }
}
