using System.Collections.Generic;
using Treasure_Hunter.Enumerations;
using UnityEngine;

namespace Treasure_Hunter.Mazes
{
	public interface IMaze
	{
	    int Length { get; }
	    int Width { get; }
        void GenerateMaze(int length, int width);
        bool IsPointAWall(int x, int y);
	}
}
