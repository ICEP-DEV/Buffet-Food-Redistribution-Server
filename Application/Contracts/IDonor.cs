using Application.DTOs;
using Domain.Entities;
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

        Task<int> UpdateDonorAsync(int id, Donor donor);

        Task<int> DeleteDonorAsync(int id);

    }
}
