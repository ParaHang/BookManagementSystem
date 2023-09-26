using BookManagementSystem.Common;
using BookManagementSystem.DBContext;
using BookManagementSystem.Entities;
using BookManagementSystem.Interfaces;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly UserManager<Users> _userManager;
        private readonly ILoginService _loginService;
        private readonly ApplicationDbContext _context;
        public IConfiguration _config;
        public LoginController(IConfiguration config, UserManager<Users> userManager, ApplicationDbContext context, IAuthenticateService authenticateService, ILoginService loginService)
        {
            _authenticateService = authenticateService;
            _context = context;
            _userManager = userManager;
            _config = config;
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            ResultModel<UserModel> response = new ResultModel<UserModel>();

            response = await _loginService.Authenticate(userLogin, ipAddress());
            if (response.data != null)
            {
                UserModel user = response.data[0];
                var token = _authenticateService.GenerateJwtToken(user);
                setTokenCookie(user.RefreshToken);

                user.Token = token;

                return StatusCode((int)HttpStatusCode.OK, response);
            }
            response.message = "User not found.";
            return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        //not necessary unless token is expired. 
        [AllowAnonymous]
        [HttpGet("GetRefreshToken")]
        public async Task<ActionResult<ResultModel<AuthenticateResponse>>> RefreshToken()
        {
            ResultModel<AuthenticateResponse> response = new ResultModel<AuthenticateResponse>();

            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
            {
                response.message = "No Cookies found";
                response.status = "99";
                response.success = false;
                return StatusCode((int)HttpStatusCode.Forbidden, response);
            }
            response = await _loginService.RefreshToken(refreshToken, ipAddress());

            if (response.data == null)
            {
                response.message = "Invalid Token";
                return StatusCode((int)HttpStatusCode.BadRequest, response);
            }

            setTokenCookie(response.data[0].RefreshToken);
            response.message = "Operation Successful";
            response.status = "00";
            response.success = true;
            return StatusCode((int)HttpStatusCode.OK, response);
        }
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
