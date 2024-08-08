using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IFoodDonation
    {
         void PopulateFoodDonations(FoodItem foodItems);
         IEnumerable<FoodDonationDTO> GetDonationsAsync();
         Task<List<FoodDonationDTO>> GetDonationsForDonorAsync();

        Task<int> GetTotalDonationsCountAsync();


        //Task AddFoodDonationAsync(int donorId, int foodItemId, int quantity, DateTime dateCooked);
    }
}
