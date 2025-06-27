namespace ProjectManagement.Models.Entities
{
    public class Timeline
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<TimelineEvent> Events { get; set; } = new List<TimelineEvent>();
    }

    public class TimelineEvent
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int TimelineId { get; set; }

        // Navigation properties
        public virtual Timeline Timeline { get; set; } = null!;
    }
}