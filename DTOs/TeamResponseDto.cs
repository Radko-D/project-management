namespace ProjectManagement.DTOs
{
    public class TeamResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<TeamMemberResponseDto> TeamMembers { get; set; } = new();
        public List<ProjectTeamResponseDto> ProjectTeams { get; set; } = new();
    }

    public class TeamMemberResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public DateTime JoinedAt { get; set; }
    }

    public class ProjectTeamResponseDto
    {
        public int ProjectId { get; set; }
        public int TeamId { get; set; }
        public DateTime AssignedAt { get; set; }
        public string ProjectName { get; set; } = string.Empty;
    }
}