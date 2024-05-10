using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class UserVoteDto
    {

        public string UserId { get; set; }
        public string? UserName { get; set; }
        public bool IsAnon { get; set; }

    }
}
