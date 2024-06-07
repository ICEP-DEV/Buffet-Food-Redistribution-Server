using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IFoodItem
    {
        //bool AddFoodItem(FoodItem item, string imagePath);
        //Task<int> AddFoodItemAsync(FoodItem item);
        Task AddFoodItemsAsync(FoodItem foodItem); 
        Task<IEnumerable<FoodItem>> GetFoodItemsAsync();

        //Task<int> DeleteItemAsync(int id);
    }
}
