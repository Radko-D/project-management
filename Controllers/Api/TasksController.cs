using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models.Entities;
using ProjectManagement.Services;
using ProjectManagement.DTOs;
using TaskStatus = ProjectManagement.Models.Entities.TaskStatus;

namespace ProjectManagement.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IBoardService _boardService;

        public TasksController(ITaskService taskService, IBoardService boardService)
        {
            _taskService = taskService;
            _boardService = boardService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            var taskDtos = tasks.Select(MapToResponseDto);
            return Ok(taskDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            
            var taskDto = MapToResponseDto(task);
            return Ok(taskDto);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto taskDto)
        {
            // Validate board exists
            var board = await _boardService.GetBoardByIdAsync(taskDto.BoardId);
            if (board == null)
            {
                return BadRequest("Board not found");
            }

            // Parse and validate priority
            if (!Enum.TryParse<TaskPriority>(taskDto.Priority, out var priority))
            {
                return BadRequest($"Invalid priority value: {taskDto.Priority}");
            }

            // Parse due date
            DateTime? dueDate = null;
            if (!string.IsNullOrEmpty(taskDto.DueDate))
            {
                if (!DateTime.TryParse(taskDto.DueDate, out var parsedDate))
                {
                    return BadRequest("Invalid due date format");
                }
                dueDate = parsedDate;
            }

            var task = new TaskItem
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Priority = priority,
                Status = TaskStatus.ToDo, // Default status
                DueDate = dueDate,
                EstimatedHours = taskDto.EstimatedHours,
                BoardId = taskDto.BoardId,
                MilestoneId = taskDto.MilestoneId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdTask = await _taskService.CreateTaskAsync(task);
            var responseDto = MapToResponseDto(createdTask);
            
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto taskDto)
        {
            // Parse and validate priority
            if (!Enum.TryParse<TaskPriority>(taskDto.Priority, out var priority))
            {
                return BadRequest($"Invalid priority value: {taskDto.Priority}");
            }

            // Parse and validate status
            if (!Enum.TryParse<TaskStatus>(taskDto.Status, out var status))
            {
                return BadRequest($"Invalid status value: {taskDto.Status}");
            }

            // Parse due date
            DateTime? dueDate = null;
            if (!string.IsNullOrEmpty(taskDto.DueDate))
            {
                if (!DateTime.TryParse(taskDto.DueDate, out var parsedDate))
                {
                    return BadRequest("Invalid due date format");
                }
                dueDate = parsedDate;
            }

            var task = new TaskItem
            {
                Id = id,
                Title = taskDto.Title,
                Description = taskDto.Description,
                Priority = priority,
                Status = status,
                DueDate = dueDate,
                EstimatedHours = taskDto.EstimatedHours,
                ActualHours = taskDto.ActualHours,
                MilestoneId = taskDto.MilestoneId,
                UpdatedAt = DateTime.UtcNow
            };

            var updatedTask = await _taskService.UpdateTaskAsync(id, task);
            if (updatedTask == null) return NotFound();
            
            return Ok(MapToResponseDto(updatedTask));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("board/{boardId}")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasksByBoard(int boardId)
        {
            var tasks = await _taskService.GetTasksByBoardAsync(boardId);
            var taskDtos = tasks.Select(MapToResponseDto);
            return Ok(taskDtos);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasksByStatus(string status)
        {
            if (!Enum.TryParse<TaskStatus>(status, out var taskStatus))
            {
                return BadRequest($"Invalid status value: {status}");
            }

            var tasks = await _taskService.GetTasksByStatusAsync(taskStatus);
            var taskDtos = tasks.Select(MapToResponseDto);
            return Ok(taskDtos);
        }

        [HttpPost("{taskId}/assign/{memberId}")]
        public async Task<IActionResult> AssignTaskToMember(int taskId, int memberId)
        {
            var result = await _taskService.AssignTaskToMemberAsync(taskId, memberId);
            if (!result) return BadRequest("Failed to assign task to member");
            return Ok();
        }

        [HttpPost("{taskId}/tags/{tagId}")]
        public async Task<IActionResult> AddTagToTask(int taskId, int tagId)
        {
            var result = await _taskService.AddTagToTaskAsync(taskId, tagId);
            if (!result) return BadRequest("Failed to add tag to task");
            return Ok();
        }

        [HttpDelete("{taskId}/tags/{tagId}")]
        public async Task<IActionResult> RemoveTagFromTask(int taskId, int tagId)
        {
            var result = await _taskService.RemoveTagFromTaskAsync(taskId, tagId);
            if (!result) return BadRequest("Failed to remove tag from task");
            return Ok();
        }

        private static TaskResponseDto MapToResponseDto(TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority.ToString(),
                Status = task.Status.ToString(),
                DueDate = task.DueDate,
                EstimatedHours = task.EstimatedHours,
                ActualHours = task.ActualHours,
                BoardId = task.BoardId,
                MilestoneId = task.MilestoneId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                Board = task.Board != null ? new BoardSummaryDto
                {
                    Id = task.Board.Id,
                    Name = task.Board.Name,
                    Project = task.Board.Project != null ? new ProjectSummaryDto
                    {
                        Id = task.Board.Project.Id,
                        Name = task.Board.Project.Name,
                        Status = task.Board.Project.Status.ToString()
                    } : null
                } : null,
                Milestone = task.Milestone != null ? new MilestoneSummaryDto
                {
                    Id = task.Milestone.Id,
                    Name = task.Milestone.Name,
                    DueDate = task.Milestone.DueDate,
                    IsCompleted = task.Milestone.IsCompleted
                } : null,
                TaskTags = task.TaskTags?.Select(tt => new TaskTagDto
                {
                    TagId = tt.TagId,
                    Tag = tt.Tag != null ? new TagDto
                    {
                        Id = tt.Tag.Id,
                        Name = tt.Tag.Name,
                        Color = tt.Tag.Color
                    } : null
                }).ToList() ?? new List<TaskTagDto>(),
                Assignments = task.Assignments?.Select(a => new TaskAssignmentDto
                {
                    Id = a.Id,
                    TeamMemberId = a.TeamMemberId,
                    AssignedAt = a.AssignedAt,
                    TeamMember = a.TeamMember != null ? new TeamMemberSummaryDto
                    {
                        Id = a.TeamMember.Id,
                        Name = a.TeamMember.Name,
                        Role = a.TeamMember.Role
                    } : null
                }).ToList() ?? new List<TaskAssignmentDto>(),
                Comments = task.Comments?.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    AuthorName = c.AuthorName,
                    CreatedAt = c.CreatedAt
                }).ToList() ?? new List<CommentDto>()
            };
        }
    }
}