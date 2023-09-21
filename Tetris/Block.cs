using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block
    {
        protected abstract Transform[][] Tiles { get; }
        protected abstract Transform StartOffset { get; }

        public abstract int Id { get; }

        private int _rotationState = 0;
        private Transform offset; 

        public Block()
        {
            offset = new(StartOffset.Row, StartOffset.Column);
        }

        public IEnumerable<Transform> GetTileTransforms()
        {
            foreach (Transform transform in Tiles[_rotationState]) 
            {
                yield return new Transform(transform.Row + offset.Row, transform.Column + offset.Column);
            }
        }

        public void RotateClockwise()
        {
            _rotationState = (_rotationState + 1) % Tiles.Length;
        }

        public void RotateCounterClockwise()
        {
            if (_rotationState < 0)
            {
                _rotationState = Tiles.Length - 1;
                return;
            }

            _rotationState--;
        }

        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset()
        {
            _rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
