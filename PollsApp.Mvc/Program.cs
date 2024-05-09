using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PollsApp.Application.Persistence;
using PollsApp.Domain;
using PollsApp.Identity;
using PollsApp.Mvc.ApiClient;
using PollsApp.Mvc.Hubs;
using PollsApp.Mvc.LocalStorageServices;
using PollsApp.Persistence.Repositories;
using System.Text.Json.Serialization;

namespace PollsApp.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
            });

            /*builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));*/

            /*builder.Services.AddDbContext<DbContextSqlite>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DbConnectionString"),
                b => b.MigrationsAssembly(typeof(DbContextSqlite).Assembly.FullName)));

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DbContextSqlite>().AddDefaultTokenProviders();

            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IPollsRepository, PollsRepository>();
            builder.Services.AddTransient<IUsersRepository, UsersRepository>();*/



            builder.Services.AddTransient<PollsApp.Mvc.Authentication.IAuthenticationService, PollsApp.Mvc.Authentication.AuthenticationService>();

            builder.Services.AddTransient<IWebApiClient, WebApiClient>();

            builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();

            builder.Services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });
            builder.Services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
                o.MaximumReceiveMessageSize = 10240000; // bytes
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapGet("/", context =>
            {
                return Task.Run(() => context.Response.Redirect("/polls"));
            });
            app.MapHub<CommentsHub>("/commentsHub");

            app.Run();
        }
    }
}
