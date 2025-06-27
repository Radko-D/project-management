using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models.Entities;
using TaskStatus = ProjectManagement.Models.Entities.TaskStatus;

namespace ProjectManagement.Services
{
    public class TaskService : ITaskService
    {
        private readonly ProjectManagementContext _context;

        public TaskService(ProjectManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.Board)
                .Include(t => t.Milestone)
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Include(t => t.Assignments)
                    .ThenInclude(a => a.TeamMember)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Board)
                .Include(t => t.Milestone)
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Include(t => t.Assignments)
                    .ThenInclude(a => a.TeamMember)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem?> UpdateTaskAsync(int id, TaskItem task)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null) return null;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Priority = task.Priority;
            existingTask.Status = task.Status;
            existingTask.DueDate = task.DueDate;
            existingTask.EstimatedHours = task.EstimatedHours;
            existingTask.ActualHours = task.ActualHours;
            existingTask.MilestoneId = task.MilestoneId;
            existingTask.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByBoardAsync(int boardId)
        {
            return await _context.Tasks
                .Where(t => t.BoardId == boardId)
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Include(t => t.Assignments)
                    .ThenInclude(a => a.TeamMember)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(TaskStatus status)
        {
            return await _context.Tasks
                .Where(t => t.Status == status)
                .Include(t => t.Board)
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .ToListAsync();
        }

        public async Task<bool> AssignTaskToMemberAsync(int taskId, int memberId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            var member = await _context.TeamMembers.FindAsync(memberId);
            
            if (task == null || member == null) return false;

            var existingAssignment = await _context.TaskAssignments
                .FirstOrDefaultAsync(ta => ta.TaskId == taskId && ta.TeamMemberId == memberId);
            
            if (existingAssignment != null) return true; // Already assigned

            var assignment = new TaskAssignment
            {
                TaskId = taskId,
                TeamMemberId = memberId,
                AssignedAt = DateTime.UtcNow
            };

            _context.TaskAssignments.Add(assignment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddTagToTaskAsync(int taskId, int tagId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            var tag = await _context.Tags.FindAsync(tagId);
            
            if (task == null || tag == null) return false;

            var existingTaskTag = await _context.TaskTags
                .FirstOrDefaultAsync(tt => tt.TaskId == taskId && tt.TagId == tagId);
            
            if (existingTaskTag != null) return true; // Already tagged

            var taskTag = new TaskTag
            {
                TaskId = taskId,
                TagId = tagId
            };

            _context.TaskTags.Add(taskTag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTagFromTaskAsync(int taskId, int tagId)
        {
            var taskTag = await _context.TaskTags
                .FirstOrDefaultAsync(tt => tt.TaskId == taskId && tt.TagId == tagId);
            
            if (taskTag == null) return false;

            _context.TaskTags.Remove(taskTag);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}