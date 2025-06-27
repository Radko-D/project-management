using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models.Entities;
using ProjectManagement.Services;
using ProjectManagement.DTOs;

namespace ProjectManagement.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MilestonesController : ControllerBase
    {
        private readonly IMilestoneService _milestoneService;
        private readonly IProjectService _projectService;

        public MilestonesController(IMilestoneService milestoneService, IProjectService projectService)
        {
            _milestoneService = milestoneService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MilestoneResponseDto>>> GetMilestones()
        {
            var milestones = await _milestoneService.GetAllMilestonesAsync();
            var milestoneDtos = milestones.Select(MapToResponseDto);
            return Ok(milestoneDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MilestoneResponseDto>> GetMilestone(int id)
        {
            var milestone = await _milestoneService.GetMilestoneByIdAsync(id);
            if (milestone == null) return NotFound();
            
            var milestoneDto = MapToResponseDto(milestone);
            return Ok(milestoneDto);
        }

        [HttpPost]
        public async Task<ActionResult<MilestoneResponseDto>> CreateMilestone(CreateMilestoneDto milestoneDto)
        {
            // Validate project exists
            var project = await _projectService.GetProjectByIdAsync(milestoneDto.ProjectId);
            if (project == null)
            {
                return BadRequest("Project not found");
            }

            // Parse due date
            if (!DateTime.TryParse(milestoneDto.DueDate, out var dueDate))
            {
                return BadRequest("Invalid due date format");
            }

            var milestone = new Milestone
            {
                Name = milestoneDto.Name,
                Description = milestoneDto.Description,
                DueDate = dueDate,
                IsCompleted = milestoneDto.IsCompleted,
                ProjectId = milestoneDto.ProjectId,
                CreatedAt = DateTime.UtcNow
            };

            var createdMilestone = await _milestoneService.CreateMilestoneAsync(milestone);
            var responseDto = MapToResponseDto(createdMilestone);
            
            return CreatedAtAction(nameof(GetMilestone), new { id = createdMilestone.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMilestone(int id, UpdateMilestoneDto milestoneDto)
        {
            // Parse due date
            if (!DateTime.TryParse(milestoneDto.DueDate, out var dueDate))
            {
                return BadRequest("Invalid due date format");
            }

            var milestone = new Milestone
            {
                Id = id,
                Name = milestoneDto.Name,
                Description = milestoneDto.Description,
                DueDate = dueDate,
                IsCompleted = milestoneDto.IsCompleted
            };

            var updatedMilestone = await _milestoneService.UpdateMilestoneAsync(id, milestone);
            if (updatedMilestone == null) return NotFound();
            
            return Ok(MapToResponseDto(updatedMilestone));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMilestone(int id)
        {
            var result = await _milestoneService.DeleteMilestoneAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<MilestoneResponseDto>>> GetMilestonesByProject(int projectId)
        {
            var milestones = await _milestoneService.GetMilestonesByProjectAsync(projectId);
            var milestoneDtos = milestones.Select(MapToResponseDto);
            return Ok(milestoneDtos);
        }

        private static MilestoneResponseDto MapToResponseDto(Milestone milestone)
        {
            return new MilestoneResponseDto
            {
                Id = milestone.Id,
                Name = milestone.Name,
                Description = milestone.Description,
                DueDate = milestone.DueDate,
                IsCompleted = milestone.IsCompleted,
                ProjectId = milestone.ProjectId,
                CreatedAt = milestone.CreatedAt,
                Project = milestone.Project != null ? new ProjectSummaryDto
                {
                    Id = milestone.Project.Id,
                    Name = milestone.Project.Name,
                    Status = milestone.Project.Status.ToString(),
                    StartDate = milestone.Project.StartDate,
                    EndDate = milestone.Project.EndDate
                } : null,
                Tasks = milestone.Tasks?.Select(task => new TaskSummaryDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Status = task.Status.ToString(),
                    Priority = task.Priority.ToString()
                }).ToList() ?? new List<TaskSummaryDto>()
            };
        }
    }
}