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
        private readonly ICommunityEventService _communityEventService;

        public CommunityEventController(ICommunityEventService communityEventService)
        {
            _communityEventService = communityEventService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody]SearchRequest searchRequest)
        {
            var result = await _communityEventService.GetCommunityEvents(searchRequest);
            return FromResult(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CommunityEventCreateRequest request)
        {
            var result = await _communityEventService.CreateCommunityEvent(request);
            return FromResult(result);
        }

        /// <summary>
        /// This is an incomplete example - should be factored out into a base class for other 
        /// controllers to inherit from 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        private IActionResult FromResult<T>(Result<T> result)
        {
            if (result.Success)
            {
                return Ok(result);
            }
            else if (result.HttpStatusCode == 403)
            {
                return Forbid(result.Message);
            }

            return StatusCode(result.HttpStatusCode);
        }
    }
}
