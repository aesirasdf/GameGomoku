using System.ComponentModel.DataAnnotations;

namespace GomokuGame.Models
{
    public class Board
    {
        public int[,]? Intersections { get; set; }
        [Range(1, 2)]
        public int CurrentPlayerId { get; set; }
        public int ColumnSize { get; set; }
        public int RowSize { get; set; }

        public int GameStatus { get; set; }
    }
}
