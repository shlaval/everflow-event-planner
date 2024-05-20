using CommunityEventPlanner.Server.Controllers;
using CommunityEventPlanner.Server.Logic;
using CommunityEventPlanner.Shared;
using CommunityEventPlanner.Shared.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CommunityEventPlanner.Tests
{
    public class CommunityEventControllerTests
    {
        private CommunityEventController _communityEventController;
        private Mock<ICommunityEventService> _communityEventServiceMock;

        public CommunityEventControllerTests()
        {
            _communityEventServiceMock = new Mock<ICommunityEventService>();
            _communityEventController = new CommunityEventController(_communityEventServiceMock.Object);
        }

        [Fact]
        public async Task CommunityEventContoller_ValidSearch_Returns_200_Ok()
        {
            // Arrange
            _communityEventServiceMock
                .Setup(mock => mock.GetCommunityEvents(It.IsAny<SearchRequest>()))
                .ReturnsAsync(new Result<IList<CommunityEvent>>(new List<CommunityEvent>()));

            var request = new SearchRequest()
            {
                SearchTerm = "Test"
            };

            // Act
            var result = await _communityEventController.Search(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            if (result is OkObjectResult okObjectResult) 
            {
                okObjectResult.StatusCode.Should().Be(200);
            }
        }
    }
}
