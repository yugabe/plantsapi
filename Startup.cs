using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Plants.API.Data;
using Plants.API.Models;
using Plants.API.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Plants.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(c =>
                {
                    var p = c.Password;
                    p.RequireDigit = p.RequireLowercase = p.RequireNonAlphanumeric = p.RequireUppercase = false;
                    p.RequiredLength = 1;
                    c.User.RequireUniqueEmail = false;
                    c.ClaimsIdentity.UserIdClaimType = "userid";
                    c.Cookies.ApplicationCookie.AutomaticChallenge = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<SecurityKey>(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Secrets:JwtSecurityKey").Value)));

            services.AddSingleton<AuthorizationFlipSwitch>();
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Plants API", Version = "v1" });
                //c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Plants.API.xml"));
            });

            services.AddScoped(s => new CurrentUserInfo { Id = s.GetRequiredService<IHttpContextAccessor>().HttpContext.User.FindFirst("userid")?.Value });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SecurityKey jwtSigningKey, AuthorizationFlipSwitch authFlipSwitch)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(p => p.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowAnyOrigin()
                              .AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseWhen(c => !IsApi(c), branch =>
                {
                    branch.UseDeveloperExceptionPage();
                    branch.UseDatabaseErrorPage();
                    branch.UseBrowserLink();
                });
            }
            else
            {
                app.UseWhen(c => !IsApi(c), branch =>
                {
                    branch.UseExceptionHandler("/Home/Error");
                });
            }

            app.UseWhen(IsApi, branch =>
            {
                branch.UseApiInterceptor();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Plants API V1");
            });

            app.UseStaticFiles();

            var jwtOptions = new JwtBearerOptions { AutomaticAuthenticate = true };
            jwtOptions.TokenValidationParameters.IssuerSigningKey = jwtSigningKey;
            jwtOptions.TokenValidationParameters.ValidateAudience = false;
            jwtOptions.TokenValidationParameters.ValidateActor = false;
            jwtOptions.TokenValidationParameters.ValidateIssuer = false;
            //jwtOptions.Events = new JwtBearerEvents();

            app.UseWhen(context => IsApi(context) && authFlipSwitch.Enabled,
                branch => branch.UseJwtBearerAuthentication(jwtOptions));

            app.UseWhen(context => IsApi(context) && !authFlipSwitch.Enabled,
                branch => branch.UseAnonymousUserMockAuthentication());

            app.UseWhen(context => !IsApi(context), 
                branch => branch.UseIdentity());

            app.UseMvcWithDefaultRoute();
        }
        private static bool IsApi(HttpContext context) => context.Request.Path.StartsWithSegments(new PathString("/api"), StringComparison.OrdinalIgnoreCase);
    }
}
