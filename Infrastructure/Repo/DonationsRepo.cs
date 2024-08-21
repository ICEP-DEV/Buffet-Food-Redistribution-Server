using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Globalization;
using System.Security.Claims;


namespace Infrastructure.Repo
{
    public class DonationsRepo : IFoodDonation
    {
        
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DonationsRepo(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {

            this._appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }




        public void PopulateFoodDonations(FoodItem foodItems)
        {
            var donorIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("DonorId");

            // Check if the claim exists and if its value can be parsed as an integer
            if (donorIdClaim != null && int.TryParse(donorIdClaim.Value, out int donorId))
            {
                // The value of the claim has been successfully parsed as an integer
                foodItems.DateCooked = DateTime.Now;
                _appDbContext.FoodItems.Add(foodItems);
                _appDbContext.SaveChanges();

                var foodDonation = new FoodDonation
                {
                    DateCooked = DateTime.Now,
                    DonorId = donorId,
                    ItemId = foodItems.Id,
                    Quantity = foodItems.Quantity,
                };

                _appDbContext.FoodDonations.Add(foodDonation);
                _appDbContext.SaveChanges();
            }
            else
            {
                // Handle the case where the "DonorId" claim is not found or its value cannot be parsed as an integer
                // You may want to log an error, return a specific HTTP response, or take other appropriate action

            }
        }

        public async Task<List<FoodDonationDTO>> GetDonationsForDonorAsync()
        {
            try
            {
                var donorIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("DonorId");
                var identity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;

                if (donorIdClaim == null)
                {
                    throw new InvalidOperationException("User's donor ID not found in claims.");
                }

                // Retrieve the donorId as a string
                var donorIdString = donorIdClaim.Value;
                var DonorName = identity?.FindFirst(ClaimTypes.Name)?.Value;

                // Convert donorIdString to int
                if (!int.TryParse(donorIdString, out int donorId))
                {
                    throw new InvalidOperationException("Unable to parse donor ID as integer.");
                }

                var donations = await (from donation in _appDbContext.FoodDonations
                                       where donation.DonorId == donorId
                                       join foodItem in _appDbContext.FoodItems on donation.ItemId equals foodItem.Id
                                       select new FoodDonationDTO
                                       {
                                           DonorId = donorId,
                                           ItemId = foodItem.Id,
                                           Name = DonorName,
                                           Quantity = donation.Quantity,
                                           DateCooked = donation.DateCooked
                                       }).ToListAsync();

                return donations;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"An error occurred while retrieving donations for the donor: {ex}");

                // Rethrow the exception wrapped in a new exception with a more descriptive message
                throw new Exception("An error occurred while retrieving donations for the donor. See inner exception for details.", ex);
            }
        }

        public async Task<List<FoodItem>> DonorFood()
        {
            // Retrieve the donor ID from the claims
            var donorIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("DonorId");

            if (donorIdClaim == null)
            {
                throw new InvalidOperationException("User's donor ID not found in claims.");
            }

            var donorIdString = donorIdClaim.Value;

            if (!int.TryParse(donorIdString, out int donorId))
            {
                throw new InvalidOperationException("Unable to parse donor ID as integer.");
            }

            // Query the FoodDonations table for food items associated with the donor ID
            var itemId = await _appDbContext.FoodDonations
                .Where(fd => fd.DonorId == donorId)
                .Select(fd => fd.ItemId) // Assuming FoodItem is a navigation property or you need to select the items specifically
                .ToListAsync();

            var foodItems = await (from fi in _appDbContext.FoodItems
                                   join dr in _appDbContext.DonationRequests
                                   on fi.Id equals dr.RequestId
                                   where itemId.Contains(fi.Id)
                                         && dr.isCollected == false
                                   select fi)
                    .ToListAsync();

            return foodItems;
        }

        public IEnumerable<FoodDonationDTO> GetDonationsAsync()
        
