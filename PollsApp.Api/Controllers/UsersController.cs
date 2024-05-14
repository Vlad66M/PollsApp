using AutoMapper;
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
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersRepository usersRepository;
        private readonly IPollsRepository pollsRepository;
        private readonly IMapper mapper;

        public UsersController(IUsersRepository usersRepository, IPollsRepository pollsRepository, IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.pollsRepository = pollsRepository;
            this.mapper = mapper;
        }
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = usersRepository.GetUserById(id);
            if (user != null)
            {
                UserDto userDto = mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutUser(EditUserModel model)
        {
            Console.WriteLine("PutUser");
           
            User user = new User { Id=model.Id, Email = model.Email, Name = model.Name, Avatar = model.Avatar };
            try
            {
                usersRepository.PutUser(user);
            }
            catch (Exception ex) 
            {
                return BadRequest("Не удалось сохранить изменения");
            }
            return Ok(user);
        }


    }
}
