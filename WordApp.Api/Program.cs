using Serilog;
using WordApp.Api.AutoMapperProfiles;
using WordApp.Api.Extensions;
using WordApp.Api.Middlewares;

namespace WordApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddDatabaseContext(builder.Configuration);
            builder.Services.AddServices();
            builder.Services.AddJwtAuthentification(builder.Configuration);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            builder.Services.AddAutoMapper(typeof(AppMappingProfile));

            Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File("logs/yaelapp-log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod());

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();


            app.MapControllers();

            app.Run();

        }
    }
}