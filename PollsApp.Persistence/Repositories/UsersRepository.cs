using Microsoft.EntityFrameworkCore;
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
        public Role GetRoles(string roleName)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                return db.URoles.Where(r => r.Name == "user").FirstOrDefault();
            }
        }

        public User GetUser(string email, string password)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var user = db.Users.Where(u => u.Email == email && u.Password == password).Include(u => u.Role).Include(u => u.Votes).FirstOrDefault();
                return user;
            }

        }

        public User GetUser(string id)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var user = db.Users.Where(u => u.Id == id).Include(u => u.Role).Include(u => u.Votes).FirstOrDefault();
                return user;
            }
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(string id)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var user = db.Users.Where(u => u.Id == id).Include(u => u.Role).Include(u => u.Votes).FirstOrDefault();
                return user;
            }
        }

        public bool isEmailRegistered(string email)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                return db.Users.Any(u => u.Email == email);
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
