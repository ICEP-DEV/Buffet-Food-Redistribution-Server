using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipientController : ControllerBase
    {
        private readonly IRecipient recipient;
        public RecipientController(IRecipient recipient)
        {

            this.recipient = recipient;
        }

        [HttpPost("Login")]

        public async Task<ActionResult<LoginResponse>> LoginUser(RecipientsLoginDTO recipientLoginDTO)
        {
            var result = await recipient.LoginRecipientAsync(recipientLoginDTO);
            return Ok(result);
        }


        [HttpPost("register")]

        public async Task<ActionResult<LoginResponse>> RegisterUser([FromBody] RecipientDTO recipientDTO)
        {
            var result = await recipient.RegisterRecipientAsync(recipientDTO);
            return Ok(result);
        }

        [HttpGet("AllRecipients")]

        public async Task<List<Recipient>> GetRecipientListAsync()
        {
            return await recipient.GetRecipientListAsync();  
        }

        [HttpGet("{email}")]

       public Task<Recipient> GetRecipientByEmail(string email)
        {
            return recipient.GetRecipientByEmail(email);
        }

        [HttpPut]
        [Authorize]
        public async Task <int> UpdateRecipientAsync( Recipient _recipient)
        {
            var result = await recipient.UpdateRecipientAsync(_recipient);
            return(result);
        }

        [HttpDelete("{id}")]

        public async Task <int> DeleteRecipientAsync(int id)
        {
            return await recipient.DeleteRecipientAsync(id);
        }

        [HttpGet("Profile")]
        

        public async Task <ActionResult<Recipient>> GetRecipientProfile()
        {
            var recipientProfile = await recipient.GetRecipientProfile();

            if(recipientProfile == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(recipientProfile);
            }


        }

    }
}
