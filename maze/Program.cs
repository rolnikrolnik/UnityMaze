using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze
{
    class Program
    {
        private const int FRAME_SIZE = 1;
        private const int MAZE_SIZE = 10;
        private const int MAZE_START = FRAME_SIZE;
        private const int MAZE_END = MAZE_SIZE + MAZE_START;
        private const int BOARD_SIZE = MAZE_SIZE + 2*FRAME_SIZE;

        static void Main(string[] args)
        {
            char[][] maze = new char[BOARD_SIZE][];
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

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                Console.WriteLine(maze[i]);
            }
        }
    }
}
