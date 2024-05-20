using Microsoft.AspNetCore.Identity;

namespace CommunityEventPlanner.Shared
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public virtual ICollection<CommunityEvent> SignedUpEvents { get; set; } = new List<CommunityEvent>();
    }
}
