using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int RecipientId { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
