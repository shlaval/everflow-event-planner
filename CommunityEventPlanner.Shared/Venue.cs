using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CommunityEventPlanner.Shared
{
    public class Venue
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual ICollection<CommunityEvent> CommunityEvents { get; set; } = new List<CommunityEvent>();
    }
}
