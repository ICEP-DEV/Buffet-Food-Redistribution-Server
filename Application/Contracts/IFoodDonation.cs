using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IFoodDonation
    {
         void PopulateFoodDonations(int donorId, IEnumerable<FoodItem> foodItems);
         IEnumerable<FoodDonationDTO> GetDonationsAsync();
        Task<IEnumerable<FoodDonationDTO>> GetDonationsForDonorAsync(int donorId);

        //Task AddFoodDonationAsync(int donorId, int foodItemId, int quantity, DateTime dateCooked);
    }
}
