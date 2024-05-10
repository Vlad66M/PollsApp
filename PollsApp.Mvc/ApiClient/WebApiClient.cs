using Newtonsoft.Json;
using PollsApp.Application.DTOs;
using PollsApp.Application;
using PollsApp.Domain;
using PollsApp.Application.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Hanssens.Net;
using PollsApp.Mvc.LocalStorageServices;
using System.Net.Http.Headers;

namespace PollsApp.Mvc.ApiClient
{
    public class WebApiClient : IWebApiClient
    {

        private readonly ILocalStorageService _localStorageService;

        public WebApiClient(ILocalStorageService localStorageService)
        {
            this._localStorageService = localStorageService;
        }
        public WebApiClient()
        {
            
        }

        private HttpClient GetHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:7222/");
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _localStorageService.GetStorageValue<string>("token"));
            }
            catch { }
            
            return httpClient;
        }

        public async Task<User> GetUserAsync(string path)
        {
            User user = null;
            HttpResponseMessage response = await GetHttpClient().GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User>();
            }
            return user;
        }

        public async Task<UserDto> GetUserDtoAsync(string path)
        {
            UserDto user = null;
            HttpResponseMessage response = await GetHttpClient().GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<UserDto>();
            }
            return user;
        }

        public async Task<User> PutUserAsync(EditUserModel model)
        {
            HttpResponseMessage response = await GetHttpClient().PutAsJsonAsync($"api/users", model);
            if (response.IsSuccessStatusCode)
            {
                User user = await response.Content.ReadAsAsync<User>();
                return user;
            }
            return null;
        }

        public async Task<PagedListModel> GetPollsAsync(string? userId, string? search, bool? active, bool? notvoted, int? page)
        {
            PagedListModel polls = new PagedListModel();
            List<Poll> polls1 = new List<Poll>();
            bool res = false;
            string path = $"/api/polls?userId={userId}&search={search}&active={active}&notvoted={notvoted}&page={page}";
            HttpResponseMessage response = await GetHttpClient().GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var resStr = await response.Content.ReadAsStringAsync();
                polls = await Task.Run(() => JsonConvert.DeserializeObject<PagedListModel>(resStr));
            }
            return polls;
        }

        public async Task<Application.DTOs.PollDetailsDto> GetPollInfoAsync(long pollId, string userId)
        {
            Application.DTOs.PollDetailsDto pollInfo = null;
            string path = $"/api/polls/{pollId}?userId={userId}";
            HttpResponseMessage response = await GetHttpClient().GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                pollInfo = await response.Content.ReadAsAsync<Application.DTOs.PollDetailsDto>();
            }
            return pollInfo;
        }

        public async Task<bool> PostPollAsync(PostPollModel model)
        {
            HttpResponseMessage response = await GetHttpClient().PostAsJsonAsync($"api/polls", model);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PutPollAsync(PostPollModel model)
        {
            HttpResponseMessage response = await GetHttpClient().PutAsJsonAsync($"api/polls/{model.Id}", model);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeletePollAsync(long id)
        {
            HttpResponseMessage response = await GetHttpClient().DeleteAsync($"api/polls/" + id);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PostVoteAsync(PostVoteModel vote)
        {
            HttpResponseMessage response = await GetHttpClient().PostAsJsonAsync($"api/votes", vote);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PostCommentAsync(PostCommentModel comment)
        {
            HttpResponseMessage response = await GetHttpClient().PostAsJsonAsync($"api/comments", comment);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<AuthResponse> LoginAsync(AuthRequest request)
        {
            AuthResponse res = new();
            HttpResponseMessage response = await GetHttpClient().PostAsJsonAsync($"api/account/login", request);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadAsAsync<AuthResponse>();
            }
            return res;
        }

        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            RegistrationResponse res = new();
            HttpResponseMessage response = await GetHttpClient().PostAsJsonAsync($"api/account/register", request);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadAsAsync<RegistrationResponse>();
            }
            return res;
        }

        
    }
}
