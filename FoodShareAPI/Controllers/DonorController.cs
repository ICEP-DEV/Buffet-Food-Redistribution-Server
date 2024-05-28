using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IDonor donor;
        public DonorController(IDonor donor)
        {

            this.donor = donor;
        }

        [HttpPost("Login")]

        public async Task<ActionResult<LoginResponse>> LogUserIn(DonorLoginDTO loginDTO)
        {
            var result = await donor.LoginDonorAsync(loginDTO);
            return Ok(result);
        }


        [HttpPost("register")]

        public async Task<ActionResult<LoginResponse>> RegisterUser(DonorDTO registerDTO)
        {
            var result = await donor.RegisterDonorAsync(registerDTO);
            return Ok(result);
        }

        [HttpGet("AllDonors")]

        public async Task<List<Donor>> GetDonorListAsync()
        {
            return await donor.GetDonorListAsync();
        }

        [HttpGet("{email}")]

        public Task<Donor> GetDonorByEmail(string email)
        {
            return donor.GetDonorByEmail(email);
        }

        [HttpPut("{id}")]
        public async Task<int> UpdateDonorAsync(int id, Donor _donor)
        {
            return await donor.UpdateDonorAsync(id, _donor);
        }

        [HttpDelete("{id}")]

        public async Task<int> DeleteDonorAsync(int id)
        {
            return await donor.DeleteDonorAsync(id);
        }
    }
}
