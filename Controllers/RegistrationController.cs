using FoodShareNetwork.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.CodeDom.Compiler;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserDbContext _userDbContext;
        public UserController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }


        /*  public async Task<IActionResult> Register([FromBody] User user)
          {
              if (_userDbContext.Users.Any(u => u.Email == user.Email))
              {
                  return BadRequest("User already exists");
              }
              if (!ModelState.IsValid)
              {
                  return BadRequest(ModelState); // Return bad request with validation errors
              }
              if (user == null) { }
              await _userDbContext.Users.AddAsync(user);
              _userDbContext.SaveChanges();

              return Ok("Registered successfully"); // Return bad request with registration errors
          }*/

        [HttpPost("Register")]

        public async Task<IActionResult> Register(User request)
        {
            if (_userDbContext.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("Something wrong");
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            _userDbContext.Users.Add(user);
            await _userDbContext.SaveChangesAsync();

            return Ok("Registered successfully");
        }


        [HttpPost]
        [Route("Login")]

        public IActionResult Login(UserLoginRequest request)
        {
            var user = _userDbContext.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);
            if (user != null)
            {
                return Ok("Login successful");
            }

            return NoContent();
        }



        /* [HttpPost]
         [Route("{email}/{password}")]
         public async Task<IActionResult> Login(string email, string password)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest();
             }

             ResponseDTO responseDTO = new ResponseDTO();
             responseDTO.Status = await _userDbContext.Users.AnyAsync(u => u.Email == email);
             if (!responseDTO.Status)
             {
                 responseDTO.Message = "Incorrect username or password";
                 ;
             }
             else
             {
                 var result = await _userDbContext.Users.SingleAsync(u => u.Email == email);
                 if (result.Password == password)
                 {
                     responseDTO.Message = "Password correct";
                     responseDTO.Result = result;
                 }
                 else
                 {
                     responseDTO.Status = false;
                     responseDTO.Message = "Incorrect username or password";
                 }
             }

             //  responseDTO.Status = CheckEmail(user.Email);
             return Ok(responseDTO);
         }*/




        [HttpGet("List")]
         public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            if (_userDbContext == null)
            {
                return NotFound();
            }

            return await _userDbContext.Users.ToListAsync();
        }
    }
}


   






    
