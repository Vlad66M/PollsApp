using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PollsApp.Application.Identity;
using PollsApp.Identity;

namespace PollsApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAuthService _authenticationService;
        public AccountController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
        {
            Console.WriteLine("Register");
            //var authServise = new AuthService();
            AuthResponse result;
            try
            {
                result = await _authenticationService.Login(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Unauthorized");
            }
            

        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            Console.WriteLine("Register");
            //var authServise = new AuthService();
            var result = await _authenticationService.Register(request);
            return Ok(result);
        }
    }
}
