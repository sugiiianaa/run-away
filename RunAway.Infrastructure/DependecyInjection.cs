using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunAway.Application.IRepositories;
using RunAway.Domain.Commons;
using RunAway.Infrastructure.Persistence;
using RunAway.Infrastructure.Repositories;
using RunAway.Infrastructure.Services;

namespace RunAway.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                       ?? throw new InvalidOperationException("DB_CONNECTION_STRING is not set.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString,
                    x => x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddScoped<IAccommodationRepository, AccommodationRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
