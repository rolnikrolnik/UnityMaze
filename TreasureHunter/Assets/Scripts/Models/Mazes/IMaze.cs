using System.Collections;
using UnityEngine;

namespace Treasure_Hunter.Mazes
{
	public interface IMaze
	{
		void GenerateMaze(int length, int width);
        int GetLength();
        int GetWidth();
		bool IsPointAWall(int x, int y);
	    Vector3 GetPlayerCoords(Vector3 mazeWallScale, Vector3 mazeScale);
	}
}
