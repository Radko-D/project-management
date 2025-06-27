using ProjectManagement.Models.Entities;

namespace ProjectManagement.Services
{
    public interface IBoardService
    {
        Task<IEnumerable<Board>> GetAllBoardsAsync();
        Task<Board?> GetBoardByIdAsync(int id);
        Task<Board> CreateBoardAsync(Board board);
        Task<Board?> UpdateBoardAsync(int id, Board board);
        Task<bool> DeleteBoardAsync(int id);
        Task<IEnumerable<Board>> GetBoardsByProjectAsync(int projectId);
    }
}