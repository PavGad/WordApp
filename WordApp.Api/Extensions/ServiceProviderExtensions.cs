using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WordApp.Domain.Interfaces;
using WordApp.Domain.Services;
using WordApp.Persistence;
using WordApp.Persistence.Interfaces;
using WordApp.Persistence.Repositories;

namespace WordApp.Api.Extensions
{
    public static class ServiceProviderExtensions
    {

        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration["ConnectionStrings:Default"], builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));
        }

        public static void AddJwtAuthentification(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["JwtSettings:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = config["JwtSettings:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]))
                };
            });
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IUserWordService, UserWordService>();
            services.AddTransient<IUserWordsRepository, UserWordsRepository>();

            services.AddTransient<IWordSetService, WordSetService>();
            services.AddTransient<IWordSetRepository, WordSetRepository>();

            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IUserService, UserService>();
        }

    }
}
