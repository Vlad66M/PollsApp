using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class PostCommentModel
    {
        public string UserId { get; set; }
        public long PollId { get; set; }
        public string Text { get; set; }
    }
}
