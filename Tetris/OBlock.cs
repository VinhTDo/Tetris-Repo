namespace Tetris
{
    public class OBlock : Block
    {
        private readonly Transform[][] _tiles = new Transform[][]
        {
            new Transform[] {new(0,0), new(0,1), new(1,0), new(1,1)}
        };

        public override int Id => 4;

        protected override Transform StartOffset => new(0,4);
        protected override Transform[][] Tiles => _tiles;
    }
}
