using Microsoft.EntityFrameworkCore;
using PollsApp.Application.DTOs;
using PollsApp.Application.Persistence;
using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {

        public User GetUserById(string id)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var user = db.Users.Where(u => u.Id == id).Include(u => u.Role).Include(u => u.Votes).FirstOrDefault();
                
                return user;
            }
        }

        public User PutUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var u = db.Users.Where(u=>u.Id == user.Id).First();
                u.Email = user.Email;
                u.Name = user.Name;
                u.UserName = user.Name;
                u.NormalizedUserName = user.Name.ToUpper();
                u.Avatar = user.Avatar;
                db.SaveChanges();
                return user;
            }
        }
    }
}
