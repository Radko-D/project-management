namespace ProjectManagement.Models.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "#000000";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }

    public class TaskTag
    {
        public int TaskId { get; set; }
        public int TagId { get; set; }

        // Navigation properties
        public virtual TaskItem Task { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}