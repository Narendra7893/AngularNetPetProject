using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using API.Services;
using API.Helpers;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
             services.AddScoped<ITokenService, TokenService>();
             services.AddScoped<IUserRepository,UserRepository>();
             services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
             services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
             services.AddScoped<IPhotoInterface, PhotoService>();
             return services;
        }
        
    }
}