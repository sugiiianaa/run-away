using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunAway.API.Helpers;
using RunAway.Application.Dtos;
using RunAway.Application.Features.Users.Commands.CreateUser;
using RunAway.Application.Features.Users.Queries.LoginUser;
using RunAway.Domain.ValueObjects;

namespace RunAway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType<ApiResponse<UserIdDto>>(StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<UserIdDto>>> Create([FromBody] RegisterUserDto request)
        {
            var command = new CreateUserCommand
            {
                Email = new Email(request.Email),
                Password = request.Password,
                Name = request.Name,
            };

            var result = await mediator.Send(command);

            return result.ToApiResponse(201, "User created successfully");
        }

        [HttpPost("login")]
        [ProducesResponseType<ApiResponse<TokenDto>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<TokenDto>>> Login([FromBody] LoginUserDto request)
        {
            var command = new LoginUserQuery
            {
                Email = new Email(request.Email),
                Password = request.Password,
            };

            var result = await mediator.Send(command);

            return result.ToApiResponse(200);
        }
    }
}
