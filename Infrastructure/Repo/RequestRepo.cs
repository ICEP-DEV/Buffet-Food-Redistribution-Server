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
                            Status = "pending",
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
            catch (Exception )
            {
               
            }
        }

        public void UpdateDonationRequestStatus(int donationRequestId, string newStatus)
        {
            // Retrieve the donation request
            var donationRequest = _appDbContext.DonationRequests
                .FirstOrDefault(dr => dr.RequestId == donationRequestId);

            if (donationRequest != null)
            {
                // Update the status of the donation request
                donationRequest.Status = newStatus;
                _appDbContext.SaveChanges();

                // If the new status is "Accepted", update the related FoodItems
                if (newStatus == "Accepted")
                {
                    // Find all related FoodItems (assuming you have a relationship or a way to get related items)
                    var foodItems = _appDbContext.FoodItems
                        .Where(fi => fi.Id == donationRequestId) // Adjust this as necessary
                        .ToList();

                    foreach (var foodItem in foodItems)
                    {
                        // Update the isRequested attribute
                        foodItem.IsRequested = true;
                    }

                    // Save changes for the updated FoodItems
                    _appDbContext.SaveChanges();
                }
            }
        }

        public void UpdateCollectionStatus(int donationRequestId)
        {
            var donationRequest = _appDbContext.DonationRequests
                .FirstOrDefault(dr => dr.RequestId == donationRequestId);

            if (donationRequest != null)
            {
                donationRequest.isCollected = true;
                _appDbContext.SaveChanges(true);
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
                var donorIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("DonorId");

                if (donorIdClaim != null && int.TryParse(donorIdClaim.Value, out int donorId))
                {
                    var donation = await _appDbContext.FoodDonations
                        .FirstOrDefaultAsync(d => d.DonorId == donorId);

                    if (donation != null)
                    {
                        var requests = await _appDbContext.DonationRequests
                            .Where(dr => dr.DonationId == donation.DonationId)
                            .ToListAsync();

                        Console.WriteLine($"Number of requests retrieved: {requests.Count}");

                        foreach (var request in requests)
                        {
                            Console.WriteLine($"Request ID: {request.RequestId}, DonationId: {request.DonationId}");
                        }
                        return requests;
                    }
                }
                return new List<DonationRequest>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                throw new Exception("An error occurred while fetching donor requests.", ex);
            }
        }

        public async Task<int?> GetRequestId(int donationId)
        { 
            var requestId = await _appDbContext.DonationRequests.FirstOrDefaultAsync(d => d.DonationId == donationId);

            return requestId != null ? requestId.RequestId : (int?) null;
        }

        public async Task<int?> GetRecipientIdForDonationRequestAsync(int requestId)
        {
            var request = await _appDbContext.DonationRequests
                                             .FirstOrDefaultAsync(dr => dr.RequestId == requestId);
            return request != null ? request.RecipientId : (int?)null;
        }

        public async Task<string> GetRequestStatus(int requestId)
        {
            var donationRequest = await _appDbContext.DonationRequests.FirstOrDefaultAsync(dr => dr.RequestId == requestId);

            if (donationRequest != null)
            {
                return donationRequest.Status;
            }
            else
            {
                return string.Empty; 
            }
        }

        public async Task<List<DonationRequest>> GetDonationRequstListAsync()
        {
            var result = await _appDbContext.DonationRequests.ToListAsync();

            return result;
        }

        public async Task<List<DonationRequest>> GetAcceptedHistory()
        {
            var acceptedDonations = await _appDbContext.DonationRequests
                                                .Where(dr => dr.Status == "Accepted")
                                                .ToListAsync();
            
            return acceptedDonations;
        }

        public async Task<List<DonationRequest>> GetDeclinedHistory()
        {
            //int recipientId;


            var declinedDonations = await _appDbContext.DonationRequests
                                                .Where(dr => dr.Status == "Declined")
                                                .ToListAsync();

            return declinedDonations;
        }

        public async Task<List<DonationRequest>> GetPendingHistory()
        {
            var pendingDonations = await _appDbContext.DonationRequests
                                                .Where(dr => dr.Status == "Pending")
                                                .ToListAsync();

            return pendingDonations;
        }
    }
}
