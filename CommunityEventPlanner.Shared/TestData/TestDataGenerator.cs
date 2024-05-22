using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CommunityEventPlanner.Shared.TestData
{
    public static class TestDataGenerator
    {
        public static void Generate(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            GenerateTestUsers(context);
            GenerateVenues(context);
            GenerateCommunityEvents(context);
            context.SaveChanges();
        }

        private static void GenerateTestUsers(ApplicationDbContext context)
        {
            var users = new List<ApplicationUser>()
            {
                new ()
                {
                    FirstName = "Test",
                    LastName = "User1",
                    Email = "test.user1@example.com",
                    NormalizedEmail = "TEST.USER1@EXAMPLE.COM",
                    UserName = "TestUser1",
                    NormalizedUserName = "TESTUSER1",
                    PhoneNumber = "+123456789",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new ()
                {
                    FirstName = "Test",
                    LastName = "User2",
                    Email = "test.user2@example.com",
                    NormalizedEmail = "TEST.USER2@EXAMPLE.COM",
                    UserName = "TestUser2",
                    NormalizedUserName = "TESTUSER2",
                    PhoneNumber = "+123456789",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
           };

            foreach (var user in users)
            {
                if (!context.Users.Any(u => u.UserName == user.UserName))
                {
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(user, "secret");
                    user.PasswordHash = hashed;

                    var userStore = new UserStore<ApplicationUser>(context);
                    var result = userStore.CreateAsync(user);
                }
            }
        }

        private static void GenerateVenues(ApplicationDbContext context)
        {
            var venues = new List<Venue>()
            {
                new ()
                {
                    Name = "The Dog and Whistle",
                    Description = "Back room of the local Pub."
                },
                new ()
                {
                    Name = "St Mary's Church Hall",
                    Description = "Entrance on Front Street. Get the key from Geraldine, the Vicar."
                },
                new ()
                {
                    Name = "Sick Parrot Cafe",
                    Description = "Upstairs for the meeting room."
                }
            };

            context.Venues.AddRange(venues);
        }

        private static void GenerateCommunityEvents(ApplicationDbContext context)
        {
            var martialArtsTag = new Tag() { Name = "Martial Arts" };
            var leisureTag = new Tag() { Name = "Leisure" };
            var politicsTag = new Tag() { Name = "Politics" };

            var signUp = new SignUp() { CommunityEventId = 1, UserId = "TestUser2", CreatedDate = DateTime.Now };

            var communityEvents = new List<CommunityEvent>()
            {
                new CommunityEvent()
                {
                    Title = "Book Club",
                    Description = "Discussion of Wuthering Heights",
                    OwnerId = "TestUser1",
                    VenueId = 1,
                    ScheduledDateTime = new DateTime(2024, 5, 28, 12, 0, 0),
                    Duration = TimeSpan.FromMinutes(60),
                    Tags = new List<Tag>() { leisureTag },
                    SignUps = new List<SignUp>(){ signUp }
                },               
                new CommunityEvent()
                {
                    Title = "Cobra-Kai Training Session",
                    Description = "Cobra-Kai Training Session",
                    OwnerId = "TestUser1",
                    VenueId = 2,
                    ScheduledDateTime = new DateTime(2024, 5, 28, 17, 0, 0),
                    Duration = TimeSpan.FromMinutes(60),
                    Tags = new List<Tag>() { martialArtsTag, leisureTag }
                },               
                new CommunityEvent()
                {
                    Title = "Parish Council Meeting",
                    Description = "Update on steps to curb local petunia abuse.",
                    OwnerId = "TestUser2",
                    VenueId = 3,
                    ScheduledDateTime = new DateTime(2024, 5, 28, 12, 0, 0),
                    Duration = TimeSpan.FromMinutes(120),
                    Tags = new List<Tag>() { politicsTag }
                },
                new CommunityEvent()
                {
                    Title = "Cobra-Kai After Session Session",
                    Description = "Cobra-Kai social event",
                    OwnerId = "TestUser1",
                    VenueId = 1,
                    ScheduledDateTime = new DateTime(2024, 5, 28, 19, 0, 0),
                    Duration = TimeSpan.FromMinutes(360),
                    Tags = new List<Tag>() { martialArtsTag, leisureTag }
                }
            };

            context.CommunityEvents.AddRange(communityEvents);
        }
    }
}
