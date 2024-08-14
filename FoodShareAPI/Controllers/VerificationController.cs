using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FoodShareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : ControllerBase
    {
        private readonly IVerification? _verification;

        public VerificationController(IVerification? verification)
        {
            _verification = verification;
        }

        [HttpPost]
        public async Task<ActionResult<VerificationDTO>> VerifyOrganization(int regNo)
        {
            var verify = await _verification.VerifyOrganization(regNo);
            return Ok(verify);
        }
    }
}
