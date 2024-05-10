using PollsApp.Application;
using PollsApp.Application.DTOs;
using PollsApp.Application.Identity;
using PollsApp.Domain;

namespace PollsApp.Mvc.ApiClient
{
    public interface IWebApiClient
    {
        Task<bool> DeletePollAsync(long id);
        Task<Application.DTOs.PollDetailsDto> GetPollInfoAsync(long pollId, string userId);
        Task<PagedListModel> GetPollsAsync(string? userId, string? search, bool? active, bool? notvoted, int? page);
        Task<User> GetUserAsync(string path);
        Task<UserDto> GetUserDtoAsync(string path);
        Task<AuthResponse> LoginAsync(AuthRequest request);
        Task<bool> PostCommentAsync(PostCommentModel comment);
        Task<bool> PostPollAsync(PostPollModel model);
        Task<User> PutUserAsync(EditUserModel model);
        Task<bool> PostVoteAsync(PostVoteModel vote);
        Task<bool> PutPollAsync(PostPollModel model);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
    }
}