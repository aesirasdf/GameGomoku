﻿namespace GomokuGame.Models
{
    public class Player
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
