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
    public class VoteDto
    {
       
        public long Id { get; set; }

        
        public long PollOptionId { get; set; }

        
        public string UserId { get; set; }

        
        public bool IsAnon { get; set; }

        
        public PollOptionDto PollOption { get; set; }

        
        public UserDto User { get; set; }
    }
}
