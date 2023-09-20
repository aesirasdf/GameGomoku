using GameGomoku.Models;

namespace GameGomoku.Interfaces
{
    public interface IGameService
    {
        Task<APIResponse> CreateBoard(int columnSize, int rowSize, int startingPlayer);
        Task<APIResponse> CreateStone(int row, int column);
    }
}
