using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RunAway.API.Helpers;
using RunAway.Application.IRepositories;

namespace RunAway.API.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ResourceOwnerAuthorizeAttribute : TypeFilterAttribute
    {
        public ResourceOwnerAuthorizeAttribute(Type type) : base(type)
        {
        }

        public class ResourceOwnerAuthorizeFilter : IAsyncAuthorizationFilter
        {
            private readonly IAccommodationRepository _accommodationRepository;

            public ResourceOwnerAuthorizeFilter(IAccommodationRepository accommodationRepository)
            {
                _accommodationRepository = accommodationRepository;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                // Check if user is authenticated
                if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Admin always has access
                if (UserClaimsHelper.IsAdmin(context.HttpContext.User))
                {
                    return;
                }

                // Get the user ID from claims
                var userId = UserClaimsHelper.GetUserId(context.HttpContext.User);
                if (userId == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Extract accommodation ID from route
                if (!context.RouteData.Values.TryGetValue("id", out var routeId) ||
                    !Guid.TryParse(routeId?.ToString(), out var accommodationId))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                // Check if user is the owner of the resource
                var accommodation = await _accommodationRepository.GetByIdAsync(accommodationId);
                if (accommodation == null)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
