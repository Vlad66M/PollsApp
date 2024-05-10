using PollsApp.Application.DTOs;
using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.Persistence
{
    public interface IPollsRepository
    {
        public PagedList<Poll> GetPolls(string? userId, string? search, bool? active, bool? notvoted, int page = 1);

        public DTOs.PollDetails GetPoll(long pollId, string userId);

        public void PostPoll(Poll poll);

        public void PutPoll(Poll poll);

        public void DeletePoll(long pollId);

        public void PostVote(Vote vote);

        public void PostComment(Comment comment);
    }
}
