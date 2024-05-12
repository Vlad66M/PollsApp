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
                PostPollOption postPollOption = new PostPollOption()
                {
                    Text = option.PollOption.Text,
                    Photo = option.PollOption.Photo is null ? null : Convert.ToBase64String(option.PollOption.Photo),
                    Audio = option.PollOption.Audio is null ? null : Convert.ToBase64String(option.PollOption.Audio)
                };
                model.PollOptions.Add(postPollOption);
            }
            model.IsActive = poll.Poll.IsActive;
            model.AllowComments = poll.Poll.AllowComments;
            return model;
        }
    }
}
