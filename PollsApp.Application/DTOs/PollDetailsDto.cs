using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class PollDetailsDto
    {
        public PollDto Poll { get; set; }
        public bool IsVoted { get; set; }
        public bool IsAnon { get; set; }
        public List<OptionDetailsDto> Options { get; set; } = new();
        public int Votes { get; set; }
    }
}
