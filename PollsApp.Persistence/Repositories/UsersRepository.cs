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
        /*public Role GetRoles(string roleName)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                return db.URoles.Where(r => r.Name == "user").FirstOrDefault();
            }
        }*/

        /*public User GetUser(string email, string password)
        {
            *//*using (DbContextSqlite db = new DbContextSqlite())
            {
                var user = db.Users.Where(u => u.Email == email && u.Password == password).Include(u => u.Role).Include(u => u.Votes).FirstOrDefault();
                return user;
            }*//*
            throw new NotImplementedException();
        }*/

        /*public UserDto GetUser(string id)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var user = db.Users.Where(u => u.Id == id).Include(u => u.Role).Include(u => u.Votes).FirstOrDefault();
                UserDto userDto = new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Votes = user.Votes,
                    RoleName = user.Role.Name,
                    Role = user.Role

                };
                return userDto;
            }
        }*/

        /*public User GetUserByEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }*/

        public UserDto GetUserById(string id)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var user = db.Users.Where(u => u.Id == id).Include(u => u.Role).Include(u => u.Votes).FirstOrDefault();
                UserDto userDto = new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Votes = user.Votes,
                    RoleName = user.Role.Name,
                    Role = user.Role

                };
                return userDto;
            }
        }

        /*public bool isEmailRegistered(string email)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                return db.Users.Any(u => u.Email == email);
            }
        }*/

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
