using System.Collections;

namespace Treasure_Hunter.Mazes
{
	public interface IMaze
	{
		void GenerateMaze(int length, int width);
		bool IsPointAWall(int x, int y);
	}
}
