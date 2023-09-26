namespace Tetris
{
    public class GameState
    {
        private Block _currentBlock;

        public Block CurrentBlock { 
            get => _currentBlock;
            private set 
            { 
                _currentBlock = value;
                _currentBlock.Reset();
            } 
        }
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; } = 0;

        public GameState(int gridRowSize, int gridColumnSize)
        {
            GameGrid = new(gridRowSize, gridColumnSize);
            BlockQueue = new();
            CurrentBlock = BlockQueue.GetNextBlock();
            GameOver = false;
        }

        private bool DoesBlockFit()
        {
            foreach (Transform transform in _currentBlock.GetTileTransforms())
            {
                if (!GameGrid.IsEmpty(transform.Row, transform.Column))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Transform transform in _currentBlock.GetTileTransforms())
            {
                GameGrid[transform.Row, transform.Column] = _currentBlock.Id;
            }

            Score += GameGrid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
                return;
            }

            CurrentBlock = BlockQueue.GetNextBlock();
        }

        public void RotateBlockClockwise()
        {
            _currentBlock.RotateClockwise();

            if (!DoesBlockFit())
            {
                _currentBlock.RotateCounterClockwise();
            }
        }

        public void RotateBlockCounterClockwise()
        {
            _currentBlock.RotateCounterClockwise();

            if (!DoesBlockFit())
            {
                _currentBlock.RotateClockwise();
            }
        }

        public void MoveBlockLeft()
        {
            _currentBlock.Move(0, -1);

            if (!DoesBlockFit())
            {
                _currentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            _currentBlock.Move(0, 1);

            if (!DoesBlockFit())
            {
                _currentBlock.Move(0, -1);
            }
        }

        public void MoveBlockDown()
        {
            _currentBlock.Move(1, 0);

            if (!DoesBlockFit())
            {
                _currentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }
    }
}
