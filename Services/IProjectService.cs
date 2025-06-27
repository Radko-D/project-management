using ProjectManagement.Models.Entities;

namespace ProjectManagement.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project?> UpdateProjectAsync(int id, Project project);
        Task<bool> DeleteProjectAsync(int id);
        Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status);
    }
}