using Application.Contracts;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
    public class FoodItemRepo : IFoodItem
    {

        private readonly AppDbContext _appDbContext;
  
        
        public FoodItemRepo(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
           
        }


        //Authorized users add food items
        public async Task AddFoodItemsAsync(FoodItem foodItem)
        {
            _appDbContext.FoodItems.Add(foodItem);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> GetTotalQuantity()
        {
            return await _appDbContext.FoodItems.SumAsync(f => f.Quantity);
        }

        public async Task<IEnumerable<FoodItem>> GetFoodItemsAsync()
        {

            return await _appDbContext.FoodItems
                              .Where(fi => !fi.IsRequested) // Filter where IsRequested is false
                              .ToListAsync();
        }

        public async Task<IEnumerable<FoodItem>> GetItemsAsync()
        {
            return await _appDbContext.FoodItems
                             .Where(fi => fi.IsRequested) 
                             .ToListAsync();
        }
    }
}
