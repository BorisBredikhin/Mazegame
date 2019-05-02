using System;
using System.Drawing;

namespace Mazegame.Model
{
    public class Player
    {
        public readonly Maze Maze;
        public Point Position { get; set; } = new Point(1,1);

        public event Action OnFinish;

        public int Path { get; private set; }

        public Player(Maze maze)
        {
            Maze = maze;
        }

        public void Move(int dx, int dy)
        {
            if (Maze[Position.X + dx, Position.Y + dy] != CellType.Wall)
            {
                Position = new Point(Position.X + dx, Position.Y + dy);
                Path++;
            }
            if (Position == Maze.EndPoint)
                OnFinish?.Invoke();
        }
    }
}
