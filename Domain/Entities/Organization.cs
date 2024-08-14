using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Organization
    {
        public int OrganizationId { get; set; } 

        public string OrganizationName { get; set;} = string.Empty;

        public int Regno { get; set; }
    }
}
