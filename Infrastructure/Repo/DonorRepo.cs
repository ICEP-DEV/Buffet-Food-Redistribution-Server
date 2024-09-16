using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IHttpContextAccessor _httpContextAccessor;




       //private Donor donor;
        
        public DonorRepo(AppDbContext appDbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
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
            else
            {
                appDbContext.Donors.Add(new Donor()
                {
                    DonorName = donorDTO.DonorName,
                    DonorEmail = donorDTO.DonorEmail,
                    DonorAddress = donorDTO.DonorAddress,
                    DonorPhoneNum = donorDTO.DonorPhoneNum,
                    Password = BCrypt.Net.BCrypt.HashPassword(donorDTO.Password)
                });
                await appDbContext.SaveChangesAsync();
            }
            
            return new RegistrationResponse(true, "Registration completed");
        }

        public Task<Donor> GetDonorByEmail(string email)
        {
            var result = appDbContext.Donors.FirstOrDefault(u => u.DonorEmail == email);

            return Task.FromResult(result)!;
        }

        public async Task<List<Donor>> GetDonorListAsync()
        {
            var result = await appDbContext.Donors.ToListAsync();
            
            return result;
        }

        public async Task<int> UpdateDonorAsync( Donor donor)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(donor.Password))
            {
                donor.Password = BCrypt.Net.BCrypt.HashPassword(donor.Password);
            }

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                // Handle case where user ID claim is not found or cannot be parsed
                throw new ApplicationException("User ID claim not found or invalid.");
            }
           
            // Update the donor profile based on the current logged-in user's ID
            var result = await appDbContext.Donors
                .Where(u => u.DonorId == userId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.DonorName, donor.DonorName)
                    .SetProperty(u => u.DonorEmail, donor.DonorEmail)
                    .SetProperty(u => u.DonorPhoneNum, donor.DonorPhoneNum)
                    .SetProperty(u => u.DonorAddress, donor.DonorAddress)
                    
                );

            return result;
        }

        public async Task<int> TotalDonors()
        {
            return await appDbContext.Donors.CountAsync();
        }
        public async Task<int> DeleteDonorAsync(int id)
        {
            var donorIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("DonorId");

            if (donorIdClaim == null || !int.TryParse(donorIdClaim.Value, out int donorId))
        {
            // Handle case where donorId claim is not found or cannot be parsed
            // For example, return an error code or throw an exception
            throw new ApplicationException("DonorId claim not found or invalid.");
        }

        // Perform the delete operation based on the current user's id
        var affectedRecords = await appDbContext.Donors
            .Where(d => d.DonorId == donorId && d.DonorId == id)
            .ExecuteDeleteAsync();

        return affectedRecords;
        }

        //Token
        private string GenerateJWTToken(Donor donor)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, donor.DonorId.ToString()),
                new Claim(ClaimTypes.Name, donor.DonorName),
                new Claim(ClaimTypes.Email, donor.DonorEmail),
                new Claim("DonorId",donor.DonorId.ToString()),
                new Claim(ClaimTypes.StreetAddress, donor.DonorAddress),
                new Claim(ClaimTypes.MobilePhone, donor.DonorPhoneNum.ToString())
                
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

        public Task<Donor> GetDonorProfile()
        {
            var identity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var donorIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("DonorId");

            var Name = identity?.FindFirst(ClaimTypes.Name)?.Value;
            var Email = identity?.FindFirst(ClaimTypes.Email)?.Value;
            var Address = identity?.FindFirst(ClaimTypes.StreetAddress)?.Value;
            var PhoneNum = identity?.FindFirst(ClaimTypes.MobilePhone)?.Value;

            if (donorIdClaim != null && int.TryParse(donorIdClaim.Value, out int donorId))
            {
                var DonorProfile = new Donor
                {
                    DonorId = donorId,
                    DonorName = Name!,
                    DonorEmail = Email!,
                    DonorPhoneNum = PhoneNum!,
                    DonorAddress = Address!
                };

                return Task.FromResult(DonorProfile); // Wrap the result in a completed Task
            }
            else
            {
                
                return Task.FromResult<Donor>(null!); 
            }
        }


        private async Task<Donor?> FindUserByEmail(string email)
        {
            var donor = await appDbContext.Donors.FirstOrDefaultAsync(u => u.DonorEmail == email);
            return donor;  // Donor? indicates that Donor is nullable
        }

        /* public async Task<IActionResult> GetCurrentUser(int id)
         {

         }*/

    } 
}
