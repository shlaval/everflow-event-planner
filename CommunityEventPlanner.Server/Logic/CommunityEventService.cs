using CommunityEventPlanner.Shared;
using CommunityEventPlanner.Shared.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Result<int>> CreateCommunityEvent(CommunityEventCreateRequest createRequest)
        {
            var userId = GetUserId();
            if (userId != null)
            {
                var communityEvent = new CommunityEvent()
                {
                    Title = createRequest.Title,
                    Description = createRequest.Description,
                    VenueId = createRequest.VenueId,
                    OwnerId = userId,
                    ScheduledDateTime = createRequest.ScheduledDateTime,
                    Duration = createRequest.Duration
                };

                _context.CommunityEvents.Add(communityEvent);
                await _context.SaveChangesAsync();
            }

            return new Result<int>() { Message = "An unknown error occurred", HttpStatusCode = 500 };
        }

        public async Task<Result<bool>> DeleteCommunityEvent(int id)
        {
            var userId = GetUserId();
            if (userId != null)
            {
                var communityEvent = await _context.CommunityEvents.FirstOrDefaultAsync(ce => ce.Id == id);
                if (communityEvent == null)
                {
                    return new Result<bool>(false) { HttpStatusCode = 404, Message = "The requested entity was not found" };
                }
                else if (communityEvent.OwnerId != userId)
                {
                    return new Result<bool>(false) { HttpStatusCode = 403, Message = "The requested operation is not permitted." };
                }
                else
                {
                    _context.CommunityEvents.Remove(communityEvent);
                    await _context.SaveChangesAsync();
                }
            }

            return new Result<bool>() { Message = "An unknown error occurred", HttpStatusCode = 500 };
        }

        public async Task<Result<CommunityEvent>> GetCommunityEvent(int id)
        {
            var communityEvent = await _context.CommunityEvents.FirstOrDefaultAsync(ce => ce.Id == id);
            if (communityEvent == null)
            {
                return new Result<CommunityEvent>() { HttpStatusCode = 404, Message = "The requested entity was not found" };
            }

            return new Result<CommunityEvent>(communityEvent);
        }

        public async Task<Result<IList<CommunityEvent>>> GetCommunityEvents(SearchRequest searchRequest)
        {
            var queryable = GetCommunityEventQueryable(searchRequest);
            var communityEvents = await queryable.ToListAsync();
            return new Result<IList<CommunityEvent>>(communityEvents);
        }

        public async Task<Result<bool>> SignUpForCommunityEvent(int id, string userId)
        {
            var signUp = new SignUp()
            {
                CommunityEventId = id,
                UserId = userId
            };

            _context.SignUps.Add(signUp);
            await _context.SaveChangesAsync();
            return new Result<bool>(true);
        }

        public Task<Result<bool>> UpdateCommunityEvent(CommunityEventCreateRequest communityEvent)
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

        private IQueryable<CommunityEvent> GetCommunityEventQueryable(SearchRequest searchRequest)
        {
            var baseQueryable = _context.CommunityEvents.AsQueryable();

            if (searchRequest != null)
            {
                if (searchRequest.SearchTerm != null)
                {
                    baseQueryable = baseQueryable.Where(ce => ce.Title.Contains(searchRequest.SearchTerm) || ce.Description.Contains(searchRequest.SearchTerm));
                }

                if (searchRequest.VenueId.HasValue)
                {
                    baseQueryable = baseQueryable.Where(ce => ce.VenueId == searchRequest.VenueId.Value);
                }

                if (searchRequest.TagIds.Any())
                {
                    baseQueryable = baseQueryable.Where(ce => ce.Tags.Any(t => searchRequest.TagIds.Contains(t.Id)));
                }

                if (searchRequest.StartDate.HasValue)
                {
                    baseQueryable = baseQueryable.Where(ce => ce.ScheduledDateTime > searchRequest.StartDate.Value);
                }

                if (searchRequest.EndDate.HasValue)
                {
                    baseQueryable = baseQueryable.Where(ce => ce.ScheduledDateTime < searchRequest.EndDate.Value);
                }
            }

            return baseQueryable;
        }
    }
}
