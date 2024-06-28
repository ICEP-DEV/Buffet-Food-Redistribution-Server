using Application.Contracts;
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
        public async Task <IActionResult> SendMail(int donorId, int itemId)
        {
            try
            {
                string? donorEmail = await _email.GetDonorEmail(donorId, itemId);
                
                MailRequestDTO mailRequest = new MailRequestDTO();

                mailRequest.ToEmail = donorEmail;
                mailRequest.Subject = "Welcome to food share nerwork";
                mailRequest.Body = "A request for food has been made to confirm the request please click";

                await _email.SendEmailAsync(mailRequest,donorId);

                return Ok();
            }catch (Exception ex)
            {
                throw;
            }
           
        }
    }
}