        {
            var donations = (from donation in _appDbContext.FoodDonations
                             join donor in _appDbContext.Donors on donation.DonorId equals donor.DonorId
                             join foodItem in _appDbContext.FoodItems on donation.ItemId equals foodItem.Id
                             select new FoodDonationDTO
                             {
                                 DonorId = donor.DonorId,
                                 ItemId = foodItem.Id,
                                 Quantity = donation.Quantity,
                                 DateCooked = donation.DateCooked
                             }).ToList();

            return donations;
        }


       public async  Task <int> GetTotalDonationsCountAsync()
        {
            return await _appDbContext.FoodDonations.CountAsync();
        }


        public async Task<Dictionary<string, int>> GetMonthlyTotalFromWeeklyTotalsAsync()
        {
            // Step 1: Calculate Weekly Totals
            var weeklyTotals = await _appDbContext.FoodDonations
                .Select(d => new
                {
                    // Calculate the start of the week based on Monday
                    WeekStart = d.DateCooked.AddDays(-(int)d.DateCooked.DayOfWeek + (int)DayOfWeek.Monday).Date,
                    Month = d.DateCooked.Month,
                    Year = d.DateCooked.Year,
                    d.Quantity
                })
                .GroupBy(g => new { g.WeekStart, g.Month, g.Year })
                .Select(g => new
                {
                    g.Key.WeekStart,
                    g.Key.Month,
                    g.Key.Year,
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .ToListAsync();

            // Step 2: Aggregate Weekly Totals into Monthly Totals
            var monthlyTotals = weeklyTotals
                .GroupBy(w => new { w.Year, w.Month })
                .Select(g => new
                {
                    MonthYear = $"{g.Key.Year}-{g.Key.Month:D2}",
                    TotalQuantity = g.Sum(x => x.TotalQuantity)
                })
                .ToList();

            return monthlyTotals.ToDictionary(r => r.MonthYear, r => r.TotalQuantity);
        }

       /* public async Task<Dictionary<string, int>> GetMonthlyTotalFromWeeklyTotalsAsync()
        {
            // Retrieve data from the database
            var donations = await _appDbContext.FoodDonations
                .Select(d => new
                {
                    d.DateCooked,
                    d.Quantity
                })
                .ToListAsync();

            // Perform weekly and monthly calculations in memory
            var weeklyTotals = donations
                .GroupBy(d => new
                {
                    Year = d.DateCooked.Year,
                    Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                        d.DateCooked, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Week = g.Key.Week,
                    TotalQuantity = g.Sum(d => d.Quantity)
                })
                .ToList();

            var monthlyTotals = weeklyTotals
                .GroupBy(w => new
                {
                    Year = w.Year,
                    Month = new DateTime(w.Year, 1, 1).AddDays((w.Week - 1) * 7).Month
                })
                .Select(g => new
                {
                    MonthYear = $"{g.Key.Year}-{g.Key.Month:D2}",
                    TotalQuantity = g.Sum(w => w.TotalQuantity)
                })
                .ToList();

            return monthlyTotals.ToDictionary(r => r.MonthYear, r => r.TotalQuantity);
        }*/

        public async Task<Dictionary<string, int>> GetWeeklyTotalDonationsAsync()
        {
            var result = await _appDbContext.FoodDonations
                .Select(d => new
                {
                    Year = d.DateCooked.Year,
                    Week = (int)Math.Floor((d.DateCooked.DayOfYear - 1) / 7.0) + 1,
                    d.Quantity
                })
                .GroupBy(g => new { g.Year, g.Week })
                .Select(g => new
                {
                    YearWeek = $"{g.Key.Year}-W{g.Key.Week:D2}",
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .ToListAsync();

            return result.ToDictionary(r => r.YearWeek, r => r.TotalQuantity);
        }
       

        public async Task<Dictionary<string, int>> GetMonthlyTotalDonationsAsync()
        {
            var result = await _appDbContext.FoodDonations
                .GroupBy(d => new { Year = d.DateCooked.Year, Month = d.DateCooked.Month })
                .Select(g => new
                {
                    YearMonth = $"{g.Key.Year}-{g.Key.Month:D2}",
                    TotalQuantity = g.Sum(d => d.Quantity)
                })
                .ToListAsync();

            return result.ToDictionary(r => r.YearMonth, r => r.TotalQuantity);
        }
    }
    //Methods

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
