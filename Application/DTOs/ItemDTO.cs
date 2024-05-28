using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ItemDTO
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public DateTime DateCooked { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ItemImage { get; set; } = string.Empty;
    }
}
