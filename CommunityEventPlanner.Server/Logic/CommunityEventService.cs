using CommunityEventPlanner.Shared;
using CommunityEventPlanner.Shared.Requests;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CommunityEventPlanner.Server.Logic
{
    public class CommunityEventService : ICommunityEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommunityEventService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<int>> CreateCommunityEvent(CommunityEvent communityEvent)
        {
            var userId = GetUserId();
            if (userId != null)
            {
                var newCommunityEvent = new CommunityEvent()
                {
                    Title = communityEvent.Title,
                    Description = communityEvent.Description,
                    VenueId = communityEvent.VenueId,
                    OwnerId = userId,
                };

                _context.CommunityEvents.Add(newCommunityEvent);
            }

            return new Result<int>() { Message = "An unknown error occurred", HttpStatusCode = 500 };
        }

        public Task<Result<bool>> DeleteCommunityEvent(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<CommunityEvent>> GetCommunityEvent(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IList<CommunityEvent>>> GetCommunityEvents(SearchRequest searchRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> SignUpForCommunityEvent(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> UpdateCommunityEvent(CommunityEvent communityEvent)
        {
            throw new NotImplementedException();
        }

        private string? GetUserId()
        {
            if (_httpContextAccessor.HttpContext?.User != null)
            {
                return _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
            }

            return null;
        }
    }
}
