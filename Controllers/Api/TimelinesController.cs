using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models.Entities;
using ProjectManagement.Services;
using ProjectManagement.DTOs;

namespace ProjectManagement.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimelinesController : ControllerBase
    {
        private readonly ITimelineService _timelineService;
        private readonly IProjectService _projectService;

        public TimelinesController(ITimelineService timelineService, IProjectService projectService)
        {
            _timelineService = timelineService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimelineResponseDto>>> GetTimelines()
        {
            var timelines = await _timelineService.GetAllTimelinesAsync();
            var timelineDtos = timelines.Select(MapToResponseDto);
            return Ok(timelineDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TimelineResponseDto>> GetTimeline(int id)
        {
            var timeline = await _timelineService.GetTimelineByIdAsync(id);
            if (timeline == null) return NotFound();
            
            var timelineDto = MapToResponseDto(timeline);
            return Ok(timelineDto);
        }

        [HttpPost]
        public async Task<ActionResult<TimelineResponseDto>> CreateTimeline(CreateTimelineDto timelineDto)
        {

            var project = await _projectService.GetProjectByIdAsync(timelineDto.ProjectId);
            if (project == null)
            {
                return BadRequest("Project not found");
            }


            if (!DateTime.TryParse(timelineDto.StartDate, out var startDate))
            {
                return BadRequest("Invalid start date format");
            }

            if (!DateTime.TryParse(timelineDto.EndDate, out var endDate))
            {
                return BadRequest("Invalid end date format");
            }

            if (endDate <= startDate)
            {
                return BadRequest("End date must be after start date");
            }

            var timeline = new Timeline
            {
                Name = timelineDto.Name,
                Description = timelineDto.Description,
                StartDate = startDate,
                EndDate = endDate,
                ProjectId = timelineDto.ProjectId,
                CreatedAt = DateTime.UtcNow
            };

            var createdTimeline = await _timelineService.CreateTimelineAsync(timeline);
            var responseDto = MapToResponseDto(createdTimeline);
            
            return CreatedAtAction(nameof(GetTimeline), new { id = createdTimeline.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeline(int id, UpdateTimelineDto timelineDto)
        {

            if (!DateTime.TryParse(timelineDto.StartDate, out var startDate))
            {
                return BadRequest("Invalid start date format");
            }

            if (!DateTime.TryParse(timelineDto.EndDate, out var endDate))
            {
                return BadRequest("Invalid end date format");
            }

            if (endDate <= startDate)
            {
                return BadRequest("End date must be after start date");
            }

            var timeline = new Timeline
            {
                Id = id,
                Name = timelineDto.Name,
                Description = timelineDto.Description,
                StartDate = startDate,
                EndDate = endDate
            };

            var updatedTimeline = await _timelineService.UpdateTimelineAsync(id, timeline);
            if (updatedTimeline == null) return NotFound();
            
            return Ok(MapToResponseDto(updatedTimeline));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeline(int id)
        {
            var result = await _timelineService.DeleteTimelineAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<TimelineResponseDto>>> GetTimelinesByProject(int projectId)
        {
            var timelines = await _timelineService.GetTimelinesByProjectAsync(projectId);
            var timelineDtos = timelines.Select(MapToResponseDto);
            return Ok(timelineDtos);
        }

        [HttpPost("{timelineId}/events")]
        public async Task<ActionResult<TimelineEventDto>> AddTimelineEvent(int timelineId, CreateTimelineEventDto eventDto)
        {

            var timeline = await _timelineService.GetTimelineByIdAsync(timelineId);
            if (timeline == null)
            {
                return BadRequest("Timeline not found");
            }


            if (!DateTime.TryParse(eventDto.EventDate, out var eventDate))
            {
                return BadRequest("Invalid event date format");
            }


            if (eventDate < timeline.StartDate || eventDate > timeline.EndDate)
            {
                return BadRequest("Event date must be within timeline start and end dates");
            }

            var timelineEvent = new TimelineEvent
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                EventDate = eventDate,
                TimelineId = timelineId
            };

            var createdEvent = await _timelineService.AddTimelineEventAsync(timelineId, timelineEvent);
            
            var eventResponseDto = new TimelineEventDto
            {
                Id = createdEvent.Id,
                Title = createdEvent.Title,
                Description = createdEvent.Description,
                EventDate = createdEvent.EventDate,
                TimelineId = createdEvent.TimelineId
            };

            return Ok(eventResponseDto);
        }

        private static TimelineResponseDto MapToResponseDto(Timeline timeline)
        {
            return new TimelineResponseDto
            {
                Id = timeline.Id,
                Name = timeline.Name,
                Description = timeline.Description,
                StartDate = timeline.StartDate,
                EndDate = timeline.EndDate,
                ProjectId = timeline.ProjectId,
                CreatedAt = timeline.CreatedAt,
                Project = timeline.Project != null ? new ProjectSummaryDto
                {
                    Id = timeline.Project.Id,
                    Name = timeline.Project.Name,
                    Status = timeline.Project.Status.ToString(),
                    StartDate = timeline.Project.StartDate,
                    EndDate = timeline.Project.EndDate
                } : null,
                Events = timeline.Events?.Select(e => new TimelineEventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    TimelineId = e.TimelineId
                }).OrderBy(e => e.EventDate).ToList() ?? new List<TimelineEventDto>()
            };
        }
    }
}