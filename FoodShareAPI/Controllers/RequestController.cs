using Application.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {

        private readonly IRequest _request;

        public RequestController(IRequest request)
        {
            _request = request;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateRequest(int foodDonationId)
        {
            try
            {
                _request.CreateRequest( foodDonationId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("updatestatus/{donationRequestId}")]

        public IActionResult UpdateDonationRequestStatus(int donationRequestId, string newStatus)
        {
            try
            {
                _request.UpdateDonationRequestStatus(donationRequestId, newStatus);
                return Ok($"Donation request status updated successfully to {newStatus}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]

        public async Task<IActionResult> GetDonationRequests()
        {
            var requests = await _request.GetDonationRequstListAsync();
            return Ok(requests);
           
        }
    }
}
