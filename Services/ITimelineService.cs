using ProjectManagement.Models.Entities;

namespace ProjectManagement.Services
{
    public interface ITimelineService
    {
        Task<IEnumerable<Timeline>> GetAllTimelinesAsync();
        Task<Timeline?> GetTimelineByIdAsync(int id);
        Task<Timeline> CreateTimelineAsync(Timeline timeline);
        Task<Timeline?> UpdateTimelineAsync(int id, Timeline timeline);
        Task<bool> DeleteTimelineAsync(int id);
        Task<IEnumerable<Timeline>> GetTimelinesByProjectAsync(int projectId);
        Task<TimelineEvent> AddTimelineEventAsync(int timelineId, TimelineEvent timelineEvent);
    }
}