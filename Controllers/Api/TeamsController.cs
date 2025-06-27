using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models.Entities;
using ProjectManagement.Services;
using ProjectManagement.DTOs;

namespace ProjectManagement.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamResponseDto>>> GetTeams()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            var teamDtos = teams.Select(team => new TeamResponseDto
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                CreatedAt = team.CreatedAt,
                TeamMembers = team.TeamMembers?.Select(tm => new TeamMemberResponseDto
                {
                    Id = tm.Id,
                    Name = tm.Name,
                    Email = tm.Email,
                    Role = tm.Role,
                    TeamId = tm.TeamId,
                    JoinedAt = tm.JoinedAt
                }).ToList() ?? new List<TeamMemberResponseDto>(),
                ProjectTeams = team.ProjectTeams?.Select(pt => new ProjectTeamResponseDto
                {
                    ProjectId = pt.ProjectId,
                    TeamId = pt.TeamId,
                    AssignedAt = pt.AssignedAt,
                    ProjectName = pt.Project?.Name ?? "Unknown"
                }).ToList() ?? new List<ProjectTeamResponseDto>()
            }).ToList();

            return Ok(teamDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponseDto>> GetTeam(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null) return NotFound();

            var teamDto = new TeamResponseDto
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                CreatedAt = team.CreatedAt,
                TeamMembers = team.TeamMembers?.Select(tm => new TeamMemberResponseDto
                {
                    Id = tm.Id,
                    Name = tm.Name,
                    Email = tm.Email,
                    Role = tm.Role,
                    TeamId = tm.TeamId,
                    JoinedAt = tm.JoinedAt
                }).ToList() ?? new List<TeamMemberResponseDto>(),
                ProjectTeams = team.ProjectTeams?.Select(pt => new ProjectTeamResponseDto
                {
                    ProjectId = pt.ProjectId,
                    TeamId = pt.TeamId,
                    AssignedAt = pt.AssignedAt,
                    ProjectName = pt.Project?.Name ?? "Unknown"
                }).ToList() ?? new List<ProjectTeamResponseDto>()
            };

            return Ok(teamDto);
        }

        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam(CreateTeamDto teamDto)
        {
            var team = new Team
            {
                Name = teamDto.Name,
                Description = teamDto.Description
            };

            var createdTeam = await _teamService.CreateTeamAsync(team);
            return CreatedAtAction(nameof(GetTeam), new { id = createdTeam.Id }, createdTeam);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, CreateTeamDto teamDto)
        {
            var team = new Team
            {
                Id = id,
                Name = teamDto.Name,
                Description = teamDto.Description
            };

            var updatedTeam = await _teamService.UpdateTeamAsync(id, team);
            if (updatedTeam == null) return NotFound();
            return Ok(updatedTeam);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var result = await _teamService.DeleteTeamAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("members")]
        public async Task<ActionResult<TeamMember>> AddTeamMember(CreateTeamMemberDto memberDto)
        {
            var member = new TeamMember
            {
                Name = memberDto.Name,
                Email = memberDto.Email,
                Role = memberDto.Role,
                TeamId = memberDto.TeamId,
                JoinedAt = DateTime.UtcNow
            };

            var createdMember = await _teamService.AddTeamMemberAsync(member);
            return Ok(createdMember);
        }

        [HttpDelete("members/{memberId}")]
        public async Task<IActionResult> RemoveTeamMember(int memberId)
        {
            var result = await _teamService.RemoveTeamMemberAsync(memberId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{teamId}/projects/{projectId}")]
        public async Task<IActionResult> AssignTeamToProject(int teamId, int projectId)
        {
            var result = await _teamService.AssignTeamToProjectAsync(teamId, projectId);
            if (!result) return BadRequest("Failed to assign team to project");
            return Ok();
        }
    }
}