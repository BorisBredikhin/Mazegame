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
        private Brush _wallBrush = new SolidBrush(Color.GreenYellow);

        public Form1()
        {
            DoubleBuffered = true;
            InitializeComponent();
            _gameTimer = new Timer {Interval = 100};

            var width = Size.Width / CELL_SIZE;
            var height = Size.Height / CELL_SIZE - 1;
            if (width%2==0)
            {
                width--;
            }

            if (height%2==0)
            {
                height--;
            }

            _maze = new Maze(new Size(width, height));
            _maze.Generate();

            _gameTimer.Tick += GameTimerOnTick;
            _gameTimer.Start();
        }

        private void GameTimerOnTick(object sender, EventArgs e)
        {
            Invalidate();
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
                        graphics.FillRectangle(_wallBrush, i*CELL_SIZE, j * CELL_SIZE, CELL_SIZE, CELL_SIZE);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
