﻿using Application.Contracts;
using Application.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmail _email;
        private readonly IRequest _request;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailController(IEmail email, IRequest request, AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _email = email;
            _request = request;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("DonorMail")]
        public async Task<IActionResult> SendMail(string email, int itemId)
        {
            try
            {
                //string? donorEmail = await _email.GetDonorEmailFromDatabase(email);

                string? donorEmail = _appDbContext.FoodItems.FirstOrDefault(e => e.Contact == email)?.Contact;

                int donation = await _request.GetDonorDonation(itemId);
                int? requestId = await _request.GetRequestId(donation);
                _request.CreateRequest(donation);

                MailRequestDTO mailRequest = new MailRequestDTO();

                mailRequest.ToEmail = donorEmail;
                mailRequest.Subject = "Donation Request from Food Share Nerwork";
                mailRequest.Body = $"We are writing to you notify you that there is a request for the food you donated<br/>" +
                                    $"To accept or decline this request, please visit <a href='http://localhost:3000/request/{donation}' >here</a> <br/><br/>" +
                                    $"Thank you for time and consideration. <br/><br/>" +
                                    $"Sincerely,<br/><br/>" +
                                    $"Food Share Network";

                await _email.SendEmailAsync(mailRequest, email);

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost("RecipientMail")]
        public async Task<IActionResult> NotifyRecipient(int requestId)
        {
            try
            {
                
                string? email = await _email.GetRecipientEmail(requestId);
                var status = await _request.GetRequestStatus(requestId);

                // Handle different statuses
                switch (status)
                {
                    case "Accepted":
                        await HandleAcceptedStatus(email, requestId);
                        break;

                    case "Declined":
                        await HandleDeclinedStatus(email, requestId);
                        break;

                    default:
                        return BadRequest("Request status is neither 'Accepted' nor 'Declined'.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

       
        [HttpGet("{requestId}")]
        public async Task<IActionResult> GetEmail(int requestId)
        {
            try
            {
                var recipient = await _email.GetRecipientInfo(requestId);
                if (recipient == null)
                {
                    return NotFound($"Email not found for recipient ID {requestId}");
                }

                return Ok(recipient);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        private async Task HandleAcceptedStatus(string? email, int requestId)
        {
            // Define the total time allowed for collection in milliseconds
            int totalTimeMillis = 300000; // 5 minutes

            // Calculate the collection deadline
            DateTime collectionDeadline = DateTime.UtcNow.AddMilliseconds(totalTimeMillis);

            // Create the recipient mail DTO
            RecipientMailDTO recipientMailDTO = new RecipientMailDTO
            {
                ToEmail = email,
                Subject = "Response to your request from Food Share Network",
                Body = $"We are writing to notify you of the response to your request.<br/><br/>" +
                       $"Your request has been accepted. You have until <strong>{collectionDeadline:MMMM dd, yyyy HH:mm:ss UTC}</strong> to collect the food.<br/><br/>" +
                       $"Time remaining: <strong>{totalTimeMillis / 60000} minutes</strong>."
            };

            // Notify recipient
            await _email.NotifyRecipient(recipientMailDTO, requestId);

            // Simulate waiting (non-blocking)
            await Task.Delay(totalTimeMillis); // Delay in milliseconds
        }

        private async Task HandleDeclinedStatus(string? email, int requestId)
        {
            // Create the recipient mail DTO for declined status
            RecipientMailDTO recipientMailDTO = new RecipientMailDTO
            {
                ToEmail = email,
                Subject = "Response to your request from Food Share Network",
                Body = $"We are writing to notify you of the response to your request.<br/><br/>" +
                       $"Your request has been declined."
            };

            // Notify recipient
            await _email.NotifyRecipient(recipientMailDTO, requestId);
        }

    }
}
