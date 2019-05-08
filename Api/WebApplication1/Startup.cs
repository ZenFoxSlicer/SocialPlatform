using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using App.Data.Data;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using App.Service.Models;
using App.Service.Helpers;
using AutoMapper;
using App.Service.Services;
using App.Service.Interfaces;
using App.Data.Entities;
using System.Net;
using FluentValidation.AspNetCore;
using App.Service.Models.Validators;


namespace App.Api
{
    public class Startup
    {
        private readonly SymmetricSecurityKey _signingKey;

        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;

            var secretKey = Configuration.GetSection( "JwtSecretKey" ).Value;
            _signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddMvc();
            services.AddDbContext<ApplicationDbContext>( options =>
                options.UseNpgsql( Configuration.GetConnectionString( "DefaultConnection" ) ,
                    b => b.MigrationsAssembly( "App.Data" ) ) );

            services.AddScoped<IJwtFactoryService , JwtFactoryService>();
            //services.AddSingleton<IJwtFactoryService , JwtFactoryService>();
            var jwtAppSettingOptions = Configuration.GetSection( nameof( JwtIssuerOptions ) );

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>( options =>
            {
                options.Issuer = jwtAppSettingOptions[ nameof( JwtIssuerOptions.Issuer ) ];
                options.Audience = jwtAppSettingOptions[ nameof( JwtIssuerOptions.Audience ) ];
                options.SigningCredentials = new SigningCredentials( _signingKey , SecurityAlgorithms.HmacSha256 );
                options.ValidFor = TimeSpan.FromMinutes( Convert.ToInt64( jwtAppSettingOptions[ nameof( JwtIssuerOptions.Expiration ) ] ) );
            } );
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true ,
                ValidIssuer = jwtAppSettingOptions[ nameof( JwtIssuerOptions.Issuer ) ] ,

                ValidateAudience = true ,
                ValidAudience = jwtAppSettingOptions[ nameof( JwtIssuerOptions.Audience ) ] ,

                ValidateIssuerSigningKey = true ,
                IssuerSigningKey = _signingKey ,

                RequireExpirationTime = false ,
                ValidateLifetime = false ,
                ClockSkew = TimeSpan.Zero
            };

            //services.AddAuthorization( options =>
            //{
            //    options.AddPolicy( "ApiUser" , policy => policy.RequireClaim( Constants.Strings.JwtClaimIdentifiers.Rol , Constants.Strings.JwtClaims.ApiAccess ) );
            //} );

            services.AddAuthentication( options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            } ).AddJwtBearer( configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[ nameof( JwtIssuerOptions.Issuer ) ];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            } );

            var builder = services.AddIdentityCore<AppIdentityUser>( o =>
         {
             o.Password.RequireDigit = true;
             o.Password.RequireLowercase = false;
             o.Password.RequireUppercase = true;
             o.Password.RequireNonAlphanumeric = true;
             o.Password.RequiredLength = 8;
         } );

            builder = new IdentityBuilder( builder.UserType , typeof( IdentityRole ) , builder.Services );
            builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddMvc()
                .AddJsonOptions( options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                } )
                .AddFluentValidation( fv => fv.RegisterValidatorsFromAssemblyContaining<RegistrationViewModelValidator>() );

            services.AddMvc().SetCompatibilityVersion( CompatibilityVersion.Version_2_2 );

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app , IHostingEnvironment env )
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler(
            builder =>
            {
                builder.Run(
                  async context =>
                  {
                  context.Response.StatusCode = ( int )HttpStatusCode.InternalServerError;
                  context.Response.Headers.Add( "Access-Control-Allow-Origin" , "*" );

                  var error = context.Features.Get<IExceptionHandlerFeature>();
                  if ( error != null )
                  {
                      context.Response.Headers.Add("Application-Error", error.Error.Message );
                      context.Response.Headers.Add( "access-control-expose-headers" , "Application-Error" );
                  }
                  } );
            } );

            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
