using CommunityEventPlanner.Server.Logic;
using CommunityEventPlanner.Shared;
using CommunityEventPlanner.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CommunityEvent communityEvent)
        {
            var result = await communityEventService.CreateCommunityEvent(communityEvent);
            return Ok(result);
        }
    }
}
