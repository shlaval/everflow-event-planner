using CommunityEventPlanner.Shared.Requests;
using CommunityEventPlanner.Tests.TestFixtures;
using FluentAssertions;

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

        [Fact]
        public async Task CommunityEventService_GetCommunityEvent_Ok()
        {
            // Arrange
            var communityEventService = _testFixture.CommunityEventService;

            // Act
            var result = await communityEventService.GetCommunityEvent(1);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.HttpStatusCode.Should().Be(200);
            var communityEvent = result.Value;
            communityEvent.Title.Should().Be("Book Club");
        }

        [Theory]
        [InlineData(4, "2024-5-28T10:00:00Z", false)]
        [InlineData(1, "2024-5-28T11:00:00Z", false)]
        [InlineData(1, "2024-5-28T10:00:00Z", true)]
        public async Task CommunityEventService_CreateCommunityEvent_Ok(int venueId, string scheduledDateTime, bool success)
        {
            // Arrange
            var communityEventService = _testFixture.CommunityEventService;
            var context = _testFixture.ApplicationDbContext;
            var maxId = context.CommunityEvents.Max(ce => ce.Id);
            var request = new CommunityEventCreateRequest()
            {
                Title = "Title",
                Description = "Description",
                VenueId = venueId,
                ScheduledDateTime = DateTime.Parse(scheduledDateTime),
                Duration = TimeSpan.FromMinutes(30)
            };

            // Act
            var result = await communityEventService.CreateCommunityEvent(request);

            // Assert
            result.Should().NotBeNull();
            if (success)
            {
                result.Success.Should().BeTrue();
                result.HttpStatusCode.Should().Be(204);
                result.Value.Should().Be(maxId + 1);
                var entity = context.CommunityEvents.First(e => e.Id == result.Value);
                context.CommunityEvents.Remove(entity);
            }
            else
            {
                result.Success.Should().BeFalse();
                result.HttpStatusCode.Should().Be(400);
                result.Message.Should().Be("A booking already exists at the specified venue at the requested time");
            }
        }

        [Theory]
        [InlineData("Cobra", new int[0], null, new int[] { 2, 4 })]
        [InlineData(null, new int[] { 1, 2, 3 }, null, new int[] { 1, 2, 3, 4})]
        [InlineData(null, new int[] { 3 }, null, new int[] { 3 })]
        [InlineData(null, new int[0], 1, new int[] { 1, 4 })]
        public async Task CommunityEventService_Search_Ok(string searchTerm, int[] tagIds, int? venueId, int[] expected)
        {
            // Arrange
            var communityEventService = _testFixture.CommunityEventService;
            var searchRequest = new SearchRequest()
            {
                SearchTerm = searchTerm,
                TagIds = tagIds.ToList(),
                VenueId = venueId
            };

            // Act
            var result = await communityEventService.GetCommunityEvents(searchRequest);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.HttpStatusCode.Should().Be(200);
            var retrievedIds = result.Value.Select(ce => ce.Id).ToList();
            retrievedIds.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1, "TestUser1", true)]
        [InlineData(1, "TestUser2", false)]
        [InlineData(2, "TestUser1", true)]
        [InlineData(2, "TestUser2", true)]
        public async Task CommunityEventservice_Signup_Ok(int communityEventId, string userId, bool success)
        {
            // Arrange
            var communityEventService = _testFixture.CommunityEventService;
            var context = _testFixture.ApplicationDbContext;

            // Act
            var result = await communityEventService.SignUpForCommunityEvent(communityEventId, userId);

            // Assert
            result.Should().NotBeNull();
            if (success)
            {
                result.Success.Should().BeTrue();
                result.HttpStatusCode.Should().Be(200);
            }
            else
            {
                result.Success.Should().BeFalse();
                result.HttpStatusCode.Should().Be(400);
            }
        }
    }
}