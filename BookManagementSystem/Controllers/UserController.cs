using BookManagementSystem.Common;
using BookManagementSystem.DBContext;
using BookManagementSystem.Entities;
using BookManagementSystem.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        { 
            _userService = userService;
        }
        [HttpPost("CreateUser")]
        public async Task<ActionResult<string>> SaveUser([FromBody] AdminViewModel model)
        { 
            if(ModelState.IsValid)
            {
                var result = await _userService.CreateUser(model);
                return StatusCode((int)HttpStatusCode.OK, result);
            }
            return StatusCode((int)HttpStatusCode.BadRequest);
        }
        [HttpPost("AddRole")]
        public async Task<ActionResult<string>> SaveRole([FromBody] Roles model)
        {
            if(ModelState.IsValid)
            {
                var result = await _userService.CreateRole(model);
                return StatusCode((int)HttpStatusCode.OK, result);
            }
            return StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
