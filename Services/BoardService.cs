using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models.Entities;

namespace ProjectManagement.Services
{
    public class BoardService : IBoardService
    {
        private readonly ProjectManagementContext _context;

        public BoardService(ProjectManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Board>> GetAllBoardsAsync()
        {
            return await _context.Boards
                .Include(b => b.Project)
                .Include(b => b.Tasks)
                .ToListAsync();
        }

        public async Task<Board?> GetBoardByIdAsync(int id)
        {
            return await _context.Boards
                .Include(b => b.Project)
                .Include(b => b.Tasks)
                    .ThenInclude(t => t.TaskTags)
                        .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Board> CreateBoardAsync(Board board)
        {
            board.CreatedAt = DateTime.UtcNow;
            
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task<Board?> UpdateBoardAsync(int id, Board board)
        {
            var existingBoard = await _context.Boards.FindAsync(id);
            if (existingBoard == null) return null;

            existingBoard.Name = board.Name;
            existingBoard.Description = board.Description;

            await _context.SaveChangesAsync();
            return existingBoard;
        }

        public async Task<bool> DeleteBoardAsync(int id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board == null) return false;

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Board>> GetBoardsByProjectAsync(int projectId)
        {
            return await _context.Boards
                .Where(b => b.ProjectId == projectId)
                .Include(b => b.Tasks)
                .ToListAsync();
        }
    }
}