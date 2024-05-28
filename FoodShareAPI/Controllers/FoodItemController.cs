using Application.Contracts;
using Domain.Entities;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private IFileUploading _fileUpload;
        private readonly IFoodItem _foodItem;

        public FoodItemController(IFileUploading fileUploading, IFoodItem foodItem) 
        { 
           this._fileUpload = fileUploading;
            this._foodItem = foodItem;
        }


        [HttpPost("AddItem")]

        public IActionResult AddFoodItem(FoodItem item) 
        {
            var status = new ItemDTO();

            if(!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please enter valid data";
                return Ok(status);
            }

            if(item.ImageFile != null)
            {
                var fileResult = _fileUpload.SaveImage(item.ImageFile);
                if(fileResult.Item1 == 1)
                {
                    item.ItemImage = fileResult.Item2;
                }

                var productResult = _foodItem.AddFoodItem(item);
                if(productResult)
                {
                    status.StatusCode = 1;
                    status.Message = "Item added successfully";
                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Error adding product";
                }
                
            }

            return Ok(status);
        }


        [HttpGet("GetImage")]

        public async Task <IEnumerable> GetFoodItemAsync()
        {
            var result = await _foodItem.GetFoodItemsAsync();

            return result;
        }
        
    }
}
