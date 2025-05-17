using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunAway.API.Helpers;
using RunAway.Application.Dtos.AccommodationAvailability;
using RunAway.Application.Features.AccommodationAvailabilities.Commands.AddRoomAvailabilities;
using RunAway.Infrastructure.Constants;

namespace RunAway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccommodationAvailabilityController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Add new room availability record
        /// POST: /api/RoomAvailability
        /// </summary>
        [HttpPost]
        [Authorize(Policy = UserAuthorizationPolicy.RequireAdminRole)]
        public async Task<ActionResult<ApiResponse<CreateAccommodationAvailabilityResponseDto>>> Create([FromBody] AddRoomAvailabilitiesCommand command)
        {
            var result = await _mediator.Send(command);

            if (result == null)
                return this.ToApiError<CreateAccommodationAvailabilityResponseDto>(400, "Input invalid");

            return result.ToApiResponse(201, "Room availability record added successfully");
        }
    }
}
