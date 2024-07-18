using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
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
    public class AdminRepo:IAdmin
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public AdminRepo(AppDbContext appDbContext, IConfiguration configuration)
        {
            this._appDbContext = appDbContext;   
            this._configuration = configuration;
        }

        public async Task<RegistrationResponse> Register(Admin admin)
        {

            _appDbContext.Admin.Add(new Admin()
            {
                Name = admin.Name,
                Email = admin.Email,
                Phone = admin.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(admin.Password)
            });

            await _appDbContext.SaveChangesAsync();
            return new RegistrationResponse(true, "Admin registered");
        }

        public async Task<LoginResponse> LoginAdmin(AdminDTO admin)
        {
            var getUser = await FindUserByEmail(admin.Email);

            if (getUser == null)
            {
                return new LoginResponse(false, "User not found");
            }
            bool checkPassword = BCrypt.Net.BCrypt.Verify(admin.Password, getUser.Password);
            if (checkPassword)
                return new LoginResponse(true, "Login successful", GenerateJWTToken(getUser));
            else
                return new LoginResponse(false, "Invalid credentials");
        }
        

        

        private string GenerateJWTToken(Admin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString()),
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim("AdminId",admin.AdminId.ToString()),
               // new Claim(ClaimTypes.StreetAddress, donor.DonorAddress),
                new Claim(ClaimTypes.MobilePhone, admin.Phone.ToString())

            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<Admin> FindUserByEmail(string email) =>
          await _appDbContext.Admin.FirstOrDefaultAsync(a => a.Email == email);
    }
}
