using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class PollVoteInfo
    {
        public long PollOptionId { get; set; }
        public bool IsAnon { get; set; }
    }
}
