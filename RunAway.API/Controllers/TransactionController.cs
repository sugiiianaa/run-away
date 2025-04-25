using Microsoft.AspNetCore.Mvc;
using RunAway.API.Helpers;

namespace RunAway.API.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status201Created)]
        public Task<ActionResult<ApiResponse<Guid>>> CreateTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
