using System.Collections;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Abstract;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treasure_Hunter.Mazes;

namespace Treasure_Hunter.Managers
{
    public class MazeManager : LevelManager
    {
        #region Public fields

        public int width;
        public int length;
        public bool prim;
        public float heightOfMaze;
        public MazeType mazeType;
        public GameObject mazeWallPrefab;
        public Material floorMaterial;
        public Material wallMaterial;

        #endregion

        private IMaze MazeCreator { get; set; }

        public override void MoveUIToCanvas()
        {

        }

        public IEnumerator GenerateMaze(MazeType mazeType)
        {
            //zmienić skybox'y
            yield return StartCoroutine(Activate());
        }

        public void Start()
        {
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                var mazeWallScale = this.mazeWallPrefab.transform.localScale;
                foreach (var wall in GameObject.FindGameObjectsWithTag("MazeWall"))
                {
                    Destroy(wall);
                }

                switch (mazeType)
                {
                    case MazeType.WormsWorldMaze:
                    case MazeType.NecropolisMaze:
                    case MazeType.PrehistoricMaze:
                        this.MazeCreator = new Maze();
                        (this.MazeCreator as Maze).IsPrim = prim;
                        break;

                    case MazeType.SwampMaze:
                        this.MazeCreator = new CellularAutomata();
                        break;
                }

                this.MazeCreator.GenerateMaze(length, width);

                for (int x = 0; x < length; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        if (this.MazeCreator.IsPointAWall(x, y))
                        {
                            var mazeWall = Instantiate(mazeWallPrefab,
                                                       new Vector3(x + mazeWallScale.x / 2, mazeWallScale.y / 2, y + mazeWallScale.z / 2),
                                                       Quaternion.identity) as GameObject;
                            mazeWall.transform.parent = this.transform;
                        }
                        else
                        {
                            var mazeFloor = Instantiate(mazeWallPrefab,
                                                        new Vector3(x + mazeWallScale.x / 2, mazeWallScale.y / 2, y + mazeWallScale.z / 2),
                                                       Quaternion.identity) as GameObject;
                            mazeFloor.transform.localScale = new Vector3(mazeWallScale.x, mazeWallScale.y * 0.5f, mazeWallScale.z);
                            var mazeFlooorMeshRenderer = mazeFloor.GetComponent<MeshRenderer>();
                            mazeFlooorMeshRenderer.material = this.floorMaterial;
                        }
                    }
                }
            }
        }
    }
}
