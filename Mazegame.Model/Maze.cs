using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Mazegame.Model
{
    public sealed class Maze
    {
        private CellType[,] _maze;
        public readonly Size Size;

        public Maze(Size size)
        {
            Size = size;
            _maze = new CellType[Size.Width,Size.Height];
            Generate();
        }

        public void Generate(int? seed = null)
        {
            var random = new Random(seed ?? DateTime.Now.Millisecond);

            GenerateStartFrame();

            GenerateDFS(random);
        }

        private void GenerateDFS(Random random)
        {
            var startCell = (1, 1);

            var visited = new HashSet<(int, int)>();
            var stack = new Stack<(int, int)>();

            visited.Add(startCell);
            stack.Push(startCell);

            while (stack.Count>0)
            {
                var neighbours = GetNeighbours(startCell, visited).ToArray();
                if (neighbours.Length > 0)
                {
                    var n = neighbours[random.Next(neighbours.Length)];
                    _maze[(startCell.Item1 + n.Item1) / 2, (startCell.Item2 + n.Item2) / 2] = CellType.Cell;
                    stack.Push(n);
                    visited.Add(n);
                }
                else
                    startCell = stack.Pop();
            }
        }

        private IEnumerable<(int, int)> GetNeighbours((int, int) startCell, HashSet<(int, int)> visited)
        {
            for (var dx = -2; dx <= 2; dx+=2)
            for (var dy = -2; dy <= 2; dy+=2)
                if (dx*dy==0 && dx+dy!=0)
                
            {
                var t = (startCell.Item1 + dx, startCell.Item2 + dy);
                if (0 < t.Item1 && t.Item1 < Size.Width - 1 &&
                    0 < t.Item2 && t.Item2 < Size.Height - 1)
                    if (!visited.Contains(t))
                        yield return t;
            }
        }

        private void GenerateStartFrame()
        {
            for (var i = 0; i < Size.Height; i++)
            for (var j = 0; j < Size.Width; j++)
                _maze[j, i] =
                    i % 2 != 0 && j % 2 != 0 && i < Size.Height - 1 && j < Size.Width - 1
                    ? CellType.Cell
                    : CellType.Wall;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(Size.Height*(Size.Height+2));

            for (var i = 0; i < Size.Height; i++)
            {
                for (var j = 0; j < Size.Width; j++)
                    sb.Append((char)_maze[j, i]);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public CellType this[int i, int j]
        {
            get => _maze[i, j];
        }
    }
}
