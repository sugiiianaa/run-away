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
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Create new user entity
        /// POST: /api/Users/register
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<CreateUserResponseDto>>> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return this.ToApiError<CreateUserResponseDto>(result.ApiResponseErrorCode, result.ErrorMessage);
            }

            return result.ToApiResponse(201, "User created successfully");
        }

        /// <summary>
        /// Login user
        /// POST: /api/Users/login
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<LoginUserResponseDto>>> Login([FromBody] LoginUserQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return this.ToApiError<LoginUserResponseDto>(result.ApiResponseErrorCode, result.ErrorMessage);
            }

            return result.ToApiResponse(200);
        }
    }
}
