using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
    public class RecipientRepo : IRecipient
    {

        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;

        public RecipientRepo(AppDbContext appDbContext, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
        }

        public async Task<LoginResponse> LoginRecipientAsync(RecipientsLoginDTO recipientLoginDTO)
        {
            var getUser = await FindUserByEmail(recipientLoginDTO.RecipientEmail!);

            if (getUser == null)
            {
                return new LoginResponse(false, "User not found, sorry");
            }

            bool checkPassword = BCrypt.Net.BCrypt.Verify(recipientLoginDTO.Password, getUser.Password);
            if (checkPassword)
                return new LoginResponse(true, "Login successful", GenerateJWTToken(getUser));
            else
                return new LoginResponse(false, "Invalid credentials");
        }

       public async Task<RegistrationResponse> RegisterRecipientAsync(RecipientDTO recipientDTO)
        {
            var getUser = await FindUserByEmail(recipientDTO.RecipientEmail!);
            if (getUser != null)
                return new RegistrationResponse(false, "User already exist");

            appDbContext.Recipients.Add(new Recipient()
            {
                RecipientName = recipientDTO.RecipientName,
                RecipientEmail = recipientDTO.RecipientEmail,
                RecipientAddress = recipientDTO.RecipientAddress,
                RecipientPhoneNum = recipientDTO.RecipientPhoneNum,
                Password = BCrypt.Net.BCrypt.HashPassword(recipientDTO.Password)
            });
            await appDbContext.SaveChangesAsync();
            return new RegistrationResponse(true, "Registration completed");
        }


        public async Task<List<Recipient>> GetRecipientListAsync()
        {
            return await appDbContext.Recipients.ToListAsync();
        }


        public async Task<Recipient> GetRecipientByEmail(string email)
        {
            var result = await appDbContext.Recipients.FirstOrDefaultAsync(u => u.RecipientEmail == email);
            
           

            return result;
        }

        public async Task<int> UpdateRecipientAsync(int id, Recipient recipient)
        {

            string message = string.Empty;

            var result = await appDbContext.Recipients.Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.RecipientName, recipient.RecipientName)
                .SetProperty(u => u.RecipientEmail, recipient.RecipientEmail)
                .SetProperty(u => u.RecipientPhoneNum, recipient.RecipientPhoneNum)
                .SetProperty(u => u.RecipientAddress, recipient.RecipientAddress)
                .SetProperty(u => u.Password, recipient.Password)
                );

            if (result != null)
            {
               message = "Update failed";
            }
            else
            {
                message = "Update successful";
            }

            return result;
            
        }


        public async Task<int> DeleteRecipientAsync(int id)
        {
            return await appDbContext.Recipients.Where(u => u.Id == id).ExecuteDeleteAsync();
        }

        private string GenerateJWTToken(Recipient recipient)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, recipient.Id.ToString()),
                new Claim(ClaimTypes.Name, recipient.RecipientName),
                new Claim(ClaimTypes.Email, recipient.RecipientEmail),
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<Recipient> FindUserByEmail(string email) =>
          await appDbContext.Recipients.FirstOrDefaultAsync(u => u.RecipientEmail == email);

       
    }
}
