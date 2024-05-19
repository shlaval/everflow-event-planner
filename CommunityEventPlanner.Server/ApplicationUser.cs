using CommunityEventPlanner.Shared;
using Microsoft.AspNetCore.Identity;

namespace CommunityEventPlanner.Server
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<CommunityEvent> SignedUpEvents { get; set; } = new List<CommunityEvent>();
    }
}
