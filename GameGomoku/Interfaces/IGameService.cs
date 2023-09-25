using GameGomoku.Models;

namespace GameGomoku.Interfaces
{
    public interface IGameService
    {
        Task<APIResponse> CreateBoard(int rowSize, int columnSize, int startingPlayer);
        Task<APIResponse> CreateStone(int row, int column);
    }
}
