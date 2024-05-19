using CommunityEventPlanner.Shared;

namespace CommunityEventPlanner.Server.Logic
{
    public interface ICommunityEventService
    {
        Task<Result<CommunityEvent>> GetCommunityEvent(int id);
        Task<Result<int>> CreateCommunityEvent(CommunityEvent communityEvent);
        Task<Result<bool>> DeleteCommunityEvent(int id);
        Task<Result<bool>> UpdateCommunityEvent(CommunityEvent communityEvent);
        Task<Result<IList<CommunityEvent>>> GetCommunityEvents();   
    }
}
