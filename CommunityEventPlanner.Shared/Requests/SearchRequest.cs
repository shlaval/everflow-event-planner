namespace CommunityEventPlanner.Shared.Requests
{
    public class SearchRequest
    {
        public string? SearchTerm { get; set; }
        public int? VenueId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
    }
}
