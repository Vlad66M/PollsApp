﻿using Microsoft.EntityFrameworkCore;
using PollsApp.Application;
using PollsApp.Application.DTOs;
using PollsApp.Application.Persistence;
using PollsApp.Domain;
using PollsApp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PollsApp.Persistence.Repositories
{
    public class PollsRepository : IPollsRepository
    {

        public PollsRepository()
        {
        }
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

        public Application.DTOs.PollDetails GetPoll(long pollId, string userId)
        {
            Application.DTOs.PollDetails pollInfo = null;
            using (DbContextSqlite db = new DbContextSqlite())
            {
                Poll poll = db.Polls.Where(p => p.Id == pollId).Include(p => p.PollOptions).Include(p => p.Comments).ThenInclude(c => c.User).FirstOrDefault();
                if (poll != null)
                {
                    pollInfo = new Application.DTOs.PollDetails();
                    User user = db.Users.Where(u => u.Id == userId).Include(u => u.Role).FirstOrDefault();

                    pollInfo.Poll = poll;
                    foreach (PollOption pollOption in poll.PollOptions)
                    {
                        Application.DTOs.OptionDetails optionInfo = new();
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
                            User userInfo = new();
                            if (u.Votes.Any(v => v.PollOptionId == pollOption.Id && v.IsAnon == true))
                            {
                                var anonUser = db.Users.FirstOrDefault(u => u.Email == "anon@mail.com");
                                userInfo.Id = anonUser.Id;
                                userInfo.Name = anonUser.Name;
                                userInfo.Avatar = anonUser.Avatar;

                                if (u.Id == userId)
                                {
                                    pollInfo.IsAnon = true;
                                }
                            }
                            else
                            {
                                userInfo.Id = u.Id;
                                userInfo.Name = u.Name;
                                userInfo.Avatar = u.Avatar;
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
            using (DbContextSqlite db = new DbContextSqlite())
            {
                var polls = db.Polls.Include(p => p.PollOptions).OrderByDescending(p => p.Id).AsQueryable();
                if (!String.IsNullOrEmpty(search))
                {
                    //polls = polls.Where(p => p.Title.ToLower().Contains(search.ToLower()));
                    polls = polls.Where(p => EF.Functions.Like(p.Title, "%" + search + "%"));
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

        public void PostVote(Vote vote)
        {
            using (DbContextSqlite db = new DbContextSqlite())
            {
                vote.User = db.Users.Where(u => u.Id == vote.UserId).FirstOrDefault();
                vote.PollOption = db.PollOptions.Where(o => o.Id == vote.PollOptionId).FirstOrDefault();
                if (vote.PollOption is null) return;
                var pollId = vote.PollOption.PollId;
                var optionsIds = db.PollOptions.Where(o => o.PollId == pollId).Select(o => o.Id).ToList();
                var isVoted = db.Votes.Any(v => v.UserId == vote.UserId && optionsIds.Contains(v.PollOptionId));
                if (isVoted) return;
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
                            for(int i=0; i<poll.PollOptions.Count; i++) 
                            {
                                if (poll.PollOptions[i].Text != _poll.PollOptions[i].Text)
                                {
                                    optionsEditted = true;
                                    break;
                                }
                                if(poll.PollOptions[i].Photo is not null)
                                {
                                    try
                                    {
                                        if (!poll.PollOptions[i].Photo.SequenceEqual(_poll.PollOptions[i].Photo))
                                        {
                                            optionsEditted = true;
                                            break;
                                        }
                                    }
                                    catch
                                    {
                                        optionsEditted = true;
                                        break;
                                    }
                                }
                                if (poll.PollOptions[i].Audio is not null)
                                {
                                    try
                                    {
                                        if (!poll.PollOptions[i].Audio.SequenceEqual(_poll.PollOptions[i].Audio))
                                        {
                                            optionsEditted = true;
                                            break;
                                        }
                                    }
                                    catch
                                    {
                                        optionsEditted = true;
                                        break;
                                    }
                                }
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
                                o.Photo = option.Photo;
                                o.Audio = option.Audio;
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
    }
}
