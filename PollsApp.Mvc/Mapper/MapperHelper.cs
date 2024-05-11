using PollsApp.Application.DTOs;

namespace PollsApp.Mvc.Mapper
{
    public class MapperHelper
    {
        public static PostPollModel MapToPostPollModel(PollDetailsDto poll)
        {
            PostPollModel model = new();
            model.Title = poll.Poll.Title;
            model.Id = poll.Poll.Id;
            model.EndDate = poll.Poll.EndDate;
            foreach (var option in poll.Options)
            {
                model.PollOptions.Add(option.PollOption.Text);
            }
            model.IsActive = poll.Poll.IsActive;
            model.AllowComments = poll.Poll.AllowComments;
            return model;
        }
    }
}
