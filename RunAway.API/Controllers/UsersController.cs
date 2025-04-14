using MediatR;
using Microsoft.AspNetCore.Mvc;
using RunAway.API.Helpers;
using RunAway.API.RequestViewModel;
using RunAway.Application.Features.Users.Commands.CreateUser;
using RunAway.Domain.ValueObjects;

namespace RunAway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<Guid>>> Create([FromBody] LoginRequestViewModel request)
        {
            var command = new CreateUserCommand
            {
                Email = new Email(request.Email),
                Password = request.Password,
                Name = request.Name,
            };

            var result = await _mediator.Send(command);

            return result.ToApiResponse(201, "User created successfully");
        }
    }
}
