using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application
{
    public class PollDetails
    {
        public Poll Poll { get; set; }
        public bool IsVoted { get; set; }
        public bool IsAnon { get; set; }
        public List<OptionDetails> Options { get; set; } = new();
        public int Votes { get; set; }
    }
}
