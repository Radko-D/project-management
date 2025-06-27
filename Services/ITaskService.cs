using ProjectManagement.Models.Entities;
using TaskStatus = ProjectManagement.Models.Entities.TaskStatus;

namespace ProjectManagement.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<TaskItem?> UpdateTaskAsync(int id, TaskItem task);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<TaskItem>> GetTasksByBoardAsync(int boardId);
        Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(TaskStatus status);
        Task<bool> AssignTaskToMemberAsync(int taskId, int memberId);
        Task<bool> AddTagToTaskAsync(int taskId, int tagId);
        Task<bool> RemoveTagFromTaskAsync(int taskId, int tagId);
    }
}