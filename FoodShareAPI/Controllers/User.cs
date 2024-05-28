using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {
        private readonly IUser user;
        public User(IUser user)
        {

            this.user = user;
        }

        [HttpPost("Login")]

        public async Task<ActionResult<LoginResponse>> LogUserIn(UserLoginDTO loginDTO)
        {
            var result = await user.LoginUserAsync(loginDTO);
            return Ok(result);
        }


        [HttpPost("register")]

        public async Task<ActionResult<LoginResponse>> RegisterUser(UserRegistrationDTO registerDTO) 
        {
            var result = await user.RegisterUserAsync(registerDTO);
            return Ok(result);
        }
    }
}
