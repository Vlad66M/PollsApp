using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PollsApp.Application.DTOs;
using PollsApp.Application.Persistence;
using PollsApp.Domain;

namespace PollsApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin, user")]
    public class VotesController : Controller
    {
        private IPollsRepository pollsRepository;
        private IUsersRepository usersRepository;

        public VotesController(IPollsRepository pollsRepository,
                               IUsersRepository usersRepository)
        {
            this.pollsRepository = pollsRepository;
            this.usersRepository = usersRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Vote>> PostVote(PostVoteModel model)
        {
            Console.WriteLine("PostVote");
            Vote vote = new Vote() { UserId = model.UserId, PollOptionId = model.OptionId, IsAnon = model.IsAnon };
            pollsRepository.PostVote(vote);
            return Ok(vote);
        }
    }
}
