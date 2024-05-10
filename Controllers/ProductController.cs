using FoodShareNetwork.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace FoodShareNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        UserDbContext _userDbContext;
        public ProductController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }


        [HttpPost]

        public IActionResult FetchProducts(ProductDTO productsDTO)
        {
            var productsEntity = new Product()
            {
                ProdName = productsDTO.ProdName,
                Description = productsDTO.Description,
                Quantity = productsDTO.Quantity,
                CookedMealDate = productsDTO.CookedMealDate

            };

            _userDbContext.Products.Add(productsEntity);

            _userDbContext.SaveChanges();

            return Ok("Products added successfully");
        }


        [HttpGet("List")]

        public async Task<ActionResult<IEnumerable<Product>>> GetAllUsers()
        {
            if (_userDbContext == null)
            {
                return NotFound();
            }

            return await _userDbContext.Products.ToListAsync();
        }

    }
}
