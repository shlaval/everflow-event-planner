using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CommunityEventPlanner.Shared
{
    public class SignUp
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CommunityEventId { get; set; }

        [JsonIgnore]
        public virtual CommunityEvent CommunityEvent { get; set; }
    }
}
