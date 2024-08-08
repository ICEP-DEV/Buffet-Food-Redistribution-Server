using Application.Contracts;
using Domain.Entities;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        //private IFileUploading _fileUpload;
        private readonly IFoodItem _foodItem;

        public FoodItemController( IFoodItem foodItem)
        {
            //this._fileUpload = fileUploading;
            this._foodItem = foodItem;
        }


        [HttpPost]
        
        public async Task<IActionResult> AddFoodItem(FoodItem foodItem)
        {

            await _foodItem.AddFoodItemsAsync(foodItem);
            return Ok("Food item added successfully");
        }



       // [HttpPost("AddItem")]
        /*public async Task<IActionResult> AddFoodItem([FromBody] FoodItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var imagePath = await _fileUpload.SaveImage(item.ImageFile);
          

            var foodItemId = await _foodItem.AddFoodItemAsync(new FoodItem
            {
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                DateCooked = item.DateCooked,
                Description = item.Description,
                Address = item.Address
              //  ItemImage = imagePath
                
            });

            if (foodItemId > 0)
            {
                return Ok(new { FoodItemId = foodItemId });
            }
            else
            {
                return StatusCode(500, "Error adding food item to the database.");
            }
        }*/

        [HttpGet]
        public async Task<IEnumerable> GetFoodItemAsync()
        {
            var result = await _foodItem.GetFoodItemsAsync();

            return result;

        }

        [HttpGet("TotalQuantity")]

        public async Task <IActionResult> GetTotalQuantity()
        {
            var totalQuantity = await _foodItem.GetTotalQuantity();

            return Ok (totalQuantity);
        }

    }
    
}
