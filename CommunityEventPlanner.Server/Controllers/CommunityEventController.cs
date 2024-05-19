using CommunityEventPlanner.Server.Logic;
using CommunityEventPlanner.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CommunityEventPlanner.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommunityEventController : ControllerBase
    {
        private readonly ICommunityEventService communityEventService;

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody]SearchRequest searchRequest)
        {
            var result = await communityEventService.GetCommunityEvents(searchRequest);
            return Ok(result);
        }
    }
}
