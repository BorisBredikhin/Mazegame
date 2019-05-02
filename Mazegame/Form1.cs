using System;
using System.Drawing;
using System.Windows.Forms;
using Mazegame.Model;

namespace Mazegame
{
    public partial class Form1 : Form
    {
        private const int CELL_SIZE = 25;

        private Timer _gameTimer;
        private Maze _maze;
        private Brush _wallBrush = Brushes.GreenYellow,
            _playerBrush = Brushes.Red,
            _finishBrush = Brushes.Green;

        public Form1()
        {
            DoubleBuffered = true;
            InitializeComponent();
            _gameTimer = new Timer {Interval = 100};

            NewGame();

            KeyDown += (sender, args) =>
            {
                switch (args.KeyCode)
                {
                    case Keys.Up:
                        _maze.Player.Move(0, -1);
                        break;
                    case Keys.Down:
                        _maze.Player.Move(0, 1);
                        break;
                    case Keys.Left:
                        _maze.Player.Move(-1, 0);
                        break;
                    case Keys.Right:
                        _maze.Player.Move(1, 0);
                        break;
                }
            };
            
            _gameTimer.Tick += GameTimerOnTick;
            _gameTimer.Start();
            SizeChanged += (sender, args) => { NewGame(); };
            _newGameButton.Click += (sender, args) => { NewGame(); };
        }

        private void NewGame()
        {
            var width = Size.Width / CELL_SIZE;
            var height = (Size.Height-50) / CELL_SIZE - 1;
            if (width % 2 == 0)
            {
                width--;
            }

            if (height % 2 == 0)
            {
                height--;
            }

            _maze = new Maze(new Size(width, height));
            _maze.Generate();

            _maze.Player.OnFinish += () =>
            {
                var response = MessageBox.Show("Игра окончена, хотите начать новую?", "", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (response == DialogResult.Yes)
                {
                    NewGame();
                }
            };
        }

        private void GameTimerOnTick(object sender, EventArgs e)
        {
            Invalidate();
            _pathLabel.Text = $"Пройденный путь: {_maze.Player.Path}";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            for (var i = 0; i < _maze.Size.Width; i++)
            for (var j = 0; j < _maze.Size.Height; j++)
            {
                var cell = _maze[i, j];
                switch (cell)
                {
                    case CellType.Wall:
                        graphics.FillRectangle(_wallBrush,
                            i*CELL_SIZE,
                            j * CELL_SIZE+50,
                            CELL_SIZE,
                            CELL_SIZE);
                        break;
                    default:
                        break;
                }
            }

            graphics.FillEllipse(_playerBrush,
                _maze.Player.Position.X*CELL_SIZE,
                _maze.Player.Position.Y*CELL_SIZE+50,
                CELL_SIZE,
                CELL_SIZE);
            graphics.FillEllipse(_finishBrush,
                _maze.EndPoint.X*CELL_SIZE,
                _maze.EndPoint.Y*CELL_SIZE+50,
                CELL_SIZE,
                CELL_SIZE);
        }
    }
}
