using PollsApp.Application.DTOs;
using PollsApp.Application;
using PollsApp.Domain;

namespace PollsApp.Mvc.ViewModels
{
    public class PollsViewModel
    {
        public UserDto User { get; set; }
        public PagedList<Poll> Polls { get; set; } = new();

        public PagedListModel PagedListModel { get; set; }
        public PollDetailsDto PollInfo { get; set; }

        public SearchInfo SearchInfo { get; set; }
        public int? Page { get; set; }

        PollVoteInfo PollVoteInfo { get; set; }
    }
}
