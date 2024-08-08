using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FoodItem
    {
        public int Id { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public DateTime DateCooked { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Contact { get; set; } = string.Empty;

        public bool IsRequested { get; set; } = false;

        //public string ItemImage { get; set; } = string.Empty;



        // [NotMapped]
        // public IFormFile? ImageFile { get; set; } 

    }
}
