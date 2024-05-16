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

        public PollsController(IPollsRepository pollsRepository, IMapper mapper)
        {
            this.pollsRepository = pollsRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPolls(string? userId, string? search, bool? active, bool? notvoted, int? page)
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
        public async Task<IActionResult> PostPoll(PostPollModel model)
        {
            Console.WriteLine("PostPoll");

            Poll poll = MapperHelper.MapToPoll(model);

            pollsRepository.PostPoll(poll);

            return Ok(poll);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoll(int id, PostPollModel model)
        {
            Console.WriteLine("PutPoll");

            Poll poll = MapperHelper.MapToPoll(model);

            poll.Id = id;
            
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
