using System.ComponentModel.DataAnnotations;

namespace CommunityEventPlanner.Shared
{
    public class CommunityEvent
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        public string OwnerId { get; set; } = string.Empty;

        public int VenueId { get; set; }
        public virtual Venue? Venue { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public DateTime ScheduledDateTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
