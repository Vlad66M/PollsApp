namespace PollsApp.Mvc.ViewModels
{
    public class CommentVM
    {
        public string Text {  get; set; }
        public string UserName {  get; set; }
        public byte[] UserAvatar {  get; set; }
        public string PollId {  get; set; }
    }
}
