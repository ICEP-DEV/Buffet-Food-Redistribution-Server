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
        Task SendEmailAsync(MailRequestDTO mailRequest, string email);
        Task<string?> GetDonorEmailFromDatabase(string email);
        Task<string> GetDonorEmail(int donorId);

        Task NotifyRecipient(RecipientMailDTO mailRecipient, int requestId);

        Task<string> GetRecipientEmail(int requestId);
    }
}
