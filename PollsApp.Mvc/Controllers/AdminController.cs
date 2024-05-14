using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollsApp.Application.DTOs;
using PollsApp.Domain;
using PollsApp.Mvc.ApiClient;
using PollsApp.Mvc.Mapper;
using PollsApp.Mvc.ViewModels;

namespace PollsApp.Mvc.Controllers
{
    public class AdminController : Controller
    {
        IWebApiClient webApiClient;

        public AdminController(IWebApiClient webApiClient)
        {
            this.webApiClient = webApiClient;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("/admin")]
        public async Task<IActionResult> Index(string? search, bool? active, bool? notvoted, int? page)
        {
            Console.WriteLine("Index");
            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            UserDto user = await webApiClient.GetUserDtoAsync(userIdString);
            var polls = await webApiClient.GetPollsAsync(userIdString, search, active, notvoted, page);

            if (user != null)
            {
                PollsViewModel model = new PollsViewModel();
                model.PagedListModel = polls;
                model.User = user;
                model.SearchInfo = new SearchInfo();
                model.SearchInfo.Search = search;
                model.SearchInfo.Active = active;
                model.SearchInfo.NotVoted = notvoted;
                model.Page = page;
                return View(model);
            }

            return NotFound();

        }

        [Authorize(Roles = "admin")]
        [HttpGet("/admin/create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("/admin/create")]
        public async Task<IActionResult> Create(string Title, bool AllowComments, bool HasEndDate, DateTime? EndDate, string[] options, string[] audios, string[] photos)
        {
            if (ModelState.IsValid)
            {
                if (options == null || options.Length == 0)
                {
                    ModelState.AddModelError("", "Не заданы опции опроса");
                    return View();
                }

                PostPollModel model = new();
                model.Title = Title;
                model.StartDate = DateTime.Now;
                if (HasEndDate)
                {
                    model.EndDate = EndDate;
                }
                model.AllowComments = AllowComments;
                model.IsActive = true;

                for(int i =0;i< options.Length; i++)
                {
                    PostPollOption option = new()
                    {
                        Text = options[i],
                        Photo = photos[i],
                        Audio = audios[i]
                    };
                    model.PollOptions.Add(option);
                }

                if (HasEmptyOption(model.PollOptions))
                {
                    ModelState.AddModelError("", "Опция не может быть пустой");
                    return View();
                }

                bool result = await webApiClient.PostPollAsync(model);

                if (result)
                {
                    return RedirectToAction("Index", "Polls");
                }
                else
                {
                    ModelState.AddModelError("", "Не удалось добавить опрос");
                    return View();
                }

            }
            else
            {
                if (String.IsNullOrEmpty(Title))
                {
                    ModelState.AddModelError("", "Заголовок не может быть пустым");
                }

                return View();
            }

        }


        [Authorize(Roles = "admin")]
        [HttpGet("/admin/edit/{id:long}")]
        public async Task<IActionResult> Edit(long id)
        {
            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            var pollInfo = await webApiClient.GetPollInfoAsync(id, userIdString);
            if (pollInfo != null)
            {
                var model = new EditPollViewModel();
                model.Title = pollInfo.Poll.Title;
                model.IsActive = pollInfo.Poll.IsActive;
                model.AllowComments = pollInfo.Poll.AllowComments;
                model.EndDate = pollInfo.Poll.EndDate;
                if (pollInfo.Poll.EndDate != null)
                {
                    model.HasEndDate = true;
                }
                foreach(var o in pollInfo.Options)
                {
                    var option = new PostPollOption()
                    {
                        Text = o.PollOption.Text,
                        Photo = o.PollOption.Photo is null ? "" : Convert.ToBase64String(o.PollOption.Photo),
                        Audio = o.PollOption.Audio is null? "" : Convert.ToBase64String(o.PollOption.Audio)
                    };
                    model.PollOptions.Add(option);
                }

                return View(model);

            }
            return NotFound();

        }


        [Authorize(Roles = "admin")]
        [HttpPost("/admin/edit/{id:long}")]
        public async Task<IActionResult> Edit(int id, string Title, bool AllowComments, bool HasEndDate, DateTime? EndDate, string[] options, string[] audios, string[] photos)
        {
            if (ModelState.IsValid)
            {
                PostPollModel model = new();
                model.Id = id;
                model.Title = Title;
                if (HasEndDate)
                {
                    model.EndDate = EndDate;
                }
                model.AllowComments = AllowComments;
                model.IsActive = true;

                for (int i = 0; i < options.Length; i++)
                {
                    PostPollOption option = new()
                    {
                        Text = options[i],
                        Photo = photos[i],
                        Audio = audios[i]
                    };
                    model.PollOptions.Add(option);
                }

                if (HasEmptyOption(model.PollOptions))
                {
                    ModelState.AddModelError("", "Опция не может быть пустой");
                    return View();
                }

                await webApiClient.PutPollAsync(model);

                return RedirectToAction("Index", "Admin");
            }
            return View();
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Finish([FromForm] string id)
        {
            long pollId = long.Parse(id);
            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            var p = await webApiClient.GetPollInfoAsync(pollId, userIdString);

            PostPollModel model = MapperHelper.MapToPostPollModel(p);
            model.IsActive = false;

            await webApiClient.PutPollAsync(model);

            var currentPage = Request.Headers.Referer.ToString();
            return Redirect(currentPage);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Restart(string id)
        {
            long pollId = long.Parse(id);
            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            var p = await webApiClient.GetPollInfoAsync(pollId, userIdString);

            PostPollModel model = MapperHelper.MapToPostPollModel(p);
            model.IsActive = true;
            
            await webApiClient.PutPollAsync(model);

            var currentPage = Request.Headers.Referer.ToString();
            return Redirect(currentPage);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ForbidComments([FromForm] string id)
        {
            long pollId = long.Parse(id);
            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            var p = await webApiClient.GetPollInfoAsync(pollId, userIdString);

            PostPollModel model = MapperHelper.MapToPostPollModel(p);
            model.AllowComments = false;

            await webApiClient.PutPollAsync(model);

            var currentPage = Request.Headers.Referer.ToString();
            return Redirect(currentPage);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AllowComments([FromForm] string id)
        {
            long pollId = long.Parse(id);
            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            var p = await webApiClient.GetPollInfoAsync(pollId, userIdString);

            PostPollModel model = MapperHelper.MapToPostPollModel(p);
            model.AllowComments = true;

            await webApiClient.PutPollAsync(model);

            var currentPage = Request.Headers.Referer.ToString();
            return Redirect(currentPage);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            long pollId = long.Parse(id);

            await webApiClient.DeletePollAsync(pollId);

            var currentPage = Request.Headers.Referer.ToString();
            return Redirect(currentPage);
        }

        private bool HasEmptyOption(List<PostPollOption> options)
        {
            foreach (var item in options)
            {
                if(String.IsNullOrEmpty(item.Text)
                   && String.IsNullOrEmpty(item.Photo)
                   && String.IsNullOrEmpty(item.Audio))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
