using CommunityEventPlanner.Shared;
using CommunityEventPlanner.Shared.Requests;

namespace CommunityEventPlanner.Server.Logic
{
    public interface ICommunityEventService
    {
        Task<Result<CommunityEvent>> GetCommunityEvent(int id);
        Task<Result<int>> CreateCommunityEvent(CommunityEventCreateRequest communityEvent);
        Task<Result<bool>> DeleteCommunityEvent(int id);
        Task<Result<bool>> UpdateCommunityEvent(CommunityEventCreateRequest createRequest);
        Task<Result<IList<CommunityEvent>>> GetCommunityEvents(SearchRequest searchRequest);
        Task<Result<bool>> SignUpForCommunityEvent(int id, string userId);
    }
}
