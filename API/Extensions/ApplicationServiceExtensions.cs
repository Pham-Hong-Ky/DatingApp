
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection service, IConfiguration config)
        {
            service.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<IPhotoService, PhotoService>();
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            service.AddDbContext<DataContext>(options=>{
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return service;
        }
    }
}