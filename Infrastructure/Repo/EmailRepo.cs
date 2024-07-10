using Application.Contracts;
using Application.DTOs;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repo
{
    public class EmailRepo : IEmail
    {
        private readonly EmailSettings _settings;
        private readonly AppDbContext _appDbContext;
        private readonly IRequest _request;

        public EmailRepo(IOptions<EmailSettings> settings, AppDbContext appDbContext, IRequest request)
        {
            this._settings = settings.Value;
            this._appDbContext = appDbContext;
            this._request = request;
        }

        public async Task SendEmailAsync(MailRequestDTO mailRequest, string dEmail)
        {
            try
            {   string? donorEmail = await GetDonorEmailFromDatabase(dEmail);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_settings.Email));
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = mailRequest.Body;

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.Email, _settings.Password);

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

        public async Task NotifyRecipient(RecipientMailDTO mailRecipient, int  requestId)
        {
            try
            {
                string? recipientEmail = await GetRecipientEmail(requestId);
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_settings.Email));
                email.To.Add(MailboxAddress.Parse(mailRecipient.ToEmail));
                email.Subject = mailRecipient.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = mailRecipient.Body;

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.Email, _settings.Password);

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
        public async Task<string?> GetDonorEmailFromDatabase(string email)
        {
            try
            {
                var donor = await _appDbContext.Donors.FirstOrDefaultAsync(d => d.DonorEmail == email);
                return donor.DonorEmail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving donor email: {ex.Message}");
                return null; //
            }
            
        }

        public async Task<string?> GetRecipientEmail(int requestId)
        {
            // Get the recipient ID asynchronously
            var recipientId = await _request.GetRecipientIdForDonationRequestAsync(requestId);

            if (recipientId.HasValue)
            {
                // Retrieve the recipient from the database
                var recipient = await _appDbContext.Recipients.FirstOrDefaultAsync(r => r.Id == recipientId.Value);

                // Return the recipient's email
                return recipient?.RecipientEmail;
            }

            return null; // Handle the case where recipientId is null or recipient is not found
        }


        public async Task<string?> GetDonorEmail(int donorId)
        {
            var donorEmail = await _appDbContext.FoodDonations
                .Where(fd => fd.DonorId == donorId)
                .Join(_appDbContext.Donors, fd => fd.DonorId, d => d.DonorId, (fd, d) => new { FoodDonation = fd, Donor = d })
                .Join(_appDbContext.FoodItems, j => j.FoodDonation.ItemId, fi => fi.Id, (j, fi) => new { j.Donor, FoodItem = fi })
                .Select(j => j.Donor.DonorEmail)
                .FirstOrDefaultAsync();

            return donorEmail;
        }
    }
}
