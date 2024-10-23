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
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            {
                string? donorEmail = _appDbContext.FoodItems.FirstOrDefault(e => e.Contact == dEmail)?.Contact;

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

        public async Task ResetDonor(MailRequestDTO mail, string donorEmail)
        {
            try
            {
                //var dEmail = _appDbContext.Donors.FirstOrDefault(d => d.DonorEmail == donorEmail);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_settings.Email));
                email.To.Add(MailboxAddress.Parse(mail.ToEmail));
                email.Subject = mail.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = mail.Body;

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                 await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                 await smtp.AuthenticateAsync(_settings.Email, _settings.Password);

                 await smtp.SendAsync(email);
                 await smtp.DisconnectAsync(true);
            }
            catch(Exception ex) 
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
                return donor!.DonorEmail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving donor email: {ex.Message}");
                return null; //
            }
            
        }

        public async Task<string?> GetRecipientEmail(int donationId)
        {
            
            var recipientId = await _request.GetRecipientIdForDonationRequestAsync(donationId);

            if (recipientId.HasValue)
            {
                
                var recipient = await _appDbContext.Recipients.FirstOrDefaultAsync(r => r.Id == recipientId.Value);

                
                return recipient?.RecipientEmail;
            }

            return null; 
        }

    public async Task<string> GetRecipientInfo(int donationId)
{
    var donationRequest = await _appDbContext.DonationRequests
                                            .FirstOrDefaultAsync(dr => dr.DonationId == donationId);

    if (donationRequest == null)
    {
        throw new InvalidOperationException($"Donation request with ID {donationId} not found.");
    }

    var recipient = await _appDbContext.Recipients
                                       .FirstOrDefaultAsync(r => r.Id == donationRequest.RecipientId);

    if (recipient == null)
    {
        throw new InvalidOperationException($"Recipient with ID {donationRequest.RecipientId} not found in the database.");
    }

    // Create a DTO object with recipient information
    var recipientData = new RecipientDTO
    {
        RecipientName = recipient.RecipientName,
        RecipientEmail = recipient.RecipientEmail,
        RecipientAddress = recipient.RecipientAddress,
        RecipientPhoneNum = recipient.RecipientPhoneNum,
    };

    // Serialize recipientData to JSON string
    string json = JsonSerializer.Serialize(recipientData);

    return json;
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

        public async Task<string?> ResetPassword(string email, Donor donor)
        {
            var checkDonor = await FindUserByEmail(email);
           

            if (checkDonor != null)
            {
                var findDonor = await _appDbContext.Donors.FirstOrDefaultAsync(d => d.DonorEmail == email);
                var reset = await _appDbContext.Donors.Where(d => d.DonorEmail == email).ExecuteUpdateAsync(
                    setters => setters.SetProperty(u => u.Password, BCrypt.Net.BCrypt.HashPassword (donor.Password)));
                return ("Update successful");
            }
            else
            {
                throw new Exception("User not found!");
            }
        }

        private async Task<Donor?> FindUserByEmail(string email)
        {
            var donor = await _appDbContext.Donors.FirstOrDefaultAsync(u => u.DonorEmail == email);
            return donor;  // Donor? indicates that Donor is nullable
        }
    }
}
