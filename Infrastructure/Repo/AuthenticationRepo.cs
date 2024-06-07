using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
    public class AuthenticationRepo:ICustomAuthentication
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _appDbContext;
        
        public AuthenticationRepo(IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _appDbContext = appDbContext;
        }

        public async Task SignInWithCustomClaimsAsync(Donor donor)
        {
            var user = await FindUserByIdAsync(donor.DonorId);
            var claims = new List<Claim>
            {
                new Claim("CanFoodItems","true")
            };

            var identity = new ClaimsIdentity(claims,"Custom");
            var pricipal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(pricipal);
        }





        public async Task<Donor> FindUserByIdAsync(int donorId)
        {
            // Retrieve user information from the database based on userId
            var user = await _appDbContext.Donors.FirstOrDefaultAsync(u => u.DonorId == donorId);
            return user;
        }
    }
}
