using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze
{
    class Program
    {
        static void Main(string[] args)
        {
            char[][] maze = new char[10][];
            for (int i = 0; i < 10; i++)
			{
			    maze[i] = new char[10];
			}
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    maze[i][j] = '#';
                }
            }
        }
    }
}
