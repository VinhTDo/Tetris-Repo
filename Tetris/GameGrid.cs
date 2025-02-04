﻿namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] _grid;

        public int Rows { get; }
        public int Columns { get; }

        public int this[int row, int column]
        {
            get => _grid[row, column];
            set => _grid[row, column] = value;
        }

        public GameGrid(int row, int column) 
        {
            Rows = row;
            Columns = column;
            _grid = new int[row, column];
        }

        public bool IsInside(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }

        public bool IsEmpty(int row, int column)
        {
            return IsInside(row, column) && _grid[row, column] == 0;
        }

        public void ClearRow(int row)
        {
            for (int c = 0; c < Columns; c++)
            {
                _grid[row, c] = 0;
            }
        }

        public bool IsRowFull(int row)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (_grid[row, c] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsRowEmpty(int row)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (_grid[row, c] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int r = Rows - 1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
            }

            return cleared;
        }

        private void MoveRowDown(int row, int numRows)
        {
            for (int r = 0; r < Columns; r++)
            {
                _grid[row + numRows, r] = _grid[row, r];
                _grid[row, r] = 0;
            }
        }
    }
}
