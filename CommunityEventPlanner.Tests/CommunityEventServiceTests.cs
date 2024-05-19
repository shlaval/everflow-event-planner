using CommunityEventPlanner.Server.Logic;
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
        public void Test1()
        {

        }
    }
}