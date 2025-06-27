using ProjectManagement.Models.Entities;

namespace ProjectManagement.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetAllTeamsAsync();
        Task<Team?> GetTeamByIdAsync(int id);
        Task<Team> CreateTeamAsync(Team team);
        Task<Team?> UpdateTeamAsync(int id, Team team);
        Task<bool> DeleteTeamAsync(int id);
        Task<TeamMember> AddTeamMemberAsync(TeamMember member);
        Task<bool> RemoveTeamMemberAsync(int memberId);
        Task<bool> AssignTeamToProjectAsync(int teamId, int projectId);
    }
}