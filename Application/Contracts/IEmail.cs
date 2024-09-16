using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
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
        Task<string?> GetDonorEmail(int donorId);
        Task ResetDonor(MailRequestDTO mail, string email);
        Task NotifyRecipient(RecipientMailDTO mailRecipient, int requestId);

        Task<string?> GetRecipientEmail(int requestId);
        Task<string> GetRecipientInfo(int  recipientId);
        Task <string?> ResetPassword(string email, Donor donor);
    }
}
