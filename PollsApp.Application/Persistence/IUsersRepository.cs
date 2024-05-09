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
        //public User GetUserByEmailAndPassword(string email, string password);

        public UserDto GetUserById(string id);

        public User PutUser(User user);

        //public bool isEmailRegistered(string email);

        //public Role GetRoles(string roleName);
    }
}
