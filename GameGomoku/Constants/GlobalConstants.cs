namespace GameGomoku.Constants
{
    public static class GlobalConstants
    {
        //  MESSAGES
        public const string BoardNotExist = "You have to create a board first!";
        public const string BoardCreated = "Board has been created.";
        public const string GameAlreadyEnded = "You can't place a stone, the game already ended!";
        public const string StoneCreated = "Stone has been placed.";
        public const string StoneOutOfBounds = "You can't place a stone outside the board!";
        public const string StoneAlreadyExists = "There's already a stone in this tile!";
        public const string NoPossibleMoves = "Game ended as a draw. There's no more possible moves.";
        public const string InvalidStartingPlayer = "Starting player id should only be either 1 or 2";

        //  GAME RULE CONSTANTS
        public const int NumberOfStoneToWin = 5;

        //  PLAYER CONSTANTS
        public const string PlayerOne = "Player 1";
        public const string PlayerTwo = "Player 2";
        public const string PlayerOneColor = "Black";
        public const string PlayerTwoColor = "White";

        //  RESULT CONSTANTS
        public const string Success = "Success";
        public const string Exception = "Error!";
        public const string Invalid = "Invalid!";
        public const string NextTurn = "Next Turn";
        public const string GameEnded = "Game Ended";
    }
}
