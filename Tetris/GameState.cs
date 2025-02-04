﻿using System;

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
        public Block HeldBlock { get; private set; }
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; } = 0;
        public bool CanHold { get; private set; }

        public GameState(int gridRowSize, int gridColumnSize)
        {
            GameGrid = new(gridRowSize, gridColumnSize);
            BlockQueue = new();
            CurrentBlock = BlockQueue.GetNextBlock();
            GameOver = false;
            CanHold = true;
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

        private int GetTileDropDistance(Transform transform)
        {
            int dropDistance = 0;

            while (GameGrid.IsEmpty(transform.Row + dropDistance + 1, transform.Column))
            {
                dropDistance++;
            }

            return dropDistance;
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
            CanHold = true;
        }

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetNextBlock();
            }
            else
            {
                (HeldBlock, CurrentBlock) = (CurrentBlock, HeldBlock);
            }

            CanHold = false;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(GetBlockDropDistance(), 0);
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

        public int GetBlockDropDistance()
        {
            int dropDistance = GameGrid.Rows;

            foreach (Transform transform in CurrentBlock.GetTileTransforms())
            {
                dropDistance = Math.Min(dropDistance, GetTileDropDistance(transform));
            }

            return dropDistance;
        }
    }
}
