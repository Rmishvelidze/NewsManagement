using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Caching.Memory;
using NewsManagement.Application.Interfaces.Repositories;
using NewsManagement.Domain.Settings;
using NewsManagement.Persistence.Data;
using NewsManagement.Persistence.Implementations.Repositories.News;
using System.Reflection;
using NewsManagement.Persistence.Implementations.Repositories.Users;

namespace NewsManagement.Api.Configuration
{
    internal static class ServiceCollectionExtension
    {
        public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks();
        }

        [Obsolete("Obsolete")]
        public static void AddServices(this IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddControllers().AddFluentValidation();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<UserDataContext>();
            services.AddSingleton<NewsDataContext>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
        }
    }
}