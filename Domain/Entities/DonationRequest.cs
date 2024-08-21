using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DonationRequest
    {
        [Key]
        public int RequestId { get; set; }

        public int DonationId { get; set; }

        [ForeignKey("DonationId")]   // Foreign key annotation
        public FoodDonation? Donation { get; set; }

        public int RecipientId { get; set; }

        [ForeignKey("RecipientId")]   // Foreign key annotation
        public Recipient? Recipient { get; set; }

        public string Status { get; set; } = string.Empty;

        public bool isCollected { get; set; } = false;
    }
}
