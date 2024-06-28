using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IEmail
    {
        Task SendEmailAsync(MailRequestDTO mailRequest,int donorId);
        Task<string?> GetDonorEmailFromDatabase(int donorId);
        Task<string> GetDonorEmail(int donorId, int itemId);
    }
}
