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
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

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

        public void UpdateDonationRequestStatus(int donationRequestId, string newStatus)
        {
            var donationRequest = _appDbContext.DonationRequests.Find(donationRequestId);
            if (donationRequest != null)
            {
                donationRequest.Status = newStatus;
                _appDbContext.SaveChanges();
            }
        }

        public async Task<int> GetDonorDonation(int itemId)
        {
           var donationId =  _appDbContext.FoodDonations.
                            Where(fd => fd.ItemId == itemId)
                            .Select(fd => fd.DonationId)
                             .FirstOrDefault();
                
            return donationId;    
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
