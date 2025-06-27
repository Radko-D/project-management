using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models.Entities;

namespace ProjectManagement.Services
{
    public class MilestoneService : IMilestoneService
    {
        private readonly ProjectManagementContext _context;

        public MilestoneService(ProjectManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Milestone>> GetAllMilestonesAsync()
        {
            return await _context.Milestones
                .Include(m => m.Project)
                .Include(m => m.Tasks)
                .ToListAsync();
        }

        public async Task<Milestone?> GetMilestoneByIdAsync(int id)
        {
            return await _context.Milestones
                .Include(m => m.Project)
                .Include(m => m.Tasks)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Milestone> CreateMilestoneAsync(Milestone milestone)
        {
            milestone.CreatedAt = DateTime.UtcNow;
            
            _context.Milestones.Add(milestone);
            await _context.SaveChangesAsync();
            return milestone;
        }

        public async Task<Milestone?> UpdateMilestoneAsync(int id, Milestone milestone)
        {
            var existingMilestone = await _context.Milestones.FindAsync(id);
            if (existingMilestone == null) return null;

            existingMilestone.Name = milestone.Name;
            existingMilestone.Description = milestone.Description;
            existingMilestone.DueDate = milestone.DueDate;
            existingMilestone.IsCompleted = milestone.IsCompleted;

            await _context.SaveChangesAsync();
            return existingMilestone;
        }

        public async Task<bool> DeleteMilestoneAsync(int id)
        {
            var milestone = await _context.Milestones.FindAsync(id);
            if (milestone == null) return false;

            _context.Milestones.Remove(milestone);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Milestone>> GetMilestonesByProjectAsync(int projectId)
        {
            return await _context.Milestones
                .Where(m => m.ProjectId == projectId)
                .Include(m => m.Tasks)
                .ToListAsync();
        }
    }
}