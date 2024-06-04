using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DonorDTO
    {
        public string DonorName { get; set; } = string.Empty;

        
        public string DonorEmail { get; set; } = string.Empty;

        public string DonorPhoneNum { get; set; } = string.Empty;

        public string DonorAddress { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }
}
