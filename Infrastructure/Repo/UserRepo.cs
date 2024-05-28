using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repo
{
    //User Contract
    public class UserRepo : IUser
    {
        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;

        //Database connection 
        public UserRepo(AppDbContext appDbContext,IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
        }

        //Login route
        public async Task<LoginResponse> LoginUserAsync(UserLoginDTO userLoginDTO)
        {
            //throw new NotImplementedException();

            var getUser = await FindUserByEmail(userLoginDTO.Email!);

            if (getUser == null)
            {
                return new LoginResponse(false, "User not found, sorry");
            }

            bool checkPassword = BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, getUser.Password);
            if (checkPassword)
                return new LoginResponse(true, "Login successful", GenerateJWTToken(getUser));
            else
                return new LoginResponse(false, "Invalid credentials");

        }

        //Token service 
        private string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials:credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //Registration
        public async Task<RegistrationResponse> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO)
        {

            var getUser = await FindUserByEmail(userRegistrationDTO.Email!);
            if (getUser != null)
                return new RegistrationResponse(false, "User already exist");

            appDbContext.Users.Add(new User()
            {
                Name = userRegistrationDTO.Name,
                Email = userRegistrationDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userRegistrationDTO.Password)
            });
            await appDbContext.SaveChangesAsync();
            return new RegistrationResponse(true, "Registration completed");

        }

        //Find user by email
        private async Task<User> FindUserByEmail(string email) =>
            await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
