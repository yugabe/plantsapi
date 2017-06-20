using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Plants.API.Middlewares
{
    public class AnonymousUserMockAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AnonymousUserMockAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("userid", "user") }, "mock"));
            await _next(httpContext);
        }
    }
}
