using PollsApp.Application.DTOs;
using PollsApp.Application.Identity;

namespace PollsApp.Mvc.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> Authenticate(string email, string password);
        Task<bool> Register(RegistrationRequest registration);
        Task Logout();
    }
}
