﻿using Application.Contracts;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public bool AddFoodItem(FoodItem item, string imagePath)
        {
            
            try
            {
                item.ItemImage = imagePath;
                item.ItemName = item.ItemName;
                item.Quantity = item.Quantity;
                item.DateCooked = item.DateCooked;
                item.Description = item.Description;
                _appDbContext.FoodItems.Add(item);   

               
              _appDbContext.SaveChanges();
                return true;
            }
            catch(Exception ex) 
            {
                return false;
            }   
        }

        public async Task<IEnumerable<FoodItem>> GetFoodItemsAsync()
        {
            return await _appDbContext.FoodItems.ToListAsync();
        }
    }
}
