using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plants.API.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseApiInterceptor(this IApplicationBuilder app)
            => app.UseMiddleware<ApiInterceptorMiddleware>();

        public static IApplicationBuilder UseAnonymousUserMockAuthentication(this IApplicationBuilder app)
          => app.UseMiddleware<AnonymousUserMockAuthenticationMiddleware>();

    }
}
