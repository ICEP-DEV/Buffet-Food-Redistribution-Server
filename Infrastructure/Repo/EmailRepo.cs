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

        public EmailRepo(IOptions<EmailSettings> settings, AppDbContext appDbContext)
        {
            this._settings = settings.Value;
            this._appDbContext = appDbContext;
        }

        public async Task SendEmailAsync(MailRequestDTO mailRequest, int donorId)
        {
            try
            {   string? donorEmail = await GetDonorEmailFromDatabase(donorId);

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


        public async Task<string?> GetDonorEmailFromDatabase(int donorId)
        {
            try
            {
                var donor = await _appDbContext.Donors.FirstOrDefaultAsync(d => d.DonorId == donorId);
                return donor.DonorEmail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving donor email: {ex.Message}");
                return null; //
            }
            
        }
        /* public string GetDonationsDetails()
         {


             var foodDonations = _appDbContext.FoodDonations.ToList();
             var donors = _appDbContext.Donors;
             var foodItems = _appDbContext.FoodItems;

             StringBuilder sb = new StringBuilder();

             var donationDetails = foodDonations.Select(fd => new
             {
                 DonationId = fd.DonationId,
                // DonationDate = fd.DonationDate,
                 Donor = donors.FirstOrDefault(d => d.DonorId == fd.DonorId),
                 FoodItem = foodItems.FirstOrDefault(fi => fi.Id == fd.ItemId),


             });

             return sb.ToString();
         */

        public async Task<string> GetDonorEmail(int donorId, int itemId)
        {
            var donorEmail = await _appDbContext.FoodDonations
                .Where(fd => fd.DonorId == donorId && fd.ItemId == itemId)
                .Join(_appDbContext.Donors, fd => fd.DonorId, d => d.DonorId, (fd, d) => new { FoodDonation = fd, Donor = d })
                .Join(_appDbContext.FoodItems, j => j.FoodDonation.ItemId, fi => fi.Id, (j, fi) => new { j.Donor, FoodItem = fi })
                .Select(j => j.Donor.DonorEmail)
                .FirstOrDefaultAsync();

            return donorEmail;
        }
    }
}
