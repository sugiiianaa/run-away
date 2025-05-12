using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunAway.API.Helpers;
using RunAway.Application.Dtos.User;
using RunAway.Application.Features.Users.Commands.CreateUser;
using RunAway.Application.Features.Users.Queries.LoginUser;

namespace RunAway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Create new user entity
        /// POST: /api/Users/register
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<CreateUserResponseDto>>> Create([FromBody] CreateUserCommand command)
        {
            var result = await mediator.Send(command);

            if (result == null)
                return this.ToApiError<CreateUserResponseDto>(400, "User already registered.");

            return result.ToApiResponse(201, "User created successfully");
        }

        /// <summary>
        /// Login user
        /// POST: /api/Users/login
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginUserResponseDto>>> Login([FromBody] LoginUserQuery query)
        {
            var result = await mediator.Send(query);

            if (result == null)
                return this.ToApiError<LoginUserResponseDto>(400, "Input invalid");

            return result.ToApiResponse(200);
        }
    }
}
