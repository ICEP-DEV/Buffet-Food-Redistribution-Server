using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _admin;

        public AdminController(IAdmin admin)
        {
            _admin = admin;
        }

        [HttpPost]

        public async Task<ActionResult<LoginResponse>> LogUserIn(AdminDTO admin)
        {
            var result = await _admin.LoginAdmin(admin);
            return Ok(result);
        }

        [HttpPost("RegisterAdmin")]

        public async Task<ActionResult<RegistrationResponse>> RegisterUser([FromBody] Admin admin)
        {
            var result = await _admin.Register(admin);
            return Ok(result);
        }

    }
}
