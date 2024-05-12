using PollsApp.Application.DTOs;
using PollsApp.Domain;

namespace PollsApp.Api
{
    public static class MapperHelper
    {
        public static Poll MapToPoll(PostPollModel model)
        {
            Poll poll = new();
            poll.Title = model.Title;
            poll.IsActive = model.IsActive;
            poll.AllowComments = model.AllowComments;
            poll.StartDate = model.StartDate;
            poll.EndDate = model.EndDate;
            foreach (var option in model.PollOptions)
            {
                PollOption o = new PollOption();
                o.Poll = poll;
                o.Text = option.Text;
                try
                {
                    o.Photo = Convert.FromBase64String(option.Photo);
                }
                catch { }
                try
                {
                    o.Audio = Convert.FromBase64String(option.Audio);
                }
                catch { }

                poll.PollOptions.Add(o);
            }
            return poll;
        }
        public static PollDetailsDto Map(PollDetails poll)
        {
            PollDetailsDto pollDetails = new PollDetailsDto()
            {
                IsAnon = poll.IsAnon,
                IsVoted = poll.IsVoted,
                Votes = poll.Votes
            };

            pollDetails.Poll = new()
            {
                AllowComments = poll.Poll.AllowComments,
                EndDate = poll.Poll.EndDate,
                StartDate = poll.Poll.StartDate,
                Id = poll.Poll.Id,
                IsActive = poll.Poll.IsActive,
                Title = poll.Poll.Title
            };

            foreach (var comment in poll.Poll.Comments)
            {
                var coomentDto = new CommentDto()
                {
                    Id = comment.Id,
                    PollId = comment.PollId,
                    Text = comment.Text,
                    UserId = comment.UserId,
                    User = new UserDto()
                    {
                        Id = comment.User.Id,
                        Avatar = comment.User.Avatar,
                        Name = comment.User.Name
                    }
                };
                pollDetails.Poll.Comments.Add(coomentDto);
            }

            pollDetails.Options = new();
            foreach (var option in poll.Options)
            {
                var oDto = new OptionDetailsDto()
                {
                    IsChecked = option.IsChecked,
                    Votes = option.Votes,
                    PollOption = new PollOptionDto()
                    {
                        Id = option.PollOption.Id,
                        Text = option.PollOption.Text,
                        Photo = option.PollOption.Photo,
                        Audio = option.PollOption.Audio,
                        PollId = option.PollOption.Id
                        //Votes = mapper.Map<Vote[], List<VoteDto>>(option.PollOption.Votes.ToArray())
                    }
                };
                foreach (var vote in option.Users)
                {
                    var voter = new UserDto()
                    {
                        Id = vote.Id,
                        Avatar = vote.Avatar,
                        Name = vote.Name
                    };
                    oDto.Users.Add(voter);
                };
                pollDetails.Options.Add(oDto);
            }
            return pollDetails;
        }
    }
}
