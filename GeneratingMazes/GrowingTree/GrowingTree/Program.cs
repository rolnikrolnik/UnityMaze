using System;
using System.Linq;
using System.Runtime.Serialization.Formatters;

namespace GrowingTree
{
    class Program
    {

        static void Main(string[] args)
        {
            int height = 0;
            int width = 0;

            if (args.Length < 2)
            {
                Console.WriteLine("Please, provide 2 numbers - height and width of the maze!");
                return;
            }
            try
            {
                Int32.TryParse(args[0], out height);
                Int32.TryParse(args[1], out width);
            }
            catch (Exception)
            {
                Console.WriteLine("Bad numbers!");
                return;
            }

            var maze = new Maze();
            maze.GenerateMaze(height, width);
            maze.PrintMaze();
        }
    }
}
