using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Domain
{
    public class UserVote
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public bool IsAnon { get; set; }
    }
}
