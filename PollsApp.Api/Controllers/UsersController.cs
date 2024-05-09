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

        public UsersController(IUsersRepository usersRepository, IPollsRepository pollsRepository)
        {
            this.usersRepository = usersRepository;
            this.pollsRepository = pollsRepository;
        }
        // GET: api/Users
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string email, string password)
        {
            var user = usersRepository.GetUserByEmailAndPassword(email, password);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
            //return new JsonResult(user);
        }*/

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser(string id)
        {
            var user = usersRepository.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<User>> PutUser(EditUserModel model)
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
