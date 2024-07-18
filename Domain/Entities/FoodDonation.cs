using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FoodDonation
    {
        [Key]
        public int DonationId { get; set; }

        [ForeignKey("Donor")]
        public int DonorId { get; set; }
        public Donor? Donor { get; set; }

        [ForeignKey("FoodItem")]
        public int ItemId { get; set; }
        public FoodItem? FoodItem { get; set; }

        public int Quantity { get; set; }
        public DateTime DateCooked { get; set; }



        /*[ForeignKey("FoodItems")]
        public string ItemName { get; set; } = string.Empty;*/








    }
}
