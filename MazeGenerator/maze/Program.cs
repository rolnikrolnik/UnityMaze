using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace maze
{
    class Program
    {
        private const int FRAME_SIZE = 1;
        private const int MAZE_SIZE = 20;
        private const int MAZE_START = FRAME_SIZE;
        private const int MAZE_END = MAZE_SIZE + MAZE_START;
        private const int BOARD_SIZE = MAZE_SIZE + 2*FRAME_SIZE;

        static void AddWalls(ref List<Point> wall_list, ref char[][] maze, Point start)
        {
            if (maze[start.Y][start.X - 1] == '#') wall_list.Add(new Point(start.X - 1, start.Y));
            if (maze[start.Y][start.X + 1] == '#') wall_list.Add(new Point(start.X + 1, start.Y));
            if (maze[start.Y - 1][start.X] == '#') wall_list.Add(new Point(start.X, start.Y - 1));
            if (maze[start.Y + 1][start.X] == '#') wall_list.Add(new Point(start.X, start.Y + 1));
        }

        private static Point GetDirection(ref char[][] maze,ref Point start)
        {
            int counter = 0;
            Point ret = new Point(start.X, start.Y);
            if (maze[start.Y][start.X - 1] == ' ' && maze[start.Y][start.X + 1] != '%') { counter++; ret.X += 1; }
            if (maze[start.Y][start.X + 1] == ' ' && maze[start.Y][start.X - 1] != '%') { counter++; ret.X -= 1; }
            if (maze[start.Y - 1][start.X] == ' ' && maze[start.X][start.Y + 1] != '%') { counter++; ret.Y += 1; }
            if (maze[start.Y + 1][start.X] == ' ' && maze[start.X][start.Y - 1] != '%') { counter++; ret.Y -= 1; }

            if (counter > 1) return new Point(0,0);
            return ret;
        }

        static void Main(string[] args)
        {
            char[][] maze = new char[BOARD_SIZE][];
            var wall_list = new List<Point>();
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                maze[i] = new char[BOARD_SIZE];
            }
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                maze[i][0] = '%';
                maze[0][i] = '%';
                maze[MAZE_END][i] = '%';
                maze[i][MAZE_END] = '%';
            }

            for (int i = MAZE_START; i < MAZE_END; i++)
            {
                for (int j = MAZE_START; j < MAZE_END; j++)
                {
                    maze[i][j] = '#';
                }
            }

            Random rand = new Random();
            Point starting_point = new Point(rand.Next(MAZE_START, MAZE_END), MAZE_END-1);

            maze[starting_point.Y][starting_point.X] = ' ';
            AddWalls(ref wall_list, ref maze, starting_point);

            while (wall_list.Count != 0)
            {
                Point wall = wall_list[rand.Next(0, wall_list.Count)];
                Point direction = GetDirection(ref maze, ref wall);
                if (!direction.IsEmpty)
                {
                    maze[wall.Y][wall.X] = ' ';
                    maze[direction.Y][direction.X] = ' ';
                    //AddWalls(ref wall_list, ref maze, wall);
                    AddWalls(ref wall_list, ref maze, direction);
                }
                wall_list.Remove(wall);
            }


            for (int i = 0; i < BOARD_SIZE; i++)
            {
                Console.WriteLine(maze[i]);
            }
        }
    }
}
