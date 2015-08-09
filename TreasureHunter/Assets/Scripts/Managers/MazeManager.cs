using System.Collections;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Abstract;

namespace Treasure_Hunter.Managers
{
    public class MazeManager : LevelManager
    {
        public override void MoveUIToCanvas()
        {

        }
        
        public IEnumerator GenerateMaze(MazeType mazeType)
        {
            //zmienić skybox'y
            yield return StartCoroutine(Activate());
        }
    }
}
