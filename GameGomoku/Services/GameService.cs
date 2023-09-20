using GameGomoku.Constants;
using GameGomoku.Interfaces;
using GameGomoku.Models;
using GomokuGame.Models;

namespace GameGomoku.Services
{
    public sealed class GameService : IGameService
    {
        public Board board;

        public GameService() => board = new Board();

        /// <summary>
        /// Create and initialize the board.
        /// </summary>
        /// <param name="columnSize"></param>
        /// <param name="rowSize"></param>
        /// <param name="startingPlayer"></param>
        /// <returns></returns>
        public async Task<APIResponse> CreateBoard(int columnSize, int rowSize, int startingPlayer)
        {
            var apiResponse = new APIResponse();
            int[,] intersections = new int[columnSize, rowSize];
            for(int col = 0; col < intersections.GetLength(0); col++)
            {
                for(int row = 0; row < intersections.GetLength(1); row++)
                {
                    intersections[col, row] = 0;
                }
            }

            board.Intersections = intersections;
            board.ColumnSize = columnSize;
            board.RowSize = rowSize;
            board.CurrentPlayerId = startingPlayer;
            board.GameStatus = 0;

            apiResponse.Ok = true;
            apiResponse.Message = GlobalConstants.BoardCreated;
            apiResponse.Result = GlobalConstants.Success;

            return apiResponse;
        }

        /// <summary>
        /// Create a stone in the board by passing the column and row position.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<APIResponse> CreateStone(int column, int row)
        {
            var apiResponse = new APIResponse();
            apiResponse.Ok = false;
            if (board.Intersections == null)
            {
                apiResponse.Message = GlobalConstants.BoardNotExist;
                apiResponse.Result = GlobalConstants.Invalid;
                return apiResponse;
            }
            try {
                var columnSize = board.Intersections?.GetLength(0);
                var rowSize = board.Intersections?.GetLength(1);


                var player = new Player();
                player.Id = board.CurrentPlayerId;
                player.Name = board.CurrentPlayerId == 1 ? GlobalConstants.PlayerOne : GlobalConstants.PlayerTwo;
                player.Color = board.CurrentPlayerId == 1 ? GlobalConstants.PlayerOneColor : GlobalConstants.PlayerTwoColor;
                player.Column = column;
                player.Row = row;

                if(column < columnSize && row < rowSize && column >= 0 && row >= 0)
                {
                    var intersection = board.Intersections?[column, row];
                    if (!intersection.Equals(0)) {
                        apiResponse.Message = GlobalConstants.StoneAlreadyExists;
                        apiResponse.Result = GlobalConstants.Invalid;
                        return apiResponse;
                    }
                    else if (board.GameStatus != 0)
                    {
                        apiResponse.Message = GlobalConstants.GameAlreadyEnded;
                        apiResponse.Result = GlobalConstants.Invalid;
                        return apiResponse;
                    }
                    else
                    {
                        board.Intersections[column, row] = board.CurrentPlayerId;
                        board.GameStatus = CheckGameStatus(board.Intersections, column, row);
                        apiResponse.Ok = true;
                        switch (board.GameStatus)
                        {
                            case 0:
                                board.CurrentPlayerId = board.CurrentPlayerId == 1 ? 2 : 1;
                                apiResponse.Message = $"{player.Name} took his turn and placed a {player.Color} stone in column: {player.Column} Row: {player.Row}.";
                                apiResponse.Result = GlobalConstants.NextTurn;
                                break;
                            case 1:
                                apiResponse.Message = $"{player.Name} with {player.Color} stone wins";
                                apiResponse.Result = GlobalConstants.GameEnded;
                                break;
                            default:
                                apiResponse.Message = GlobalConstants.NoPossibleMoves;
                                apiResponse.Result = GlobalConstants.GameEnded;
                                break;
                        }
                    }
                }
                else
                {
                    // When stone is placed outside the board
                    apiResponse.Message = GlobalConstants.StoneOutOfBounds;
                    apiResponse.Result = GlobalConstants.Invalid;
                }
            }
            catch (Exception ex)
            {
                apiResponse.Message = $"GameGomoku.Services.GameService.CreateStone caught an exception: {ex.Message}";
                apiResponse.Result = GlobalConstants.Exception;

            }
            return apiResponse;
        }

