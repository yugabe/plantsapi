using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Plants.API.Services
{
    public class JwtBearerEvents : IJwtBearerEvents
    {
        public async Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            
        }

        public async Task Challenge(JwtBearerChallengeContext context)
        {
            
        }

        public async Task MessageReceived(MessageReceivedContext context)
        {
            
        }

        public async Task TokenValidated(TokenValidatedContext context)
        {
            
        }
    }
}
