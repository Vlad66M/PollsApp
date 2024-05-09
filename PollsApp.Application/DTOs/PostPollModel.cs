using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class PostPollModel
    {

        public long Id { get; set; }


        public string Title { get; set; }


        public DateTime? StartDate { get; set; }


        public DateTime? EndDate { get; set; }


        public bool IsActive { get; set; }


        public bool AllowComments { get; set; }


        public List<string> PollOptions { get; set; } = new();

    }
}
