using Microsoft.AspNetCore.SignalR;
using PollsApp.Application.DTOs;
using PollsApp.Domain;
using PollsApp.Mvc.ApiClient;
using PollsApp.Mvc.ViewModels;

namespace PollsApp.Mvc.Hubs
{
    public class CommentsHub : Hub
    {
        private readonly IWebApiClient webApiClient;

        public CommentsHub(IWebApiClient webApiClient)
        {
            this.webApiClient = webApiClient;
        }

        public async Task SendComment(string pollIdString, string text)
        {
            long pollId;
            try
            {
                string userId = Context.User.FindFirst("UserId").Value.ToString();
                var user = await webApiClient.GetUserDtoAsync(userId);
                pollId = long.Parse(pollIdString);
                
                CommentVM commentVm = new()
                {
                    PollId = pollIdString,
                    UserName = user.Name,
                    UserAvatar = user.Avatar is null ? "" : Convert.ToBase64String(user.Avatar),
                    Text = text
                };

                PostCommentModel model = new PostCommentModel() { Text = text, PollId = pollId, UserId = userId };
                await webApiClient.PostCommentAsync(model);

                await Clients.All.SendAsync("ReceiveComment", commentVm);
            }
            catch (Exception ex) { }

        }
    }
}
