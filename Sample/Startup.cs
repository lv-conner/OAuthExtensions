using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OAuthHandler.Pocket;
using Sample.Authentications;

namespace Sample
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //services.AddAuthentication(option =>
            //{
            //    option.DefaultScheme = "cookie";
            //    option.DefaultChallengeScheme = "cookie";
            //})
            //.AddCookie("cookie",options =>
            //{
            //    options.LoginPath = "/Account/Login";
            //})
            //.AddOpenIdConnect("oidc", options =>
            // {
            //     options.SignInScheme = "cookie";
            //     options.Authority = "http://localhost:5000";
            //     options.RequireHttpsMetadata = false;

            //     options.ClientId = "mvc";
            //     options.ClientSecret = "secret";
            //     options.ResponseType = "code id_token";

            //     options.SaveTokens = true;
            //     options.GetClaimsFromUserInfoEndpoint = true;

            //     options.Scope.Add("api1");
            //     options.Scope.Add("offline_access");
            // });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Cookie";
                options.DefaultChallengeScheme = "Cookie";
                options.DefaultScheme = "Cookie";

            }).AddCookie("Cookie")
            .AddPocket("pocket", options =>
             {
                 options.SignInScheme = "Cookie";
                 options.ConsumerKey = "55221-b7c0099f2fffc528da4b254b";
                 options.AuthorizationEndpoint = "https://getpocket.com/auth/authorize";
                 options.RequestTokenEndpoint = "https://getpocket.com/v3/oauth/request";
                 options.CallbackPath = "/Account/Pocket";
                 options.TokenEndpoint = "https://getpocket.com/v3/oauth/authorize";
                 options.Events.OnRemoteFailure = context =>
                 {
                     context.Response.Redirect("/Account/PocketFail");
                     return Task.CompletedTask;
                 };
             });

            services.AddHttpClient("pocket", config =>
             {
                 config.BaseAddress = new Uri("https://getpocket.com/v3");
             }).AddTypedClient<PocketClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
