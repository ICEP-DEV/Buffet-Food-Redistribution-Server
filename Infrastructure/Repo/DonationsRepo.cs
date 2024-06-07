using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
    public class DonationsRepo : IFoodDonation
    {
        
        private readonly AppDbContext _appDbContext;
      

        public DonationsRepo( AppDbContext appDbContext)
        {
            
            this._appDbContext = appDbContext;
            
        }

        /* public async Task AddFoodDonationAsync(int donorId, int foodItemId, int quantity, DateTime dateCooked)
          {
              var foodDonation = new FoodDonation
              {
                  DonorId = donorId,
                  ItemId = foodItemId,
                  Quantity = quantity,
                  DateCooked = dateCooked
              };

              await _appDbContext.AddAsync(foodDonation);

          }*/




        public void PopulateFoodDonations(int donorId, IEnumerable<FoodItem> foodItems)
        {
            var donor = _appDbContext.Donors.FirstOrDefault(d => d.DonorId == donorId);

            if (donor == null)
            {
                // Handle the case where donor is not found
                // For example, throw an exception or return an error message
                throw new ArgumentException("Donor not found.", nameof(donorId));
            }

            var foodItemsList = foodItems.ToList(); // Materialize the foodItems collection to avoid multiple database calls

            foreach (var foodItem in foodItemsList)
            {
                // Create a new FoodDonation entity instance
                var foodDonation = new FoodDonation
                {
                    DonorId = donorId,
                    ItemId = foodItem.Id,
                    Quantity = foodItem.Quantity,
                    DateCooked = foodItem.DateCooked
                };

                // Add the new FoodDonation entity instance to the FoodDonations table
                _appDbContext.FoodDonations.Add(foodDonation);
            }

            // Save changes to the database
            _appDbContext.SaveChanges();
        }

        


        public async Task<IEnumerable<FoodDonationDTO>> GetDonationsForDonorAsync(int donorId)
        {
            try
            {
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
                // Log the exception
                // You can log the exception using a logging framework like Serilog, NLog, etc.
                // Also, you can throw or return the exception depending on your application's error handling strategy.
                throw new Exception("An error occurred while retrieving donations for the donor.", ex);
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


       
    }
}