        #region Private Methods
        private bool CheckChainedIntersection(int[,] intersections, int currentCol, int currentRow, int checkCol, int checkRow)
        {
            int currentIntersection = intersections[currentCol, currentRow];
            int checkingIntersection = intersections[checkCol, checkRow];

            if (!currentIntersection.Equals(0) && currentIntersection.Equals(checkingIntersection))
            {
                return true;
            }

            return false;
        }

        private bool ValidateWinningChain(int NumberOfChainedStones) => (NumberOfChainedStones >= GlobalConstants.NumberOfStoneToWin);

        private bool CheckHorizontalIntersection(int[,] intersections, int col, int row)
        {
            int NumberOfChainedStones = 1;
            for (int i = col + 1; i < intersections.GetLength(0); i++)
            {
                if (CheckChainedIntersection(intersections, col, row, i, row)){
                    NumberOfChainedStones++;
                    continue;
                }
                break;
            }

            for (int i = col - 1; i >= 0; i--)
            {
                if (CheckChainedIntersection(intersections, col, row, i, row)){
                    NumberOfChainedStones++;
                    continue;
                }
                break;
            }

            return ValidateWinningChain(NumberOfChainedStones);
        }

        private bool CheckVerticalIntersection(int[,] intersections, int col, int row)
        {
            int NumberOfChainedStones = 1;
            for (int i = row + 1; i < intersections.GetLength(1); i++)
            {
                if (CheckChainedIntersection(intersections, col, row, col, i)){
                    NumberOfChainedStones++;
                    continue;
                }
                break;
            }

            for (int i = row - 1; i >= 0; i--)
            {
                if (CheckChainedIntersection(intersections, col, row, col, i)){
                    NumberOfChainedStones++;
                    continue;
                }
                break;
            }

            return ValidateWinningChain(NumberOfChainedStones);
        }

        private bool CheckDiagonalLeftToRightIntersection(int[,] intersections, int col, int row)
        {
            int NumberOfChainedStones = 1;


            for(int i = col + 1, j = row + 1; i < intersections.GetLength(0) && j < intersections.GetLength(1); i++, j++)
            {
                if(CheckChainedIntersection(intersections, col, row, i, j))
                {
                    NumberOfChainedStones++;
                    continue;
                }
                break;
            }

            for (int i = col - 1, j = row - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (CheckChainedIntersection(intersections, col, row, i, j))
                {
                    NumberOfChainedStones++;
                    continue;
                }
                break;
            }

            return ValidateWinningChain(NumberOfChainedStones);
        }


        private bool CheckDiagonalRightToLeftIntersection(int[,] intersections, int col, int row)
        {
            int NumberOfChainedStones = 1;

            for (int i = row + 1, j = col - 1; i < intersections.GetLength(1) && j > 0; i++, j--)
            {
                if (CheckChainedIntersection(intersections, col, row, i, j))
                {
                    NumberOfChainedStones++;
                    continue;
                }
                break;
            }

            for (int i = row - 1, j = col + 1; i >= 0 && j < intersections.GetLength(0); i--, j++)
            {
                if (CheckChainedIntersection(intersections, col, row, i, j))
                {
                    NumberOfChainedStones++;
                    continue;
                }
                break;
            }

            return ValidateWinningChain(NumberOfChainedStones);
        }

        private bool CheckWinningMove(int[,] intersections, int col, int row)
        {
            if(
                CheckDiagonalLeftToRightIntersection(intersections, col, row) ||
                CheckDiagonalRightToLeftIntersection(intersections, col, row) ||
                CheckHorizontalIntersection(intersections, col, row) ||
                CheckVerticalIntersection(intersections, col, row)
            )
            {
                return true;
            }
            return false;
        }
        private bool CheckNoPossibleMoves(int[,] intersections)
        {
            for (int i = 0; i < intersections.GetLength(0); i++)
            {
                for (int j = 0; j < intersections.GetLength(1); j++)
                {
                    if (intersections[i, j].Equals(0))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private int CheckGameStatus(int[,] intersections, int col, int row)
        {
            if (CheckWinningMove(intersections, col, row))
            {
                return 1;
            }
            
            else if (CheckNoPossibleMoves(intersections))
            {
                return 2;
            }

            return 0;
        }

        #endregion

    }
}
