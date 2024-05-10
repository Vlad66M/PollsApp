using PollsApp.Application.DTOs;
using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.Persistence
{
    public interface IUsersRepository
    {
        public User GetUserById(string id);

        public User PutUser(User user);

    }
}
