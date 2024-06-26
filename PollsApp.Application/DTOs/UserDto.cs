﻿using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class UserDto
    {
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long RoleId { get; set; }
        public RoleDto Role { get; set; }
        public string? RoleName { get; set; }
        public byte[]? Avatar { get; set; }
        public List<VoteDto> Votes { get; set; } = new();
    }
}
