using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models.Entities;

namespace ProjectManagement.Services
{
    public class TeamService : ITeamService
    {
        private readonly ProjectManagementContext _context;

        public TeamService(ProjectManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams
                .Include(t => t.TeamMembers)
                .Include(t => t.ProjectTeams)
                    .ThenInclude(pt => pt.Project)
                .ToListAsync();
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.TeamMembers)
                .Include(t => t.ProjectTeams)
                    .ThenInclude(pt => pt.Project)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Team> CreateTeamAsync(Team team)
        {
            team.CreatedAt = DateTime.UtcNow;
            
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<Team?> UpdateTeamAsync(int id, Team team)
        {
            var existingTeam = await _context.Teams.FindAsync(id);
            if (existingTeam == null) return null;

            existingTeam.Name = team.Name;
            existingTeam.Description = team.Description;

            await _context.SaveChangesAsync();
            return existingTeam;
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return false;

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TeamMember> AddTeamMemberAsync(TeamMember member)
        {
            member.JoinedAt = DateTime.UtcNow;
            
            _context.TeamMembers.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<bool> RemoveTeamMemberAsync(int memberId)
        {
            var member = await _context.TeamMembers.FindAsync(memberId);
            if (member == null) return false;

            _context.TeamMembers.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignTeamToProjectAsync(int teamId, int projectId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            var project = await _context.Projects.FindAsync(projectId);
            
            if (team == null || project == null) return false;

            var existingAssignment = await _context.ProjectTeams
                .FirstOrDefaultAsync(pt => pt.ProjectId == projectId && pt.TeamId == teamId);
            
            if (existingAssignment != null) return true; // Already assigned

            var projectTeam = new ProjectTeam
            {
                ProjectId = projectId,
                TeamId = teamId,
                AssignedAt = DateTime.UtcNow
            };

            _context.ProjectTeams.Add(projectTeam);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}