﻿using Application.Contracts;
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
            
            var recipientId = await _request.GetRecipientIdForDonationRequestAsync(requestId);

            if (recipientId.HasValue)
            {
                
                var recipient = await _appDbContext.Recipients.FirstOrDefaultAsync(r => r.Id == recipientId.Value);

                
                return recipient?.RecipientEmail;
            }

            return null; 
        }

    public async Task<string> GetRecipientInfo(int requestId)
{
    var donationRequest = await _appDbContext.DonationRequests
                                            .FirstOrDefaultAsync(dr => dr.RequestId == requestId);

    if (donationRequest == null)
    {
        throw new InvalidOperationException($"Donation request with ID {requestId} not found.");
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
    }
}
