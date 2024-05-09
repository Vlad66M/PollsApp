using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class CommentDto
    {
        
        public long Id { get; set; }

        
        public long PollId { get; set; }

        
        public long UserId { get; set; }

        
        public string Text { get; set; }

        
        public string UserName { get; set; }

        
    }
}
