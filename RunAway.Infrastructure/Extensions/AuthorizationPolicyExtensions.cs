using Microsoft.Extensions.DependencyInjection;
using RunAway.Domain.Enums;

namespace RunAway.Infrastructure.Extensions
{
    public static class AuthorizationPolicyExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Role-based policies
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireRole(UserRoles.Admin.ToString(), UserRoles.SuperUser.ToString()));

                options.AddPolicy("RequireSuperUserRole", policy =>
                    policy.RequireRole(UserRoles.SuperUser.ToString()));

                options.AddPolicy("RequireUserRole", policy =>
                    policy.RequireRole(UserRoles.User.ToString(), UserRoles.Admin.ToString(), UserRoles.SuperUser.ToString()));
            });

            return services;
        }
    }
}
