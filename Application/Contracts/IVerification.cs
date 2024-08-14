using Application.DTOs;


namespace Application.Contracts
{
    public interface IVerification
    {
        Task<VerificationDTO> VerifyOrganization(int regno);
    }
}
