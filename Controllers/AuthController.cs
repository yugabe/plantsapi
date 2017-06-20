using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plants.API.Services;

namespace Plants.API.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly AuthorizationFlipSwitch _flipSwitch;
        public AuthController(AuthorizationFlipSwitch flipSwitch)
        {
            _flipSwitch = flipSwitch;
        }

        [HttpGet("enable")]
        public void Enable() => _flipSwitch.Enabled = true;

        [HttpGet("disable")]
        public void Disable() => _flipSwitch.Enabled = false;
    }
}