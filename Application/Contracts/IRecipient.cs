using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IRecipient
    {
        Task<RegistrationResponse> RegisterRecipientAsync(RecipientDTO recipientDTO);

        Task<LoginResponse> LoginRecipientAsync(RecipientsLoginDTO recipientLoginDTO);

        Task<Recipient> GetRecipientByEmail(string email);

        Task <List<Recipient>> GetRecipientListAsync();

        Task<int> UpdateRecipientAsync(int id, Recipient recipient);

        Task <int> DeleteRecipientAsync(int id);
        Task<Recipient> GetRecipientProfile();

        // Receive donation interface 

        
    }
}
