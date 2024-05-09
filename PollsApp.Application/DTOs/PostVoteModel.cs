using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class PostVoteModel
    {
        public string UserId { get; set; }
        public long OptionId { get; set; }
        public bool IsAnon { get; set; }
    }
}
