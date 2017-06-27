using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plants.API.Models;
using Plants.API.Models.AccountViewModels;
using Plants.API.Services;

namespace Plants.API.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SecurityKey _jwtSecurityKey;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            SecurityKey jwtSecurityKey)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSecurityKey = jwtSecurityKey;
        }

        [HttpPost("api/login")]
        [ProducesResponseType(200, Type = typeof(ApiLoginResponse))]
        [AllowAnonymous]
        public async Task<IActionResult> ApiLogin([FromBody]ApiLoginCredentials credentials)
        {
            if (string.IsNullOrWhiteSpace(credentials.Username) || string.IsNullOrWhiteSpace(credentials.Password))
                return BadRequest("Invalid credentials");

            var user = (await _userManager.FindByNameAsync(credentials.Username)) ??
                await _userManager.FindByEmailAsync(credentials.Username);
            if (user == null || !(await _signInManager.CheckPasswordSignInAsync(user, credentials.Password, false)).Succeeded)
                return BadRequest("Invalid credentials");

            return Ok(new ApiLoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                    claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, user.Id), new Claim("userid", user.Id) },
                    signingCredentials: new SigningCredentials(_jwtSecurityKey, SecurityAlgorithms.HmacSha256),
                    expires: DateTime.Now.AddDays(1)))
            });
        }

        [HttpPost("api/register")]
        [ProducesResponseType(200, Type = typeof(ApiLoginResponse))]
        [AllowAnonymous]
        public async Task<IActionResult> ApiRegister([FromBody]ApiLoginCredentials credentials)
            => ((await _userManager.CreateAsync(new ApplicationUser { UserName = credentials.Username, Email = "apianonymous@anonymous.com" }, credentials.Password)).Succeeded)
                ? await ApiLogin(credentials)
                : BadRequest("Invalid user data.");
    }
}
