using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RecipientDTO
    {
        public string RecipientName { get; set; } = string.Empty;

        [EmailAddress]
        public string RecipientEmail { get; set; } = string.Empty;

        public string RecipientPhoneNum { get; set; } = string.Empty;

        public string RecipientAddress { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }
}
