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
        private readonly IRequest _request;
       

        public EmailController(IEmail email, IRequest request)
        {
            _email = email;
            _request = request;
        }

        [HttpPost("DonorMail")]
        public async Task <IActionResult> SendMail(string email, int itemId)
        { 
            try
            {
                string? donorEmail = await _email.GetDonorEmailFromDatabase(email);
                int donation = await _request.GetDonorDonation(itemId);
                int requestId = await _request.GetRequestId(donation);
                _request.CreateRequest(donation);
                
                MailRequestDTO mailRequest = new MailRequestDTO();

                mailRequest.ToEmail = donorEmail;
                mailRequest.Subject = "Donation Request from Food Share Nerwork";
                mailRequest.Body = $"We are writing to you notify you that there is a request for the food you donated<br/>" +
                                    $"To accept or decline this request, please visit <a href='http://localhost:3000/request/{requestId}' >here</a> <br/><br/>" +
                                    $"Thank you for time and consideration. <br/><br/>" + 
                                    $"Sincerely,<br/><br/>" +
                                    $"Food Share Network";

                await _email.SendEmailAsync(mailRequest, email);

                return Ok();
            }catch (Exception ex)
            {
                throw;
            }
           
        }

        [HttpPost("RecipientMail")]

        public async Task<IActionResult> NotifyRecipient(int requestId)
        {
            //int requestId = await _request.GetRequestId(donation);
            string? email = await _email.GetRecipientEmail(requestId);
            var status = await _request.GetRequestStatus(requestId);


            RecipientMailDTO recipientMailDTO = new RecipientMailDTO(); 

            recipientMailDTO.ToEmail = email;
            recipientMailDTO.Subject = "Response to your request from Food Share Network";
            recipientMailDTO.Body = $"We are writing to you to notify you on the response of your request <br/>" +
                                    $"Your request has been {status} Please follow the necessary steps";

            await _email.NotifyRecipient(recipientMailDTO, requestId);

            return Ok();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmail(int id)
        {
            var email = await _email.GetRecipientEmail(id);

            if (email == null)
            {
                return NotFound($"Email not found for recipient ID {id}");
            }

            return Ok(email);
        }
    }
}
