using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models.Entities;
using ProjectManagement.Services;
using ProjectManagement.DTOs;
using TaskStatus = ProjectManagement.Models.Entities.TaskStatus;

namespace ProjectManagement.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var projectDtos = projects.Select(MapToResponseDto);
            return Ok(projectDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            
            var projectDto = MapToResponseDto(project);
            return Ok(projectDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> CreateProject(CreateProjectDto projectDto)
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                Status = ProjectStatus.Planning,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdProject = await _projectService.CreateProjectAsync(project);
            var responseDto = MapToResponseDto(createdProject);
            
            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto projectDto)
        {
            // Parse and validate status
            if (!Enum.TryParse<ProjectStatus>(projectDto.Status, out var status))
            {
                return BadRequest($"Invalid status value: {projectDto.Status}");
            }

            var project = new Project
            {
                Id = id,
                Name = projectDto.Name,
                Description = projectDto.Description,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                Status = status,
                UpdatedAt = DateTime.UtcNow
            };

            var updatedProject = await _projectService.UpdateProjectAsync(id, project);
            if (updatedProject == null) return NotFound();
            
            return Ok(MapToResponseDto(updatedProject));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetProjectsByStatus(string status)
        {
            if (!Enum.TryParse<ProjectStatus>(status, out var projectStatus))
            {
                return BadRequest($"Invalid status value: {status}");
            }

            var projects = await _projectService.GetProjectsByStatusAsync(projectStatus);
            var projectDtos = projects.Select(MapToResponseDto);
            return Ok(projectDtos);
        }

        private static ProjectResponseDto MapToResponseDto(Project project)
        {
            // Calculate task statistics
            var totalTasks = 0;
            var completedTasks = 0;
            
            if (project.Boards != null)
            {
                foreach (var board in project.Boards)
                {
                    if (board.Tasks != null)
                    {
                        totalTasks += board.Tasks.Count;
                        completedTasks += board.Tasks.Count(t => t.Status == TaskStatus.Done);
                    }
                }
            }

            var progressPercentage = totalTasks == 0 ? 0 : Math.Round((double)completedTasks / totalTasks * 100, 1);

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status.ToString(),
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                ProgressPercentage = progressPercentage,
                Boards = project.Boards?.Select(board => new BoardSummaryDto
                {
                    Id = board.Id,
                    Name = board.Name,
                    Description = board.Description,
                    ProjectId = board.ProjectId,
                    CreatedAt = board.CreatedAt,
                    Tasks = board.Tasks?.Select(task => new TaskSummaryDto
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Status = task.Status.ToString(),
                        Priority = task.Priority.ToString()
                    }).ToList() ?? new List<TaskSummaryDto>()
                }).ToList() ?? new List<BoardSummaryDto>(),
                ProjectTeams = project.ProjectTeams?.Select(pt => new ProjectTeamSummaryDto
                {
                    TeamId = pt.TeamId,
                    TeamName = pt.Team?.Name ?? "Unknown Team",
                    MemberCount = pt.Team?.TeamMembers?.Count ?? 0,
                    AssignedAt = pt.AssignedAt
                }).ToList() ?? new List<ProjectTeamSummaryDto>(),
                Milestones = project.Milestones?.Select(milestone => new MilestoneSummaryDto
                {
                    Id = milestone.Id,
                    Name = milestone.Name,
                    DueDate = milestone.DueDate,
                    IsCompleted = milestone.IsCompleted
                }).ToList() ?? new List<MilestoneSummaryDto>(),
                Timelines = project.Timelines?.Select(timeline => new TimelineSummaryDto
                {
                    Id = timeline.Id,
                    Name = timeline.Name,
                    StartDate = timeline.StartDate,
                    EndDate = timeline.EndDate,
                    EventCount = timeline.Events?.Count ?? 0
                }).ToList() ?? new List<TimelineSummaryDto>()
            };
        }
    }
}