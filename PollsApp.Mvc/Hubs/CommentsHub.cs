using Microsoft.AspNetCore.SignalR;
using PollsApp.Application.DTOs;
using PollsApp.Domain;
using PollsApp.Mvc.ApiClient;

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
                string userIdString = Context.User.FindFirst("UserId").Value.ToString();
                string userId = userIdString;
                pollId = long.Parse(pollIdString);
                //WebApiClient webApiClient = new WebApiClient();
                Comment comment = new Comment();
                comment.PollId = pollId;
                comment.UserId = userId;
                comment.Text = text;
                comment.User = new User();

                PostCommentModel model = new PostCommentModel() { Text = text, PollId = pollId, UserId = userId };
                await webApiClient.PostCommentAsync(model);

                await Clients.Others.SendAsync("ReceiveComment", comment);
            }
            catch (Exception ex) { }

        }
    }
}
