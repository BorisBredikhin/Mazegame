using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Model;

namespace Mazegame.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var maze = new Maze(new Size(31, 11));
            Console.WriteLine(maze);
        }
    }
}
