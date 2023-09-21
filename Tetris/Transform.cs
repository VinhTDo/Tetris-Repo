namespace Tetris
{
    public class Transform
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Transform(int row, int column) 
        {
            Row = row;
            Column = column;
        }
    }
}
