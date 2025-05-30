﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunAway.Application.IRepositories;
using RunAway.Application.IServices;
using RunAway.Domain.Commons;
using RunAway.Infrastructure.Extensions;
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

            services.AddJwtAuthentication();

            // Repositories
            services.AddScoped<IAccommodationRepository, AccommodationRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IAccommodationAvailabilityRepository, AccommodationAvailabilityRepository>();

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IAccommodationService, AccommodationService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAccommodationAvailabilityService, AccommodationAvailabilityService>();

            return services;
        }
    }
}
