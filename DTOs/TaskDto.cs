namespace ProjectManagement.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium"; // Use string for easy frontend handling
        public string? DueDate { get; set; } // Use string for date handling
        public int? EstimatedHours { get; set; }
        public int BoardId { get; set; }
        public int? MilestoneId { get; set; }
    }

    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "ToDo";
        public string? DueDate { get; set; }
        public int? EstimatedHours { get; set; }
        public int? ActualHours { get; set; }
        public int? MilestoneId { get; set; }
    }

    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int? EstimatedHours { get; set; }
        public int? ActualHours { get; set; }
        public int BoardId { get; set; }
        public int? MilestoneId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Related entities
        public BoardSummaryDto? Board { get; set; }
        public MilestoneSummaryDto? Milestone { get; set; }
        public List<TaskTagDto> TaskTags { get; set; } = new();
        public List<TaskAssignmentDto> Assignments { get; set; } = new();
        public List<CommentDto> Comments { get; set; } = new();
    }

    public class BoardSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ProjectSummaryDto? Project { get; set; }
        public List<TaskSummaryDto> Tasks { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
    }

    public class MilestoneSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TaskTagDto
    {
        public int TagId { get; set; }
        public TagDto? Tag { get; set; }
    }

    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

    public class TaskAssignmentDto
    {
        public int Id { get; set; }
        public int TeamMemberId { get; set; }
        public DateTime AssignedAt { get; set; }
        public TeamMemberSummaryDto? TeamMember { get; set; }
    }

    public class TeamMemberSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}