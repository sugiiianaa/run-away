using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RunAway.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Get the assembly where all handlers are located
            var assembly = Assembly.GetExecutingAssembly();

            // Register MediatR and specify the assembly scanning more explicitly
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);

                // Optional: Add pipeline behaviors if you're using them
                // config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                // config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            });

            // Add AutoMapper
            services.AddAutoMapper(assembly);

            // Optional: Add FluentValidation if you're using it
            // services.AddValidatorsFromAssembly(assembly);

            // Return the service collection for method chaining
            return services;
        }
    }
}
