using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunAway.API.Helpers;

namespace RunAway.API.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "RequireUserRole")]
        [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status201Created)]
        public Task<ActionResult<ApiResponse<Guid>>> CreateTransaction()
        {
            Console.WriteLine("Created!");
            throw new NotImplementedException();
        }
    }
}
