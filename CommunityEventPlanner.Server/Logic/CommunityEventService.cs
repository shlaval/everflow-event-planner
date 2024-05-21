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
                if (!ValidateVenueIsFree(createRequest))
                {
                    return new Result<int>() { Message = "A booking already exists at the specified venue at the requested time", HttpStatusCode = 400 };
                }

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

                return new Result<int>(communityEvent.Id) { HttpStatusCode = 204, Success = true };
            }
            else
            {
                return new Result<int>() { Message = "Could not validate your user account", HttpStatusCode = 401 };
            }
        }

        private bool ValidateVenueIsFree(CommunityEventCreateRequest createRequest)
        {
            var venue = _context.Venues.FirstOrDefault(v => v.Id == createRequest.VenueId);
            if (venue != null)
            {
                return !venue.CommunityEvents.Any(ce => (ce.ScheduledDateTime < createRequest.ScheduledDateTime + createRequest.Duration) && 
                                                 (ce.ScheduledDateTime + ce.Duration > createRequest.ScheduledDateTime));
            }

            return false;
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
            var communityEvent = await _context.CommunityEvents.FirstOrDefaultAsync(ce => ce.Id == id);

            var userFound = _context.Users.Any(au =>  au.UserName == userId);

            if (!userFound)
            {
                return new Result<bool>(false) { Success = false, HttpStatusCode = 400, Message = "Invalid user." };
            }

            if (communityEvent != null)
            {
                if (communityEvent.SignUps.Any(su => su.UserId == userId))
                {
                    return new Result<bool>(false) { Success = false, HttpStatusCode = 400, Message = "That user has already signed up for that event." };
                }
                var signUp = new SignUp()
                {
                    CommunityEventId = id,
                    UserId = userId
                };

                _context.SignUps.Add(signUp);
                await _context.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool>(false) { Success = false, HttpStatusCode = 404, Message = "That community event could not be found" };
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
                if (!string.IsNullOrEmpty(searchRequest.SearchTerm))
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
