﻿using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmail _email;

        public EmailController(IEmail email)
        {
            _email = email;
        }

        [HttpPost("SendaMail")]
        public async Task <IActionResult> SendMail()
        {
            try
            {
                MailRequestDTO mailRequest = new MailRequestDTO();

                mailRequest.ToEmail = "kamomohapi17@gmail.com";
                mailRequest.Subject = "Welcome to food share nerwork";
                mailRequest.Body = "A request for food has been made";

                await _email.SendEmailAsync(mailRequest);

                return Ok();
            }catch (Exception ex)
            {
                throw;
            }
           
        }
    }
}
