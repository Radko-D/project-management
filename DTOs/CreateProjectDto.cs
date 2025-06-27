using ProjectManagement.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskStatus = ProjectManagement.Models.Entities.TaskStatus;

namespace ProjectManagement.DTOs
{
    // Project DTOs

        public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class UpdateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Planning";
    }

    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Summary collections to avoid deep nesting
        public List<BoardSummaryDto> Boards { get; set; } = new();
        public List<ProjectTeamSummaryDto> ProjectTeams { get; set; } = new();
        public List<MilestoneSummaryDto> Milestones { get; set; } = new();
        public List<TimelineSummaryDto> Timelines { get; set; } = new();
        
        // Calculated properties
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double ProgressPercentage { get; set; }
    }

    public class ProjectSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ProjectTeamSummaryDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public DateTime AssignedAt { get; set; }
    }

    public class TimelineSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EventCount { get; set; }
    }


    // Team DTOs
    public class CreateTeamDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateTeamMemberDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty;
        
        [Required]
        public int TeamId { get; set; }
    }

    public class UpdateTeamMemberDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty;
    }

    // Timeline DTOs
    public class CreateTimelineDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty; // Use string for easy parsing
        public string EndDate { get; set; } = string.Empty;
        public int ProjectId { get; set; }
    }

    public class UpdateTimelineDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
    }

    public class TimelineResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Related entities
        public ProjectSummaryDto? Project { get; set; }
        public List<TimelineEventDto> Events { get; set; } = new();
        
        // Calculated properties
        public int DurationDays => (EndDate - StartDate).Days;
        public int EventCount => Events.Count;
        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    }

    public class TimelineEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int TimelineId { get; set; }
    }

    public class CreateTimelineEventDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string EventDate { get; set; } = string.Empty;
    }

    // Tag DTOs
    public class CreateTagDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Color { get; set; } = "#000000";
    }

    public class UpdateTagDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Color { get; set; } = "#000000";
    }

    // Comment DTOs
    public class CreateCommentDto
    {
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public string AuthorName { get; set; } = string.Empty;
        
        [Required]
        public int TaskId { get; set; }
    }

     public class CreateBoardDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
    }

    public class UpdateBoardDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class BoardResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ProjectSummaryDto? Project { get; set; }
        public List<TaskSummaryDto> Tasks { get; set; } = new();
    }

    public class TaskSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
    }

    public class CreateMilestoneDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty; // Use string for easy parsing
        public bool IsCompleted { get; set; } = false;
        public int ProjectId { get; set; }
    }

    public class UpdateMilestoneDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class MilestoneResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Related entities
        public ProjectSummaryDto? Project { get; set; }
        public List<TaskSummaryDto> Tasks { get; set; } = new();
        
        // Calculated properties
        public bool IsOverdue => !IsCompleted && DueDate < DateTime.UtcNow;
        public int TaskCount => Tasks.Count;
        public int CompletedTaskCount => Tasks.Count(t => t.Status == "Done");
    }
}