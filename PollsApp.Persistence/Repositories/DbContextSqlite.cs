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
            //modelBuilder.Entity<User>().HasKey(m => m.Id);
            /*modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUser<string>>();
            modelBuilder.Ignore<User>();*/
            /*Role roleAdmin = new Role();
            roleAdmin.Id = 1;
            roleAdmin.Name = "admin";

            Role roleUser = new Role();
            roleUser.Id = 2;
            roleUser.Name = "user";

            modelBuilder.Entity<Role>().HasData(
                    roleAdmin,
                    roleUser
            );

            modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, Name = "anon", Email = "anon@mail.com", Password = "12345", RoleId = roleUser.Id },
                    new User { Id = 2, Name = "admin", Email = "admin@mail.com", Password = "12345", RoleId = roleAdmin.Id },
                    new User { Id = 3, Name = "user1", Email = "user1@mail.com", Password = "12345", RoleId = roleUser.Id }
            );*/
        }
    }
}
