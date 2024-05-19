using Microsoft.AspNetCore.Identity;

namespace CommunityEventPlanner.Shared
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<CommunityEvent> SignedUpEvents { get; set; } = new List<CommunityEvent>();
    }
}
