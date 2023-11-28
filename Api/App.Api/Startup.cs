using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Data.Data;
using AutoMapper;
using App.Data.Entities;
using App.Service.Models;
using App.Service.Interfaces;
using App.Service.Services;
using App.Service.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using App.Service.Services.Email;

namespace App.Api
{
    public class Startup
    {
        private readonly SymmetricSecurityKey _signingKey;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var secretKey = Configuration.GetSection("JwtSecretKey").Value;
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }

        public IConfiguration Configuration
        {
            get;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);


            var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddIdentity<AppIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            });

            services.AddScoped<IJwtFactoryService, JwtFactoryService>();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<IAuthService, AuthService>();
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(configureOptions =>
            {
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                    IssuerSigningKey = _signingKey,
                    RequireExpirationTime = false,

                };

                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            var builder = services.AddIdentityCore<AppIdentityUser>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequiredLength = 8;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            RegisterServices(services);
        }

        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IPublicationService, PublicationService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IFollowersService, FollowersService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
        }
    }
}
