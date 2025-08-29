using Chatbot.Common;
using Chatbot.Common.Extension;
using Chatbot.Common.Result;
using Chatbot.Model.User;
using Chatbot.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chatbot.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUserService userService
            , IConfiguration configuration
            , ILogger<LoginController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Jwt:Audience"];
            validationParameters.ValidIssuer = _configuration["Jwt:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstants.AppSettings.JWTSecurityKey));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserSignRequest request, string ReturnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.Login(request);

                if (!result.IsSuccessed)
                    return Ok(new Result<bool> { IsSuccessed = false, Message = result.Message });

                var userPrincipal = ValidateToken(result.ResultObj);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(SystemConstants.AppSettings.ExpiresUtcMinutes),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

                return Redirect(ReturnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi {0}", Request.GetFullUrl());
                return StatusCode(500, "Đã có lỗi xảy ra");
            }
        }
    }
}
