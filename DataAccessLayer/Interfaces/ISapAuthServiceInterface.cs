using DTOs.LoginDtos;

namespace DataAccessLayer.Interfaces
{
    public interface ISapAuthServiceInterface
    {
        Task<string> LoginAsync(SapLoginRequest sapLoginRequest);
    }
}
