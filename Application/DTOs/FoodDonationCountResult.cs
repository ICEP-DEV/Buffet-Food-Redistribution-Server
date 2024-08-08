using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FoodDonationCountResult
    {
        public List<FoodDonationDTO> ?Donations { get; set; } 
        public int TotalCount { get; set; }
    }
}
