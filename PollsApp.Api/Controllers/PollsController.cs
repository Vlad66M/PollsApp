using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PollsApp.Application;
using PollsApp.Application.DTOs;
using PollsApp.Application.Persistence;
using PollsApp.Domain;
using PollsApp.Identity;
using System.Dynamic;

namespace PollsApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin, user")]
    public class PollsController : Controller
    {
        private readonly IMapper mapper;
        private IPollsRepository pollsRepository { get; }
        private IUsersRepository usersRepository { get; }
        private IUserService userService { get; }

        public PollsController(IPollsRepository pollsRepository, 
                               IUsersRepository usersRepository, 
                               IUserService userService,
                               IMapper mapper)
        {
            this.pollsRepository = pollsRepository;
            this.usersRepository = usersRepository;
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetPolls(string? userId, string? search, bool? active, bool? notvoted, int? page)
        {
            Console.WriteLine("GetPolls");
            int pageNumber = page ?? 1;
            var polls = pollsRepository.GetPolls(userId, search, active, notvoted, pageNumber);
            PagedListModel pagedListModel = new PagedListModel();

            List<PollDto> pollsDtos = new List<PollDto>();
            foreach(Poll pollDto in polls.data)
            {
                var pDto = mapper.Map<PollDto>(pollDto);
                pollsDtos.Add(pDto);
            }
            
            pagedListModel.hasNext = polls.hasNext;
            pagedListModel.hasPrevious = polls.hasPrevious;
            pagedListModel.polls = pollsDtos;


            string res = "";
            try
            {
                res = JsonConvert.SerializeObject(pagedListModel, Formatting.Indented,
new JsonSerializerSettings
{
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
});
            }
            catch (Exception ex)
            {

            }

            return Ok(res);
        }

        [HttpGet("{pollId}")]
        public async Task<ActionResult<IEnumerable<PollDetailsDto>>> GetPoll(long pollId, string userId)
        {
            var poll = pollsRepository.GetPoll(pollId, userId);
            
            PollDetailsDto pollDetails = MapperHelper.Map(poll);
            
            return Ok(pollDetails);
        }

        [HttpPost]
        public async Task<ActionResult<Poll>> PostPoll(PostPollModel model)
        {
            Console.WriteLine("PostPoll");

            Poll poll = new();
            poll.Title = model.Title;
            poll.IsActive = model.IsActive;
            poll.AllowComments = model.AllowComments;
            poll.StartDate = model.StartDate;
            poll.EndDate = model.EndDate;
            foreach (string option in model.PollOptions)
            {
                PollOption o = new PollOption();
                o.Poll = poll;
                o.Text = option;
                poll.PollOptions.Add(o);
            }

            pollsRepository.PostPoll(poll);
            return Ok(poll);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoll(int id, PostPollModel model)
        {
            Poll poll = new();
            poll.Id = id;
            poll.Title = model.Title;
            poll.IsActive = model.IsActive;
            poll.AllowComments = model.AllowComments;
            poll.EndDate = model.EndDate;
            foreach (string option in model.PollOptions)
            {
                PollOption o = new PollOption();
                o.Poll = poll;
                o.Text = option;
                poll.PollOptions.Add(o);
            }
            Console.WriteLine("PutPoll");
            if (poll == null)
            {
                return BadRequest();
            }
            pollsRepository.PutPoll(poll);
            return Ok(poll);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoll(long id)
        {
            pollsRepository.DeletePoll(id);
            return Ok();

        }
    }
}
