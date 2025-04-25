using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RunAway.Application.IServices;
using RunAway.Application.Services;

namespace RunAway.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);

            });

            services.AddAutoMapper(assembly);

            // Services
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
