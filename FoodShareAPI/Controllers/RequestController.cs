﻿using Application.Contracts;
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
                // Log the exception (ensure you have a logging mechanism in place)
                //_logger.LogError(ex, "Error occurred in GetDonorRequests action.");
                // Return a generic error response
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
