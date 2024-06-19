using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodDonationController : ControllerBase
    {
        private readonly IFoodDonation donation;
        private readonly AppDbContext appDbContext;
        //private readonly IHttpContextAccessor httpContextAccessor;
        //
        //this.httpContextAccessor = httpContextAccessor;
        public FoodDonationController(IFoodDonation donation, AppDbContext appDbContext)
        {
            this.donation = donation;
            this.appDbContext = appDbContext;

        }


        [Authorize]
        [HttpPost("populate")]
        public IActionResult PopulateFoodDonations([FromBody] FoodItem foodItems)
        {
            donation.PopulateFoodDonations( foodItems);

            return Ok("Food donations populated successfully.");
        }


        [HttpGet]
        public IActionResult GetFoodDonations()
        {
            try
            {
                var donations = donation.GetDonationsAsync();
                return Ok(donations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching donations");
            }
        }

        [HttpGet("Donor")]

        public async Task<ActionResult<List<FoodDonationDTO>>> GetFooGetDonationsForDonor()
        {
            try
            {
                var foodDonations = await donation.GetDonationsForDonorAsync();
                return foodDonations;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex}");

                // Return a meaningful error response
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving food donations.");
            }
        }

    }
}
