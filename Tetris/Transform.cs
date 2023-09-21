namespace Tetris
{
    public class Transform
    {
        public int RowOffset { get; set; }
        public int ColumnOffset { get; set; }

        public Transform(int rowOffset, int columnOffset) 
        {
            RowOffset = rowOffset;
            ColumnOffset = columnOffset;
        }
    }
}
