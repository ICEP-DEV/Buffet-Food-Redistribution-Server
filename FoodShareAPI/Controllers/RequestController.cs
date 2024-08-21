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
        [HttpPut("updateCollection/{donationRequestId}")]

        public IActionResult UpdateCollection(int donationRequestId)
        {
            try
            {
                _request.UpdateCollectionStatus(donationRequestId);
                return Ok($"Collection status updated successfully");
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

        [HttpGet("DonorRequests")]
        [Authorize]

        public async Task<IActionResult> GetDonationRequests()
        {
            var requests = await _request.GetDonationRequstListAsync();
            return Ok(requests);
           
        }

        [HttpGet]
        public async Task<IActionResult> GetDonorRequests()
        {
            try
            {
                var donorRequests = await _request.GetDonorRequests();

                // Return OK with the donor requests
                return Ok(donorRequests);
            }
            catch (Exception ex)
            {
               
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("AcceptedRequests")]

        public Task<List<DonationRequest>> GetAcceptedRequests()
        {
            var acceptedRequests = _request.GetAcceptedHistory();

            return acceptedRequests;
        }

        [HttpGet("DeclinedRequests")]

        public Task<List<DonationRequest>> GetDeclinedRequests()
        {
            var acceptedRequests = _request.GetDeclinedHistory();

            return acceptedRequests;
        }

        [HttpGet("PendingRequests")]

        public Task<List<DonationRequest>> GetPendingRequests()
        {
            var acceptedRequests = _request.GetPendingHistory();

            return acceptedRequests;
        }
    }
}
