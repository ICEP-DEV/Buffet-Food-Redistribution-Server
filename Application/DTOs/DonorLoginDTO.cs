﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DonorLoginDTO
    {
        [EmailAddress]
        public string DonorEmail { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
