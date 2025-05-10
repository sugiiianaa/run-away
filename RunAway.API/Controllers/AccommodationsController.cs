using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunAway.Application.Features.Accommodations.Commands.AddRoom;
using RunAway.Application.Features.Accommodations.Commands.CreateAccommodations;
using RunAway.Application.Features.Accommodations.Queries.GetAccommodationDetails;

namespace RunAway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccommodationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccommodationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAccommodationCommand command)
        {
            var accommodationId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = accommodationId }, accommodationId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccommodationDetailsViewModel>> GetById(Guid id)
        {
            var query = new GetAccommodationDetailsQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{id}/rooms")]
        public async Task<ActionResult<Guid>> AddRoom(Guid id, [FromBody] AddRoomCommand command)
        {
            command.AccommodationId = id;
            var roomId = await _mediator.Send(command);
            return Ok(roomId);
        }
    }
}
