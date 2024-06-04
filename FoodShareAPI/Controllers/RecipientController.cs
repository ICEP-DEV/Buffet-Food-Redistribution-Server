﻿using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut("{id}")]
        public async Task <int> UpdateRecipientAsync(int id , Recipient _recipient)
        {
            return await recipient.UpdateRecipientAsync(id, _recipient);
        }

        [HttpDelete("{id}")]

        public async Task <int> DeleteRecipientAsync(int id)
        {
            return await recipient.DeleteRecipientAsync(id);
        }
    }
}
