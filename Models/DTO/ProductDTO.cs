namespace FoodShareNetwork.Models.DTO
{
    public class ProductDTO
    {
        public string ProdName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime CookedMealDate { get; set; }
    }
}
