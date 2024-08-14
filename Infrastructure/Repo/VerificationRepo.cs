using Application.Contracts;
using Application.DTOs;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
    public class VerificationRepo : IVerification
    {

        private readonly AppDbContext _appDbContext;

        public VerificationRepo(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
            


        public async Task<VerificationDTO> VerifyOrganization(int regno)
        {
            var organization = await _appDbContext.Organizations
      .FirstOrDefaultAsync(r => r.Regno == regno);

            if (organization == null)
            {
                return new VerificationDTO(false, "Organization does not exist");
            }
            else
            {
                return new VerificationDTO(true, "Verification successful");
            }
        }
    }
}
