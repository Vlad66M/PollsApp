using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using PollsApp.Application.Identity;
using PollsApp.Mvc.LocalStorageServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PollsApp.Mvc.ApiClient;
using PollsApp.Application.DTOs;

namespace PollsApp.Mvc.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebApiClient _client;
        private JwtSecurityTokenHandler _tokenHandler;

        public AuthenticationService(ILocalStorageService localStorage, IHttpContextAccessor httpContextAccessor, IWebApiClient webApiClient)
        {
            this._localStorage = localStorage;
            this._httpContextAccessor = httpContextAccessor;
            this._client = webApiClient;
            this._tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                //WebApiClient _client = new();
                AuthRequest authenticationRequest = new() { Email = email, Password = password };
                var authenticationResponse = await _client.LoginAsync(authenticationRequest);

                if (authenticationResponse.Token != string.Empty)
                {
                    //Get Claims from token and Build auth user object
                    var tokenContent = _tokenHandler.ReadJwtToken(authenticationResponse.Token);
                    var claims = ParseClaims(tokenContent);
                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                    var login = _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                    _localStorage.SetStorageValue("token", authenticationResponse.Token);

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Register(RegistrationRequest registration)
        {
            //WebApiClient _client = new();

            var response = await _client.RegisterAsync(registration);

            if (!string.IsNullOrEmpty(response.UserId))
            {
                await Authenticate(registration.Email, registration.Password);
                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            _localStorage.ClearStorage(new List<string> { "token" });
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private IList<Claim> ParseClaims(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
    }
}
