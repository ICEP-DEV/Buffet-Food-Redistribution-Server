using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
using BCrypt.Net;
using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repo
{
    public class RecipientRepo : IRecipient
    {

        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecipientRepo(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this._appDbContext = appDbContext;
            this._httpContextAccessor = httpContextAccessor;
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

            _appDbContext.Recipients.Add(new Recipient()
            {
                RecipientName = recipientDTO.RecipientName,
                RecipientEmail = recipientDTO.RecipientEmail,
                RecipientAddress = recipientDTO.RecipientAddress,
                RecipientPhoneNum = recipientDTO.RecipientPhoneNum,
                Password = BCrypt.Net.BCrypt.HashPassword(recipientDTO.Password)
            });
            await _appDbContext.SaveChangesAsync();
            return new RegistrationResponse(true, "Registration completed");
        }


        public async Task<List<Recipient>> GetRecipientListAsync()
        {
            return await _appDbContext.Recipients.ToListAsync();
        }


        public async Task<Recipient> GetRecipientByEmail(string email)
        {
            var result = await _appDbContext.Recipients.FirstOrDefaultAsync(u => u.RecipientEmail == email);
            
           

            return result;
        }

        public async Task<int> UpdateRecipientAsync(int id, Recipient recipient)
        {
            string message = string.Empty;
            var userIdClaim = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new ApplicationException("User ID claim not found or invalid.");
            }

            // Hash the password if it's provided
            if (!string.IsNullOrEmpty(recipient.Password))
            {
                recipient.Password = BCrypt.Net.BCrypt.HashPassword(recipient.Password);
            }

            // Perform the update operation
            var result = await _appDbContext.Recipients.Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.RecipientName, recipient.RecipientName)
                    .SetProperty(u => u.RecipientEmail, recipient.RecipientEmail)
                    .SetProperty(u => u.RecipientPhoneNum, recipient.RecipientPhoneNum)
                    .SetProperty(u => u.RecipientAddress, recipient.RecipientAddress)
                    .SetProperty(u => u.Password, recipient.Password) // Updated hashed password
                );

            if (result == 0)
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
            return await _appDbContext.Recipients.Where(u => u.Id == id).ExecuteDeleteAsync();
        }

        public Task<Recipient> GetRecipientProfile()
        {
            var identity = _httpContextAccessor?.HttpContext?.User.Identity as ClaimsIdentity;
            var recipientIdClaim = _httpContextAccessor?.HttpContext?.User.FindFirst("RecipientId");

            var Name = identity?.FindFirst(ClaimTypes.Name)?.Value;
            var Email = identity?.FindFirst(ClaimTypes.Email)?.Value;
            var Address = identity?.FindFirst(ClaimTypes.StreetAddress)?.Value;
            var PhoneNum = identity?.FindFirst(ClaimTypes.MobilePhone)?.Value;

            if (recipientIdClaim != null && int.TryParse(recipientIdClaim.Value, out int Id))
            {
                var RecipientProfile = new Recipient
                {
                    Id = Id,
                    RecipientName = Name,
                    RecipientEmail = Email,
                    RecipientPhoneNum = PhoneNum,
                    RecipientAddress = Address
                };

                return Task.FromResult(RecipientProfile); // Return completed Task with RecipientProfile
            }
            else
            {
                // Handle case where RecipientId claim is not found or cannot be parsed
                // For example, you can return null or throw an exception
                return Task.FromResult<Recipient>(null); // Return null wrapped in a Task
            }
        }

        private string GenerateJWTToken(Recipient recipient)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, recipient.Id.ToString()),
                 new Claim("RecipientId",recipient.Id.ToString()),
                new Claim(ClaimTypes.Name, recipient.RecipientName),
                new Claim(ClaimTypes.Email, recipient.RecipientEmail),
                new Claim(ClaimTypes.StreetAddress,recipient.RecipientAddress),
                new Claim(ClaimTypes.MobilePhone,recipient.RecipientAddress)
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
          await _appDbContext.Recipients.FirstOrDefaultAsync(u => u.RecipientEmail == email);

       
    }
}
