using Microsoft.EntityFrameworkCore;
using PollsApp.Application;
using PollsApp.Application.DTOs;
using PollsApp.Application.Persistence;
using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Persistence.Repositories
{
    /*internal class ApplicationContextSqlite : IPollsRepository, IUsersRepository
    {
        public void DeletePoll(long pollId)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var poll = db.Polls.Where(p => p.Id == pollId).FirstOrDefault();
                if (poll != null)
                {
                    db.Polls.Remove(poll);
                    db.SaveChanges();
                }
            }
        }

        public PollInfo GetPoll(long pollId, string userId)
        {
            PollInfo pollInfo = null;
            using (DbContextSqlite db = new DbContextSqlite())
            {
                Poll poll = db.Polls.Where(p => p.Id == pollId).Include(p => p.PollOptions).Include(p => p.Comments).ThenInclude(c => c.User).FirstOrDefault();
                //Poll poll = db.Polls.Where(p => p.Id == pollId).Include(p => p.PollOptions).FirstOrDefault();
                if (poll != null)
                {
                    pollInfo = new PollInfo();
                    User user = db.Users.Where(u => u.Id == userId).Include(u => u.Role).FirstOrDefault();

                    pollInfo.Poll = poll;
                    foreach (PollOption pollOption in poll.PollOptions)
                    {
                        OptionInfo optionInfo = new OptionInfo();
                        optionInfo.PollOption = pollOption;
                        optionInfo.Votes = db.Votes.Where(v => v.PollOptionId == pollOption.Id).Count();
                        optionInfo.IsChecked = db.Votes.Any(v => v.PollOptionId == pollOption.Id && v.UserId == userId);
                        if (optionInfo.IsChecked)
                        {
                            pollInfo.IsVoted = true;
                        }
                        pollInfo.Votes += optionInfo.Votes;
                        var userIds = db.Votes.Where(v => v.PollOptionId == pollOption.Id).Select(v => v.UserId).ToList();
                        var users = db.Users.Where(u => userIds.Contains(u.Id)).Include(u => u.Votes).ToList();

                        foreach (var u in users)
                        {
                            UserInfo userInfo = new UserInfo();
                            if (u.Votes.Any(v => v.PollOptionId == pollOption.Id && v.IsAnon == true))
                            {
                                var anonUser = db.Users.FirstOrDefault(u => u.Email == "anon@mail.com");
                                userInfo.UserId = anonUser.Id;
                                userInfo.UserName = anonUser.Name;
                                userInfo.UserAvatar = anonUser.Avatar;

                                if (u.Id == userId)
                                {
                                    pollInfo.IsAnon = true;
                                }
                            }
                            else
                            {
                                userInfo.UserId = u.Id;
                                userInfo.UserName = u.Name;
                                userInfo.UserAvatar = u.Avatar;
                            }
                            optionInfo.Users.Add(userInfo);
                        }
                        pollInfo.Options.Add(optionInfo);
                    }


                }
                return pollInfo;
            }
        }

        public PagedList<Poll> GetPolls(string? userId, string? search, bool? active, bool? notvoted, int page = 1)
        {
            long pollId = 0;
            bool isLong = false;
            try
            {
                pollId = long.Parse(search);
                isLong = true;
            }
            catch
            {

            }
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var polls = db.Polls.Include(p => p.PollOptions).AsQueryable();
                if (!String.IsNullOrEmpty(search))
                {
                    if (isLong)
                    {
                        polls = polls.Where(p => p.Id == pollId || p.Title.Contains(search));
                    }
                    else
                    {
                        polls = polls.Where(p => p.Title.Contains(search));
                    }
                }
                if (active == true)
                {
                    polls = polls.Where(p => p.IsActive == true && (p.EndDate > DateTime.Now || p.EndDate == null));
                }
                if (notvoted == true)
                {
                    var votedOptionsIds = db.Votes.Where(v => v.UserId == userId).Include(v => v.PollOption).Select(v => v.PollOptionId).Distinct().ToList();
                    if (votedOptionsIds != null)
                    {
                        polls = polls.Where(p => p.PollOptions.All(o => !votedOptionsIds.Contains(o.Id)));
                    }
                }
                int pageSize = 10;
                return PagedList<Poll>.ToPagedList(polls, page, pageSize);
            }
        }

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
            throw new NotImplementedException();
        }

        public bool isEmailRegistered(string email)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                return db.Users.Any(u => u.Email == email);
            }
        }

        public void PostComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }
            using (DbContextSqlite db = new DbContextSqlite())
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
        }

        public void PostPoll(Poll poll)
        {
            if (poll == null)
            {
                throw new ArgumentNullException(nameof(poll));
            }
            using (DbContextSqlite db = new DbContextSqlite())
            {
                db.Polls.Add(poll);

                foreach (var option in poll.PollOptions)
                {
                    option.Poll = poll;
                    db.PollOptions.Add(option);
                }

                db.SaveChanges();
            }
        }

        public User PostUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            using (DbContextSqlite db = new DbContextSqlite())
            {
                Role userRole = db.URoles.Where(r => r.Name == "user").FirstOrDefault();
                user.Role = userRole;
                db.Users.Add(user);
                db.SaveChanges();
                return user;
            }
        }

        public void PostVote(Vote vote)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                vote.User = db.Users.Where(u => u.Id == vote.UserId).FirstOrDefault();
                vote.PollOption = db.PollOptions.Where(o => o.Id == vote.PollOptionId).FirstOrDefault();
                db.Votes.Add(vote);
                db.SaveChanges();
            }
        }

        public void PutPoll(Poll poll)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var _poll = db.Polls.Where(p => p.Id == poll.Id).Include(p => p.PollOptions).FirstOrDefault();
                if (_poll != null)
                {
                    _poll.Title = poll.Title;
                    _poll.AllowComments = poll.AllowComments;
                    _poll.EndDate = poll.EndDate;
                    _poll.IsActive = poll.IsActive;
                    if (_poll.PollOptions != null)
                    {
                        bool optionsEditted = false;
                        if (poll.PollOptions.Count != _poll.PollOptions.Count)
                        {
                            optionsEditted = true;
                        }
                        else
                        {
                            string line1 = String.Join("", _poll.PollOptions.Select(o => o.Text).ToArray());
                            string line2 = String.Join("", poll.PollOptions.Select(o => o.Text).ToArray());
                            if (line1 != line2)
                            {
                                optionsEditted = true;
                            }
                        }
                        if (optionsEditted)
                        {
                            foreach (var o in _poll.PollOptions)
                            {
                                db.PollOptions.Remove(o);
                            }
                            foreach (var option in poll.PollOptions)
                            {
                                _poll.PollOptions = new List<PollOption>();
                                PollOption o = new();
                                o.Text = option.Text;
                                o.Poll = _poll;
                                _poll.PollOptions.Add(o);
                                db.PollOptions.Add(o);
                            }
                        }
                    }

                    db.SaveChanges();
                }
            }
        }
    }*/
}
