using CommunityEventPlanner.Server.Logic;
using CommunityEventPlanner.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CommunityEventPlanner.Tests.TestFixtures
{
    public class CommunityEventServiceTestFixture
    {
        private ServiceProvider _serviceProvider;

        public CommunityEventServiceTestFixture()
        {
            _serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(_serviceProvider);

            ApplicationDbContext = new ApplicationDbContext(builder.Options);
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            UserManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
            CommunityEventService = new CommunityEventService(ApplicationDbContext, UserManagerMock.Object, HttpContextAccessorMock.Object);
        }

        public ApplicationDbContext ApplicationDbContext { get; private set; }
        public Mock<UserManager<ApplicationUser>> UserManagerMock { get; private set; }
        public Mock<IHttpContextAccessor> HttpContextAccessorMock { get; private set; }
        public ICommunityEventService CommunityEventService { get; private set; }
    }
}
