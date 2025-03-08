using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using Auto1040.Core;
using Auto1040.Data.Repositories;
using Auto1040.Data;
using Auto1040.Service;
using Microsoft.EntityFrameworkCore;

namespace Auto1040.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddDependencyInjectoions(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MappingProfile), typeof(MappingPostProfile));

            // Register repositories
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();

            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserDetailsService, UserDetailsService>();
            services.AddScoped<IAuthService, AuthService>();
        }

    }
}
