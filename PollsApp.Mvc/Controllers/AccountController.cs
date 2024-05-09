using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PollsApp.Domain;
using PollsApp.Application.DTOs;
using PollsApp.Mvc.ApiClient;
using PollsApp.Application.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PollsApp.Mvc.Controllers
{
    

    public class AccountController : Controller
    {
        private readonly PollsApp.Mvc.Authentication.IAuthenticationService _authService;
        private readonly IWebApiClient webApiClient;


        public AccountController(PollsApp.Mvc.Authentication.IAuthenticationService authService, IWebApiClient webApiClient)
        {
            _authService = authService;
            this.webApiClient = webApiClient;


        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Email = model.Email;
                user.Name = model.Name;
                user.Password = model.Password;
                if (model.CroppedAvatarBase64String != null)
                {
                    try
                    {
                        byte[] avatarBytes = Convert.FromBase64String(model.CroppedAvatarBase64String);
                        user.Avatar = avatarBytes;
                    }
                    catch { }
                }
                user.Role = new Role() { Name = "user" };
                user.Votes = null;

                PostUserModel postModel = new();
                postModel.Name = user.Name;
                postModel.Email = user.Email;
                postModel.Password = user.Password;
                postModel.Avatar = user.Avatar;

                RegistrationRequest register = new()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Avatar = user.Avatar
                };
                var result = await _authService.Register(register);

                if (result)
                {
                    var isLoggedIn = await _authService.Authenticate(model.Email, model.Password);
                    if (isLoggedIn)
                    {
                        return RedirectToAction("Index", "Polls");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Не удалось зарегистрироваться");
                    return View(model);
                }

            }
            ModelState.AddModelError("", "Неверно заполнены поля");
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {

                var isLoggedIn = await _authService.Authenticate(model.Email, model.Password);
                if (isLoggedIn)
                {
                    return RedirectToAction("Index", "Polls");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректный логин или пароль");

                }

                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();
            User user = await webApiClient.GetUserAsync("api/users/" + userIdString);
            if (user == null)
            {
                return NotFound();
            }
            EditUserModel model = new EditUserModel() { Id = user.Id, Email = user.Email, Name = user.Name, Avatar = user.Avatar };
            if(user.Avatar != null)
            {
                model.CroppedAvatarBase64String = Convert.ToBase64String(model.Avatar);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = HttpContext.User.FindFirst("UserId").Value;
                if (!String.IsNullOrEmpty(model.CroppedAvatarBase64String))
                {
                    try
                    {
                        byte[] avatarBytes = Convert.FromBase64String(model.CroppedAvatarBase64String);
                        model.Avatar = avatarBytes;
                    }
                    catch { }
                }
                else
                {
                    model.Avatar = null;
                }
                
                var result = await webApiClient.PutUserAsync(model);

                if (result is not null)
                {
                    return RedirectToAction("Index", "Polls");
                }
                else
                {
                    ModelState.AddModelError("", "Не удалось сохранить изменения");
                    return View(model);
                }

            }
            ModelState.AddModelError("", "Неверно заполнены поля");
            return View(model);
        }

        /*public async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserName", user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }*/

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }


    }
}
