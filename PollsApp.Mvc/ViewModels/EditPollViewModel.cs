using PollsApp.Application.DTOs;

namespace PollsApp.Mvc.ViewModels
{
    public class EditPollViewModel
    {
        public long Id { get; set; }


        public string Title { get; set; }

        public bool HasEndDate {  get; set; }


        public DateTime? StartDate { get; set; }


        public DateTime? EndDate { get; set; }


        public bool IsActive { get; set; }


        public bool AllowComments { get; set; }


        public List<PostPollOption> PollOptions { get; set; } = new();
    }
}
