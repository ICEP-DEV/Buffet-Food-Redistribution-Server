using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAdmin
    {
        Task<RegistrationResponse> Register(Admin admin);
        Task <LoginResponse> LoginAdmin(AdminDTO admin);

    }
}
