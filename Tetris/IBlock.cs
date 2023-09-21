namespace Tetris
{
    public class IBlock : Block
    {
        private readonly Transform[][] _tiles = new Transform[][]
        {
            new Transform[] {new(1,0), new(1,1), new(1,2), new(1,3)},
            new Transform[] {new(0,2), new(1,2), new(2,2), new(3,2)},
            new Transform[] {new(2,0), new(2,1), new(2,2), new(2,3)},
            new Transform[] {new(0,1), new(1,1), new(2,1), new(3,1)}
        };

        public override int Id => 1;

        protected override Transform StartOffset => new(-1, 3);
        protected override Transform[][] Tiles => _tiles;
    }
}
