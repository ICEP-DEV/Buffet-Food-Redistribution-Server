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
        bool AddFoodItem(FoodItem item);
        Task<IEnumerable<FoodItem>> GetFoodItemsAsync();
    }
}
