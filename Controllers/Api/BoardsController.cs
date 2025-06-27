using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models.Entities;
using ProjectManagement.Services;
using ProjectManagement.DTOs;

namespace ProjectManagement.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly IProjectService _projectService;

        public BoardsController(IBoardService boardService, IProjectService projectService)
        {
            _boardService = boardService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardResponseDto>>> GetBoards()
        {
            var boards = await _boardService.GetAllBoardsAsync();
            var boardDtos = boards.Select(board => new BoardResponseDto
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                ProjectId = board.ProjectId,
                CreatedAt = board.CreatedAt,
                Project = board.Project != null ? new ProjectSummaryDto
                {
                    Id = board.Project.Id,
                    Name = board.Project.Name,
                    Status = board.Project.Status.ToString()
                } : null,
                Tasks = board.Tasks?.Select(task => new TaskSummaryDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Status = task.Status.ToString(),
                    Priority = task.Priority.ToString()
                }).ToList() ?? new List<TaskSummaryDto>()
            });

            return Ok(boardDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardResponseDto>> GetBoard(int id)
        {
            var board = await _boardService.GetBoardByIdAsync(id);
            if (board == null) return NotFound();

            var boardDto = new BoardResponseDto
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                ProjectId = board.ProjectId,
                CreatedAt = board.CreatedAt,
                Project = board.Project != null ? new ProjectSummaryDto
                {
                    Id = board.Project.Id,
                    Name = board.Project.Name,
                    Status = board.Project.Status.ToString()
                } : null,
                Tasks = board.Tasks?.Select(task => new TaskSummaryDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Status = task.Status.ToString(),
                    Priority = task.Priority.ToString()
                }).ToList() ?? new List<TaskSummaryDto>()
            };

            return Ok(boardDto);
        }

        [HttpPost]
        public async Task<ActionResult<BoardResponseDto>> CreateBoard(CreateBoardDto boardDto)
        {
            
            var project = await _projectService.GetProjectByIdAsync(boardDto.ProjectId);
            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var board = new Board
            {
                Name = boardDto.Name,
                Description = boardDto.Description,
                ProjectId = boardDto.ProjectId,
                CreatedAt = DateTime.UtcNow
            };

            var createdBoard = await _boardService.CreateBoardAsync(board);
            
            
            var responseDto = new BoardResponseDto
            {
                Id = createdBoard.Id,
                Name = createdBoard.Name,
                Description = createdBoard.Description,
                ProjectId = createdBoard.ProjectId,
                CreatedAt = createdBoard.CreatedAt,
                Project = new ProjectSummaryDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Status = project.Status.ToString()
                },
                Tasks = new List<TaskSummaryDto>()
            };

            return CreatedAtAction(nameof(GetBoard), new { id = createdBoard.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoard(int id, UpdateBoardDto boardDto)
        {
            var existingBoard = await _boardService.GetBoardByIdAsync(id);
            if (existingBoard == null) return NotFound();

            var board = new Board
            {
                Id = id,
                Name = boardDto.Name,
                Description = boardDto.Description,
                ProjectId = existingBoard.ProjectId, 
                CreatedAt = existingBoard.CreatedAt
            };

            var updatedBoard = await _boardService.UpdateBoardAsync(id, board);
            if (updatedBoard == null) return NotFound();
            
            return Ok(updatedBoard);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var result = await _boardService.DeleteBoardAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<BoardResponseDto>>> GetBoardsByProject(int projectId)
        {
            var boards = await _boardService.GetBoardsByProjectAsync(projectId);
            var boardDtos = boards.Select(board => new BoardResponseDto
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
            });

            return Ok(boardDtos);
        }
    }
}