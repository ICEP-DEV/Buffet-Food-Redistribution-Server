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
            donation.PopulateFoodDonations(foodItems);

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
            catch {
                return StatusCode(500, "An error occurred while fetching donations");
            }
        }

        [HttpGet("Donor-Items")]
        [Authorize]
         public async Task<ActionResult>DonorFood()
         {
            try
            {
                // Assuming donation is a service that handles the logic for fetching donor's food items.
                var foodItems = await donation.DonorFood();

                if (foodItems == null || !foodItems.Any())
                {
                    return NotFound("No food items found for the donor.");
                }

                return Ok(foodItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
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

        [HttpGet("total")]
        public async Task<IActionResult> GetTotalDonations()
        {
            // Assuming `donation.GetTotalDonationsCountAsync()` returns an int
            var totalDonations = await donation.GetTotalDonationsCountAsync();

            // Wrap the integer count in an Ok result
            return Ok(totalDonations);
        }

        [HttpGet("monthly-totals")]
        public async Task<IActionResult> GetMonthlyTotals()
        {
            var monthlyTotals = await donation.GetMonthlyTotalDonationsAsync();
            return Ok(monthlyTotals);
        }

        [HttpGet("weekly-totals")]
        public async Task<IActionResult> GetWeeklyTotals()
        {
            var weeklyTotals = await donation.GetWeeklyTotalDonationsAsync();
            return Ok(weeklyTotals);
        }

        [HttpGet("monthly2-totals")]
        public async Task<IActionResult> GetMonthly2Totals()
        {
            var monthlyTotals = await donation.GetMonthlyTotalFromWeeklyTotalsAsync();
            return Ok(monthlyTotals);
        }
    }
}
