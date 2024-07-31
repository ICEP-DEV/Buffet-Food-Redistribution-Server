using Application.Contracts;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Infrastructure.Repo
{
    public class RequestRepo : IRequest
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestRepo(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._appDbContext = appDbContext;
            this._httpContextAccessor = httpContextAccessor;
        }
        public void CreateRequest(int foodDonationId)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    // Extract integer value from the claim
                    int recipientId = Convert.ToInt32(userIdClaim.Value); // or int.Parse(userIdClaim.Value)

                    // Fetch foodDonation from the database
                    var foodDonation = _appDbContext.FoodDonations.FirstOrDefault(fd => fd.DonationId == foodDonationId);

                    if (foodDonation != null)
                    {
                        DonationRequest request = new DonationRequest
                        {
                            DonationId = foodDonation.DonationId, // Ensure DonationId is properly fetched
                            RecipientId = recipientId,
                            Status = "pending"
                        };

                        _appDbContext.DonationRequests.Add(request);
                        _appDbContext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException($"Food donation with ID {foodDonationId} not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // For example:
               // _logger.LogError(ex, "An error occurred while processing the donation request.");

                // Optionally, you can rethrow the exception if needed
                // throw;
            }
        }

        public void UpdateDonationRequestStatus(int donationRequestId, string newStatus)
        {
            var donationRequest = _appDbContext.DonationRequests.Find(donationRequestId);
            if (donationRequest != null)
            {
                donationRequest.Status = newStatus;
                _appDbContext.SaveChanges();
            }
        }

        public Task<int> GetDonorDonation(int itemId) // changed to fix warning
        {
            var donationId = _appDbContext.FoodDonations
                             .Where(fd => fd.ItemId == itemId)
                             .Select(fd => fd.DonationId)
                             .FirstOrDefault();

            return Task.FromResult(donationId);
        }

        public async Task<List<DonationRequest>> GetDonorRequests()
        {
            try
            {
                // Retrieve the donor ID from the claims
                var donorIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("DonorId");

                if (donorIdClaim != null && int.TryParse(donorIdClaim.Value, out int donorId))
                {
                    // Find the donation associated with the donor
                    var donation = await _appDbContext.FoodDonations
                        .FirstOrDefaultAsync(d => d.DonorId == donorId);

                    if (donation != null)
                    {
                        // Fetch all requests associated with the donor's donation
                        var requests = await _appDbContext.DonationRequests
                            .Where(dr => dr.DonationId == donation.DonationId)
                            .ToListAsync();

                        // Log the number of records retrieved
                        Console.WriteLine($"Number of requests retrieved: {requests.Count}");

                        // Log details of each record
                        foreach (var request in requests)
                        {
                            Console.WriteLine($"Request ID: {request.RequestId}, DonationId: {request.DonationId}");
                        }

                        // Return the list of requests
                        return requests;
                    }
                }

                // Return an empty list if no matching donation or requests are found
                return new List<DonationRequest>();
            }
            catch (Exception ex)
            {
                // Log the exception (using Console.WriteLine here; consider using a logging framework in production)
                Console.WriteLine($"Exception occurred: {ex.Message}");
                throw new Exception("An error occurred while fetching donor requests.", ex);
            }
        }

        public async Task<int?> GetRequestId(int donationId)
        {
            /*var requestId = await _appDbContext.DonationRequests
                             .Where(dr => dr.DonationId == donationId)
                             .Select(dr => (int?)dr.RequestId) // Cast to int? for nullable return
                             .FirstOrDefaultAsync();
            return requestId;*/

            var requestId = await _appDbContext.DonationRequests.FirstOrDefaultAsync(d => d.DonationId == donationId);

            return requestId != null ? requestId.RequestId : (int?) null;
        }

        public async Task<int?> GetRecipientIdForDonationRequestAsync(int requestId)
        {
            var request = await _appDbContext.DonationRequests
                                             .FirstOrDefaultAsync(dr => dr.RequestId == requestId);

            // Check if request is not null, then return RecipientId, otherwise return null
            return request != null ? request.RecipientId : (int?)null;
        }

        public async Task<string> GetRequestStatus(int requestId)
        {
            var donationRequest = await _appDbContext.DonationRequests.FirstOrDefaultAsync(dr => dr.RequestId == requestId);

            // Check if donationRequest is null (no matching request found)
            if (donationRequest != null)
            {
                return donationRequest.Status;
            }
            else
            {
                return string.Empty; // Return empty string if no request with matching requestId is found
            }
        }

        public async Task<List<DonationRequest>> GetDonationRequstListAsync()
        {
            var result = await _appDbContext.DonationRequests.ToListAsync();
            return result;
        }

    }
}
