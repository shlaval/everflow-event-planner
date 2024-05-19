using CommunityEventPlanner.Shared;
using CommunityEventPlanner.Shared.Requests;

namespace CommunityEventPlanner.Server.Logic
{
    public class CommunityEventService : ICommunityEventService
    {
        public Task<Result<int>> CreateCommunityEvent(CommunityEvent communityEvent)
        {
            throw new NotImplementedException();
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
    }
}
