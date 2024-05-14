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
                if (model.CroppedAvatarBase64String != null)
                {
                    try
                    {
                        byte[] avatarBytes = Convert.FromBase64String(model.CroppedAvatarBase64String);
                        model.Avatar = avatarBytes;
                    }
                    catch { }
                }

                RegistrationRequest register = new()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Avatar = model.Avatar
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }


    }
}
