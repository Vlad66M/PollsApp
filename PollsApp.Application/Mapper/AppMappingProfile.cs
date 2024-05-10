using AutoMapper;
using PollsApp.Application.DTOs;
using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Vote, VoteDto>().ReverseMap();
            CreateMap<UserVote, UserVoteDto>().ReverseMap();
            CreateMap<PollOption, PollOptionDto>().ReverseMap();
            CreateMap<Poll, PollDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<PollDetails, PollDetailsDto>().ReverseMap();
            CreateMap<OptionDetails, OptionDetailsDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            
        }
    }
}
