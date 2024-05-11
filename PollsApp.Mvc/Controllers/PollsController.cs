using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollsApp.Application.DTOs;
using PollsApp.Domain;
using PollsApp.Mvc.ApiClient;
using PollsApp.Mvc.ViewModels;

namespace PollsApp.Mvc.Controllers
{
    public class PollsController : Controller
    {
        IWebApiClient webApiClient;

        public PollsController(IWebApiClient webApiClient)
        {
            this.webApiClient = webApiClient;
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet("/polls")]
        public async Task<IActionResult> Index(string? search, bool? active, bool? notvoted, int? page)
        {
            Console.WriteLine("Index");
            string userIdString = "";
            try
            {
                userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();
            }
            catch { }

            UserDto user = await webApiClient.GetUserDtoAsync(userIdString);

            var polls = await webApiClient.GetPollsAsync(userIdString, search, active, notvoted, page);

            Console.WriteLine("HasNext: " + polls.hasNext);

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

        [Authorize(Roles = "admin, user")]
        [HttpGet("/polls/{id:long}")]
        public async Task<IActionResult> Poll(long id)
        {
            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            var pollInfo = await webApiClient.GetPollInfoAsync(id, userIdString);
            if (pollInfo != null)
            {
                UserDto user = await webApiClient.GetUserDtoAsync(userIdString);
                if (user != null)
                {
                    PollsViewModel model = new();
                    model.User = user;
                    model.PollInfo = pollInfo;
                    return View(model);
                }
            }

            return NotFound();
        }

        [Authorize(Roles = "admin, user")]
        [HttpPost("/polls/{id:long}")]
        public async Task<IActionResult> Poll(long id, long optionId, bool isAnon)
        {
            string userId = HttpContext.User.FindFirst("UserId").Value.ToString();
            

            Vote vote = new Vote();
            if (isAnon)
            {
                vote.IsAnon = true;
            }
            else
            {
                vote.IsAnon = false;
            }
            vote.UserId = userId;
            vote.PollOptionId = optionId;

            PostVoteModel model = new PostVoteModel() { UserId = userId, OptionId = optionId, IsAnon = isAnon };
            bool result = await webApiClient.PostVoteAsync(model);

            return RedirectToAction("Poll", new { id = id });

        }

        [Authorize(Roles = "admin, user")]
        [HttpGet("/polls/getpolls")]
        public async Task<IActionResult> getPollsJson(string search, bool active, bool notvoted, int page = 1)
        {
            Console.WriteLine("getPollsJson: " + notvoted);

            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            var polls = await webApiClient.GetPollsAsync(userIdString, search, active, notvoted, page);
            PollsViewModel model = new();
            model.PagedListModel = polls;

            return PartialView("_PollsList", model);
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet("/polls/getpollstitles")]
        public async Task<IActionResult> getPollsTitlesJson(string search, bool active, bool notvoted, int page = 1)
        {
            Console.WriteLine("getPollsJson: " + notvoted);

            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            var polls = await webApiClient.GetPollsAsync(userIdString, search, active, notvoted, page);

            return new JsonResult(polls.polls.Select(p => p.Title));
        }


        [Authorize(Roles = "admin, user")]
        [HttpGet("/polls_list")]
        public async Task<IActionResult> GetPollsList(string? search, bool? active, bool? notvoted, int? page)
        {
            Console.WriteLine("GetPollsList");
            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();

            var polls = await webApiClient.GetPollsAsync(userIdString, search, active, notvoted, page);
            PollsViewModel model = new();
            model.PagedListModel = polls;

            return PartialView(model);
        }




    }
}
