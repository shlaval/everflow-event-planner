using System.ComponentModel.DataAnnotations;

namespace CommunityEventPlanner.Shared.Requests
{
    public class CommunityEventCreateRequest
    {
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int VenueId { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();
        public DateTime ScheduledDateTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
