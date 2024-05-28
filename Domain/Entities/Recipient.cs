using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Recipient
    {
        public int Id { get; set; }

        [ForeignKey("Admin")]
        
        public int AdminId { get; set; }

        public string RecipientName { get; set; } = string.Empty;

        public string RecipientEmail { get; set; } = string.Empty;

        public string RecipientPhoneNum { get; set; } = string.Empty;

        public string RecipientAddress { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public virtual Admin? Admin { get; set; }
    }
}
