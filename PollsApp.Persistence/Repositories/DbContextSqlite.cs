using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Persistence.Repositories
{
    public class DbContextSqlite : IdentityDbContext<User>
    {
        public DbSet<Role> URoles { get; set; } = null!;
        //public DbSet<User> Users { get; set; } = null!;
        public DbSet<Poll> Polls { get; set; } = null!;
        public DbSet<PollOption> PollOptions { get; set; } = null!;
        public DbSet<Vote> Votes { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        public DbContextSqlite(DbContextOptions<DbContextSqlite> options) : base(options)
        {
        }

        public DbContextSqlite()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
