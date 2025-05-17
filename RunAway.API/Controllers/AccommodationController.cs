using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunAway.API.Helpers;
using RunAway.Application.Dtos.Accommodation;
using RunAway.Application.Dtos.Room;
using RunAway.Application.Features.Accommodations.Commands.AddRoom;
using RunAway.Application.Features.Accommodations.Commands.CreateAccommodations;
using RunAway.Application.Features.Accommodations.Queries.GetAccommodationDetails;
using RunAway.Infrastructure.Constants;

namespace RunAway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccommodationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Add new accommodation entity
        /// POST: /api/Accommodation
        /// </summary>
        [HttpPost]
        [Authorize(Policy = UserAuthorizationPolicy.RequireAdminRole)]
        public async Task<ActionResult<ApiResponse<CreateAccommodationResponseDto>>> Create([FromBody] CreateAccommodationCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return this.ToApiError<CreateAccommodationResponseDto>(result.ApiResponseErrorCode, result.ErrorMessage);
            }

            return result.ToApiResponse(201, "Accommodation created successfully");
        }

        /// <summary>
        /// Add new room to existing accommodation entity
        /// POST: /api/Accommodation/add-rooms
        /// </summary>
        [HttpPost("add-rooms")]
        [Authorize(Policy = UserAuthorizationPolicy.RequireAdminRole)]
        public async Task<ActionResult<ApiResponse<IList<CreateRoomResponseDto>>>> AddRoom([FromBody] AddRoomCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return this.ToApiError<IList<CreateRoomResponseDto>>(result.ApiResponseErrorCode, result.ErrorMessage);
            }

            return result.ToApiResponse(201, "Room added successfully");
        }

        /// <summary>
        /// Get accommodation detail properties
        /// GET: /api/Accommodation/{{id}}
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<GetAccomodationDetailResponseDto>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAccommodationDetailsQuery { Id = id });

            if (!result.IsSuccess)
            {
                return this.ToApiError<GetAccomodationDetailResponseDto>(result.ApiResponseErrorCode, result.ErrorMessage);
            }

            return result.ToApiResponse(200);
        }

    }
}
