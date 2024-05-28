using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
    public class DonorRepo:IDonor
    {
        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;
        
        public DonorRepo(AppDbContext appDbContext, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
        }




       public async Task<LoginResponse> LoginDonorAsync(DonorLoginDTO donorLoginDTO)
        {
            var getUser = await FindUserByEmail(donorLoginDTO.DonorEmail);

            if (getUser == null)
            {
                return new LoginResponse(false, "User not found, sorry");
            }

            bool checkPassword = BCrypt.Net.BCrypt.Verify(donorLoginDTO.Password, getUser.Password);
            if (checkPassword)
                return new LoginResponse(true, "Login successful", GenerateJWTToken(getUser));
            else
                return new LoginResponse(false, "Invalid credentials");
        }

       public async Task<RegistrationResponse> RegisterDonorAsync(DonorDTO donorDTO)
        {
            var getUser = await FindUserByEmail(donorDTO.DonorEmail!);
            if (getUser != null)
                return new RegistrationResponse(false, "User already exist");

            appDbContext.Donors.Add(new Donor()
            {
                DonorName = donorDTO.DonorName,
                DonorEmail = donorDTO.DonorEmail,
                DonorAddress = donorDTO.DonorAddress,
                DonorPhoneNum = donorDTO.DonorPhoneNum,
                Password = BCrypt.Net.BCrypt.HashPassword(donorDTO.Password)
            });
            await appDbContext.SaveChangesAsync();
            return new RegistrationResponse(true, "Registration completed");
        }
       
        public async Task<Donor> GetDonorByEmail(string email)
        {
            var result =  appDbContext.Donors.FirstOrDefault(u => u.DonorEmail == email); ;

            return result;
        }

        public async Task<List<Donor>> GetDonorListAsync()
        {
            var result = await appDbContext.Donors.ToListAsync();
            
            return result;
        }

        public async Task<int> UpdateDonorAsync(int id, Donor donor)
        {
            string message = string.Empty;

            var result = await appDbContext.Donors.Where(u => u.DonorId == id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.DonorName, donor.DonorName)
                .SetProperty(u => u.DonorEmail, donor.DonorEmail)
                .SetProperty(u => u.DonorPhoneNum, donor.DonorPhoneNum)
                .SetProperty(u => u.DonorAddress, donor.DonorAddress)
                .SetProperty(u => u.Password, donor.Password)
                );

            return result;
        }


       public async Task<int> DeleteDonorAsync(int id)
        {
            return await appDbContext.Donors.Where(u => u.DonorId == id).ExecuteDeleteAsync();
        }

        //Token
        private string GenerateJWTToken(Donor donor)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, donor.DonorId.ToString()),
                new Claim(ClaimTypes.Name, donor.DonorName),
                new Claim(ClaimTypes.Email, donor.DonorEmail),
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

        private async Task<Donor> FindUserByEmail(string email) =>
           await appDbContext.Donors.FirstOrDefaultAsync(u => u.DonorEmail == email);

       
    } 
}
