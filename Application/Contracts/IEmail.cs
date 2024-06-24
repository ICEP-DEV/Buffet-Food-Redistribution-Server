﻿using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IEmail
    {
        Task SendEmailAsync(MailRequestDTO mailRequest);
    }
}
