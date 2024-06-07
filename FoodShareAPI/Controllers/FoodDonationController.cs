using Application.Contracts;
using Domain.Entities;
using Infrastructure.Data;
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
        //, IHttpContextAccessor httpContextAccessor
        //this.httpContextAccessor = httpContextAccessor;
        public FoodDonationController(IFoodDonation donation, AppDbContext appDbContext)
        {
            this.donation = donation;
            this.appDbContext = appDbContext;
            
        }

        [HttpPost("populate")]
        public IActionResult PopulateFoodDonations(int donors, IEnumerable<FoodItem> foodItems)
        {
            /*if (donors == null || !donors.Any() || foodItems == null || !foodItems.Any())
            {
                return BadRequest("Donors or food items are missing.");
            }*/

            donation.PopulateFoodDonations(donors, foodItems);

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

    


        // [HttpPost]
        /* public async Task<IActionResult> AddFoodDonation(int donorId, int foodItemId, int quantity, DateTime dateCooked )
        {
         await donation.AddFoodDonationAsync(donorId, foodItemId, quantity, dateCooked);
         return Ok("Food donation added successfully");
          }*/


        //[HttpGet]
        /* public async Task<IActionResult> GetDonations()
        {
             try
            {
                var loggedInDonorId = await GetCurrentDonorId(); // Retrieve the donor ID of the logged-in donor asynchronously
                var donations = await donation.GetDonationsForDonorAsync(loggedInDonorId); // Pass the donor ID to the service method and await the result
                return Ok(donations);
            }
           catch (Exception ex)
            {
             // Log the error
             return StatusCode(500, "An error occurred while fetching donations."); // Return 500 status code and error message
            }
        }

       private async Task<int> GetCurrentDonorId()
         {
           // Retrieve the donor ID of the logged-in donor from the HttpContext asynchronously
          var loggedInDonorId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
          return loggedInDonorId;
        }*/
    }
}
