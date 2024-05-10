using System;

namespace WebApplication2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProdName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime CookedMealDate { get; set; }

    }


}


