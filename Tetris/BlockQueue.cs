using System;

namespace Tetris
{
    public class BlockQueue
    {
        private readonly Block[] _blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };
        private readonly Random _random = new();

        public Block NextBlock { get; private set; }

        public BlockQueue()
        {
            NextBlock = GetRandomBlock();
        }

        public Block GetNextBlock()
        {
            Block block = NextBlock;

            do
            {
                NextBlock = GetRandomBlock();
            } 
            while (block.Id == NextBlock.Id);

            return block;
        }

        private Block GetRandomBlock()
        {
            return _blocks[_random.Next(_blocks.Length)];
        }
    }
}
