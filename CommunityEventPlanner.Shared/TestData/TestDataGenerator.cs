using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CommunityEventPlanner.Shared.TestData
{
    public static class TestDataGenerator
    {
        public static void Generate(ApplicationDbContext context)
        {
            var user = new ApplicationUser
            {
                FirstName = "XXXX",
                LastName = "XXXX",
                Email = "xxxx@example.com",
                NormalizedEmail = "XXXX@EXAMPLE.COM",
                UserName = "Owner",
                NormalizedUserName = "OWNER",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


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
}
