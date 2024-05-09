using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class PollsViewModel
    {
        public User User { get; set; }
        public PagedList<Poll> Polls { get; set; } = new();

        public PagedListModel PagedListModel { get; set; }
        public PollInfo PollInfo { get; set; }

        public SearchInfo SearchInfo { get; set; }
        public int? Page { get; set; }

        PollVoteInfo PollVoteInfo { get; set; }
    }
}
