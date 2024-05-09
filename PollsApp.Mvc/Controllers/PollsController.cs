using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollsApp.Application.DTOs;
using PollsApp.Domain;
using PollsApp.Mvc.ApiClient;

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
            //string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();
            /*long userId = long.Parse(userIdString);*/

            User user = await webApiClient.GetUserAsync("api/users/" + userIdString);

            Console.WriteLine(user.Role.Name);

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
                User user = await webApiClient.GetUserAsync("api/users/" + userIdString);
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



        [Authorize(Roles = "admin")]
        [HttpGet("/polls/create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("/polls/create")]
        public async Task<IActionResult> Create(string Title, bool AllowComments, bool HasEndDate, DateTime? EndDate, string[] options)
        {
            if (ModelState.IsValid)
            {
                if (options == null || options.Length == 0)
                {
                    ModelState.AddModelError("", "Не заданы опции опроса");
                    return View();
                }

                Poll poll = new Poll();
                poll.Title = Title;
                poll.IsActive = true;
                poll.StartDate = DateTime.Now;
                if (HasEndDate)
                {
                    poll.EndDate = EndDate;
                }
                poll.AllowComments = AllowComments;

                poll.PollOptions = new List<PollOption>();
                foreach (string option in options)
                {
                    PollOption pollOption = new PollOption();
                    //pollOption.Poll = poll;
                    pollOption.Text = option;
                    poll.PollOptions.Add(pollOption);
                }

                PostPollModel model = new();
                model.Title = poll.Title;
                model.StartDate = poll.StartDate;
                model.EndDate = poll.EndDate;
                model.AllowComments = poll.AllowComments;
                model.IsActive = poll.IsActive;
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
        [HttpGet("/polls/edit/{id:long}")]
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
        [HttpPost("/polls/edit/{id:long}")]
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

                return RedirectToAction("Index", "Polls");
            }
            return View();
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet("/polls/getpolls")]
        public async Task<IActionResult> getPollsJson(string search, bool active, bool notvoted, int page = 1)
        {
            Console.WriteLine("getPollsJson: " + notvoted);

            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();
            //long userId = long.Parse(userIdString);


            var polls = await webApiClient.GetPollsAsync(userIdString, search, active, notvoted, page);
            PollsViewModel model = new();
            model.PagedListModel = polls;


            //return new JsonResult(polls);
            return PartialView("_PollsList", model);
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet("/polls/getpollstitles")]
        public async Task<IActionResult> getPollsTitlesJson(string search, bool active, bool notvoted, int page = 1)
        {
            Console.WriteLine("getPollsJson: " + notvoted);

            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();
            //long userId = long.Parse(userIdString);


            var polls = await webApiClient.GetPollsAsync(userIdString, search, active, notvoted, page);



            //return new JsonResult(polls);
            return new JsonResult(polls.polls.Select(p => p.Title));
        }


        [Authorize(Roles = "admin, user")]
        [HttpGet("/polls_list")]
        public async Task<IActionResult> GetPollsList(string? search, bool? active, bool? notvoted, int? page)
        {
            Console.WriteLine("GetPollsList");
            string userIdString = HttpContext.User.FindFirst("UserId").Value.ToString();
            //long userId = long.Parse(userIdString);



            var polls = await webApiClient.GetPollsAsync(userIdString, search, active, notvoted, page);
            PollsViewModel model = new();
            model.PagedListModel = polls;

            return PartialView(model);
        }




    }
}
