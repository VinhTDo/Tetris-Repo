﻿namespace Tetris
{
    public class LBlock : Block
    {
        private readonly Transform[][] _tiles = new Transform[][]
        {
            new Transform[] {new(0,2), new(1,0), new(1,1), new(1,2)},
            new Transform[] {new(0,1), new(1,1), new(2,1), new(2,2)},
            new Transform[] {new(1,0), new(1,1), new(1,2), new(2,0)},
            new Transform[] {new(0,0), new(0,1), new(1,1), new(2,1)}
        };

        public override int Id => 3;

        protected override Transform StartOffset => new(0, 3);
        protected override Transform[][] Tiles => _tiles;
    }
}
