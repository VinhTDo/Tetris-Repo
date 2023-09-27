using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] _blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };
        private readonly ImageSource[] _tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };
        private readonly Image[,] _imageControls;
        private const int _gridRowSize = 22;
        private const int _gridColumnSize = 10;
        private int _heightOffset = 10;
        private int _cellSize = 25;
        private int _maxDelay = 1000;
        private int _minDelay = 75;
        private int _delayDecrease = 25;
        private int _highScore = 0;
        private double _opacity = 0.25d;
        private GameState _gameState = new(_gridRowSize, _gridColumnSize);

        public MainWindow()
        {
            InitializeComponent();
            _imageControls = InitializeGameGrid(_gameState.GameGrid);
            _highScore = Properties.Settings.Default.HighScore;
        }

        private Image[,] InitializeGameGrid(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new()
                    {
                        Width = _cellSize,
                        Height = _cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * _cellSize + _heightOffset);
                    Canvas.SetLeft(imageControl, c * _cellSize);
                    GridCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }

        private async Task StartGameLoopAsync()
        {
            Draw(_gameState);

            while (!_gameState.GameOver)
            {
                int delayTime = Math.Max(_minDelay, _maxDelay - (_gameState.Score * _delayDecrease));
                await Task.Delay(delayTime);
                _gameState.MoveBlockDown();
                Draw(_gameState);
            }

            GameOverMenuGrid.Visibility = Visibility.Visible;
            FinalScoreTextBlock.Text = $"{_gameState.Score}";

            if (_gameState.Score > _highScore)
            {
                _highScore = _gameState.Score;
                Properties.Settings.Default.HighScore = _gameState.Score;
                Properties.Settings.Default.Save();
            }

            HighscoreTextBlock.Text = $"{_highScore}";
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    _imageControls[r, c].Opacity = 1d;
                    _imageControls[r, c].Source = _tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Transform transform in block.GetTileTransforms())
            {
                _imageControls[transform.Row, transform.Column].Source = _tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block nextBlock = blockQueue.NextBlock;
            NextBlockImage.Source = _blockImages[nextBlock.Id];
        }

        private void DrawHoldBlock(Block block)
        {
            if (block == null)
            {
                HoldBlockImage.Source = _blockImages[0];
                return;
            }

            HoldBlockImage.Source = _blockImages[block.Id];
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = _gameState.GetBlockDropDistance();

            foreach (Transform transform in block.GetTileTransforms())
            {
                _imageControls[transform.Row + dropDistance, transform.Column].Opacity = _opacity;
                _imageControls[transform.Row + dropDistance, transform.Column].Source = _tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHoldBlock(gameState.HeldBlock);
            ScoreTextBlock.Text = $"{gameState.Score}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    _gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    _gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    _gameState.MoveBlockDown();
                    break;
                case Key.Q:
                    _gameState.RotateBlockCounterClockwise();
                    break;
                case Key.E:
                    _gameState.RotateBlockClockwise();
                    break;
                case Key.Z:
                    _gameState.HoldBlock();
                    break;
                case Key.Space:
                    _gameState.DropBlock();
                    break;
                default:
                    return;
            }

            Draw(_gameState);
        }

        private async void GridCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await StartGameLoopAsync();
        }

        private async void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            _gameState = new(_gridRowSize, _gridColumnSize);
            GameOverMenuGrid.Visibility = Visibility.Hidden;
            await StartGameLoopAsync();
        }
    }
}
