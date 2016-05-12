using UnityEngine;
using System.Collections;

namespace Treasure_Hunter.Mazes
{
    public class Cell
    {
        private const int W = 1;
        private const int S = 2;
        private const int E = 4;
        private const int N = 8;

        public Cell()
        {
            Walls = W + S + E + N;
            IsVisited = false;
        }

        public int Walls { get; private set; }
        public bool IsVisited { get; set; }

        public bool HasWall(int direction)
        {
            return (Walls & direction) != direction;
        }

        public void DestroyWall(int direction)
        {
            Walls &= ~direction;
        }
    }
}
