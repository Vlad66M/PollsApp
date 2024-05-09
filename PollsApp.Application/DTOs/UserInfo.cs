using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public byte[] UserAvatar { get; set; }

        public string UserEmail { get; set; }
        public string RoleName {  get; set; }
    }
}
