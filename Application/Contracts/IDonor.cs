using Application.DTOs;
using Domain.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDonor
    {
        Task<RegistrationResponse> RegisterDonorAsync(DonorDTO donorDTO);

        Task<LoginResponse> LoginDonorAsync(DonorLoginDTO donorLoginDTO);

        Task<Donor> GetDonorByEmail(string email);

        Task<List<Donor>> GetDonorListAsync();

        Task<int> UpdateDonorAsync( Donor donor);

        Task<int> DeleteDonorAsync(int id);

        Task<Donor> GetDonorProfile();
        Task<int> TotalDonors();

        //Task<IAction> GetCurrentUser(int id);
    }
}
