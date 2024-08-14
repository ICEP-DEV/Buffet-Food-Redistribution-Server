using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FoodDonationDTO
    {
        public int DonorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCooked { get; set; }

    }
}
