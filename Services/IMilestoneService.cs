using ProjectManagement.Models.Entities;

namespace ProjectManagement.Services
{
    public interface IMilestoneService
    {
        Task<IEnumerable<Milestone>> GetAllMilestonesAsync();
        Task<Milestone?> GetMilestoneByIdAsync(int id);
        Task<Milestone> CreateMilestoneAsync(Milestone milestone);
        Task<Milestone?> UpdateMilestoneAsync(int id, Milestone milestone);
        Task<bool> DeleteMilestoneAsync(int id);
        Task<IEnumerable<Milestone>> GetMilestonesByProjectAsync(int projectId);
    }
}