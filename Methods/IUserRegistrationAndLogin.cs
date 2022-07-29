using Ecommerce_API.Models;

namespace Ecommerce_API.Methods
{
    public interface IUserRegistrationAndLogin
    {
        Task<NewUserRegistrationResponse> UserRegistration(NewUserRegistrationRequest userdetails);
        Task<LoginResponse> Login(LoginRequest logindetails);
    }
}