using System.ComponentModel.DataAnnotations;

namespace CommunityEventPlanner.Shared
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<CommunityEvent> CommunityEvents { get; set;} = new List<CommunityEvent>();
    }
}