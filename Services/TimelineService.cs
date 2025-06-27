using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models.Entities;

namespace ProjectManagement.Services
{
    public class TimelineService : ITimelineService
    {
        private readonly ProjectManagementContext _context;

        public TimelineService(ProjectManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Timeline>> GetAllTimelinesAsync()
        {
            return await _context.Timelines
                .Include(t => t.Project)
                .Include(t => t.Events)
                .ToListAsync();
        }

        public async Task<Timeline?> GetTimelineByIdAsync(int id)
        {
            return await _context.Timelines
                .Include(t => t.Project)
                .Include(t => t.Events)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Timeline> CreateTimelineAsync(Timeline timeline)
        {
            timeline.CreatedAt = DateTime.UtcNow;
            
            _context.Timelines.Add(timeline);
            await _context.SaveChangesAsync();
            return timeline;
        }

        public async Task<Timeline?> UpdateTimelineAsync(int id, Timeline timeline)
        {
            var existingTimeline = await _context.Timelines.FindAsync(id);
            if (existingTimeline == null) return null;

            existingTimeline.Name = timeline.Name;
            existingTimeline.Description = timeline.Description;
            existingTimeline.StartDate = timeline.StartDate;
            existingTimeline.EndDate = timeline.EndDate;

            await _context.SaveChangesAsync();
            return existingTimeline;
        }

        public async Task<bool> DeleteTimelineAsync(int id)
        {
            var timeline = await _context.Timelines.FindAsync(id);
            if (timeline == null) return false;

            _context.Timelines.Remove(timeline);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Timeline>> GetTimelinesByProjectAsync(int projectId)
        {
            return await _context.Timelines
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Events)
                .ToListAsync();
        }

        public async Task<TimelineEvent> AddTimelineEventAsync(int timelineId, TimelineEvent timelineEvent)
        {
            timelineEvent.TimelineId = timelineId;
            
            _context.TimelineEvents.Add(timelineEvent);
            await _context.SaveChangesAsync();
            return timelineEvent;
        }
    }
}