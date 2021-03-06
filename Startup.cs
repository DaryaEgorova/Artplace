using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artplace.Enums;
using Artplace.Helpers;
using Artplace.Models;
using Artplace.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artplace
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSession();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IAdvertRepository, AdvertRepository>();
            services.AddSingleton<ICommentRepository, CommentRepository>();
            services.AddSingleton<ILikesRepository, LikesRepository>();
            services.AddSingleton<IQuestionRepository, QuestionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserRepository userRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                if (!context.Session.Keys.Contains("role"))
                {
                    context.Session.SetString("role", Role.Guest.ToString());
                }

                await next.Invoke();
            });
            app.Use(async (context, next) =>
            {
                if (context.Request.Cookies.ContainsKey("enterkey")
                    && context.Session.GetString("role") == Role.Guest.ToString())
                {
                    var enterKey = context.Request.Cookies["enterkey"];
                    var user = await userRepository.GetUserByEnterKey(enterKey);
                    context.Session.SetString("role", user.Role.ToString());
                    context.Session.Set("user", user);
                }
                await next.Invoke();
            });
            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}