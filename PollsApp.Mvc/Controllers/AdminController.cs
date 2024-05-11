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
        public async Task<IActionResult> Create(string Title, bool AllowComments, bool HasEndDate, DateTime? EndDate, string[] options)
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
                model.PollOptions = options.ToList();

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
                var model = new CreatePollViewModel();
                model.Title = pollInfo.Poll.Title;
                model.IsActive = pollInfo.Poll.IsActive;
                model.AllowComments = pollInfo.Poll.AllowComments;
                model.Created = pollInfo.Poll.StartDate;
                model.EndDate = pollInfo.Poll.EndDate;
                if (pollInfo.Poll.EndDate != null)
                {
                    model.HasEndDate = true;
                }
                model.Options = pollInfo.Poll.PollOptions.Select(o => o.Text).ToList();

                return View(model);

            }
            return NotFound();

        }


        [Authorize(Roles = "admin")]
        [HttpPost("/admin/edit/{id:long}")]
        public async Task<IActionResult> Edit(int id, string Title, bool AllowComments, bool HasEndDate, DateTime? EndDate, string[] options)
        {
            if (ModelState.IsValid)
            {
                Poll poll = new();
                poll.Title = Title;
                poll.AllowComments = AllowComments;
                if (HasEndDate)
                {
                    poll.EndDate = EndDate;
                }
                foreach (string option in options)
                {
                    PollOption pollOption = new();
                    pollOption.Poll = null;
                    pollOption.PollId = poll.Id;
                    pollOption.Text = option;
                    poll.PollOptions.Add(pollOption);
                }

                PostPollModel model = new();
                model.Title = Title;
                model.Id = id;
                model.EndDate = poll.EndDate;
                foreach (string option in options)
                {
                    model.PollOptions.Add(option);
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



        [Authorize(Roles = "admin")]
        [HttpGet("/admin/getpolls")]
        public async Task<IActionResult> getPollsJson(string search, bool active, bool notvoted, int page = 1)
        {
            Console.WriteLine("adminGetPollsJson: " + notvoted);

            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();


            var polls = await webApiClient.GetPollsAsync(userIdString, search, active, notvoted, page);
            PollsViewModel model = new();
            model.PagedListModel = polls;

            return PartialView("_PollsListTable", model);
        }
    }
}
