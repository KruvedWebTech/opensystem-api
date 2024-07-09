using opensystem_api.Models;

namespace opensystem_api.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        Task<ResponseResult<string>> AuthenticateAsync(string email, string password); // Define the async authentication method
        Task<ResponseResult<string>> ForgotPassword(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<ResponseResult<string>> CreateUserAsync(CreateUserRequest request);
        Task<ResponseResult<string>> DeleteUserAsync(int id);
    }
}
