﻿using AutoMapper;
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
    public class CommentsController : Controller
    {

        private IPollsRepository pollsRepository;
        private IUsersRepository usersRepository;
        private readonly IMapper mapper;

        public CommentsController(IPollsRepository pollsRepository,
                               IUsersRepository usersRepository,
                               IMapper mapper)
        {
            this.pollsRepository = pollsRepository;
            this.usersRepository = usersRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(PostCommentModel model)
        {
            Console.WriteLine("PostComment");

            Comment comment = new Comment();
            comment.PollId = model.PollId;
            comment.UserId = model.UserId;
            comment.Text = model.Text;

            pollsRepository.PostComment(comment);
            return Ok(comment);

        }
    }
}
