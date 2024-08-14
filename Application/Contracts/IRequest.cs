using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IRequest
    {
       void CreateRequest(int foodDonationId);
        void UpdateDonationRequestStatus(int donationRequestId, string newStatus);

        Task<List<DonationRequest>> GetDonationRequstListAsync();

        Task<int> GetDonorDonation(int itemId);
        Task<int?> GetRequestId(int donationId);

        Task<int?> GetRecipientIdForDonationRequestAsync(int requestId);
        Task<string> GetRequestStatus(int requestId);

        Task<List<DonationRequest>> GetDonorRequests();

        Task<List<DonationRequest>> GetAcceptedHistory();

        Task<List<DonationRequest>> GetDeclinedHistory();

        Task<List<DonationRequest>> GetPendingHistory();

    }
}
