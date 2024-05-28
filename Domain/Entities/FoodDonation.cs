using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FoodDonation
    {

        public int Id { get; set; }

        [ForeignKey("Donors") ]
        public int DonorId { get; set; }

        [ForeignKey("Recipients")]
        public int RecipientId { get; set; }

        [ForeignKey("FoodItems")]
        public int ItemId { get; set; }

        [ForeignKey("FoodItems")]
        public DateTime DateCooked { get; set; }

        [ForeignKey("FoodItems")]
        public int Quantity { get; set; }

        

        /*[ForeignKey("FoodItems")]
        public string ItemName { get; set; } = string.Empty;*/


      
        
      



    }
}
