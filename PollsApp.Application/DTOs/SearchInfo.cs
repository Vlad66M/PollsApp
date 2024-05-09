using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class SearchInfo
    {
        public string? Search { get; set; }
        public bool? Active { get; set; }
        public bool? NotVoted { get; set; }
    }
}
