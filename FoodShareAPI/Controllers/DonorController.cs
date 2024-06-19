using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;

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

        public async Task<ActionResult<LoginResponse>> RegisterUser([FromBody] DonorDTO registerDTO)
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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateDonorAsync(Donor _donor)
        {

          var result = await donor.UpdateDonorAsync(_donor);

            if(result > 0)
            {
                return Ok(new { Message = "Donor profile updated successfully.", RowsAffected = result });
            }
            else
            {
                return NotFound(); // Donor with the specified ID not found
            }


            // return await donor.UpdateDonorAsync( _donor);
        }

        [HttpDelete]
        
        public async Task<int> DeleteDonorAsync(int id)
        {
            return await donor.DeleteDonorAsync(id);
        }

        [HttpGet ("Profile")]
        
        
        public async Task <ActionResult<Donor>> GetDonorProfile()
        {
            var donorProfile = await donor.GetDonorProfile();

            if (donorProfile != null)
            {
                return Ok(donorProfile); // Return 200 OK with the donor profile
            }
            else
            {
                return NotFound(); // Return 404 Not Found if donor profile not found
            }
        }
    }
}
