using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Infrastructure.Repo
{
    public class DonationsRepo : IFoodDonation
    {
        
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DonationsRepo(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {

            this._appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }




        public void PopulateFoodDonations(FoodItem foodItems)
        {
            var donorIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("DonorId");

            // Check if the claim exists and if its value can be parsed as an integer
            if (donorIdClaim != null && int.TryParse(donorIdClaim.Value, out int donorId))
            {
                // The value of the claim has been successfully parsed as an integer
                foodItems.DateCooked = DateTime.Now;
                _appDbContext.FoodItems.Add(foodItems);
                _appDbContext.SaveChanges();

                var foodDonation = new FoodDonation
                {
                    DateCooked = DateTime.Now,
                    DonorId = donorId,
                    ItemId = foodItems.Id,
                    Quantity = foodItems.Quantity,
                };

                _appDbContext.FoodDonations.Add(foodDonation);
                _appDbContext.SaveChanges();
            }
            else
            {
                // Handle the case where the "DonorId" claim is not found or its value cannot be parsed as an integer
                // You may want to log an error, return a specific HTTP response, or take other appropriate action

            }
        }

        public async Task<List<FoodDonationDTO>> GetDonationsForDonorAsync()
        {
            try
            {
                var donorIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("DonorId");

                if (donorIdClaim == null)
                {
                    throw new InvalidOperationException("User's donor ID not found in claims.");
                }

                // Retrieve the donorId as a string
                var donorIdString = donorIdClaim.Value;

                // Convert donorIdString to int
                if (!int.TryParse(donorIdString, out int donorId))
                {
                    throw new InvalidOperationException("Unable to parse donor ID as integer.");
                }

                var donations = await (from donation in _appDbContext.FoodDonations
                                       where donation.DonorId == donorId
                                       join foodItem in _appDbContext.FoodItems on donation.ItemId equals foodItem.Id
                                       select new FoodDonationDTO
                                       {
                                           DonorId = donorId,
                                           ItemId = foodItem.Id,
                                           Quantity = donation.Quantity,
                                           DateCooked = donation.DateCooked
                                       }).ToListAsync();

                return donations;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"An error occurred while retrieving donations for the donor: {ex}");

                // Rethrow the exception wrapped in a new exception with a more descriptive message
                throw new Exception("An error occurred while retrieving donations for the donor. See inner exception for details.", ex);
            }
        }


         public IEnumerable<FoodDonationDTO> GetDonationsAsync()
        
        {
            var donations = (from donation in _appDbContext.FoodDonations
                             join donor in _appDbContext.Donors on donation.DonorId equals donor.DonorId
                             join foodItem in _appDbContext.FoodItems on donation.ItemId equals foodItem.Id
                             select new FoodDonationDTO
                             {
                                 DonorId = donor.DonorId,
                                 ItemId = foodItem.Id,
                                 Quantity = donation.Quantity,
                                 DateCooked = donation.DateCooked
                             }).ToList();

            return donations;
        }


       public async  Task <int> GetTotalDonationsCountAsync()
        {
            return await _appDbContext.FoodDonations.CountAsync();
        }
    }
}
