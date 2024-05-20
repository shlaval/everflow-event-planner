using CommunityEventPlanner.Server.Logic;
using CommunityEventPlanner.Shared.Requests;
using CommunityEventPlanner.Tests.TestFixtures;

namespace CommunityEventPlanner.Tests
{
    public class CommunityEventServiceTests : IClassFixture<CommunityEventServiceTestFixture>
    {
        private readonly CommunityEventServiceTestFixture _testFixture;

        public CommunityEventServiceTests(CommunityEventServiceTestFixture communityEventServiceTestFixture)
        {
            _testFixture = communityEventServiceTestFixture;
        }

        [Fact]
        public async Task CommunityEventService_UpdateCommunityEvent_Throws_NotImplementedException()
        {
            // Arrange
            var communityEventService = _testFixture.CommunityEventService;
            var test = () => communityEventService.UpdateCommunityEvent(new CommunityEventCreateRequest());

            // Act and Assert
            await Assert.ThrowsAsync<NotImplementedException>(() => test());
        }
    }
}