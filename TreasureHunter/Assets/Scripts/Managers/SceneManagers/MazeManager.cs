using System.Collections;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Abstract;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Mazes;
using UnityEngine;
using System.Collections.Generic;


namespace Treasure_Hunter.Managers
{
    public class MazeManager : LevelManager
    {
        #region CLASS SETTINGS

        private static Vector3 MAZE_SCALE = new Vector3(20, 8, 20);

        #endregion

        #region SCENE REFERENCES

        public ActionChoicePopup ActionChoicePopup;
        public PlayerController Player;

        #endregion

        #region Public fields

        public int width;
        public int length;
        public bool prim;
        public float heightOfMaze;
        public MazeType mazeType;
        public GameObject mazeWallPrefab;
        public Material floorMaterial;
        public Material wallMaterial;
        public Vector3 startPosition;

        #endregion

        private IMaze MazeCreator { get; set; }

        public override void MoveUIToCanvas()
        {
            ActionChoicePopup.transform.parent = SceneManager.Instance.PagesContainer;
            ActionChoicePopup.transform.localRotation = Quaternion.identity;
            ActionChoicePopup.SetInactiveSize();
            ActionChoicePopup.transform.localPosition = Vector3.zero;
            ActionChoicePopup.Init();
            RectTransform rectTransform = ActionChoicePopup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
        
        public IEnumerator GenerateMaze(MazeType _mazeType)
        {
            //zmienić skybox'y
            mazeType = _mazeType;
            switch (mazeType)
            {
                case MazeType.PREHISTORIC_MAZE:
                case MazeType.NECROPOLIS_MAZE:
                case MazeType.WORMSWORLD_MAZE:
                    this.MazeCreator = new Maze();
                    (this.MazeCreator as Maze).IsPrim = prim;
                    break;

                case MazeType.SWAMP_MAZE:
                    this.MazeCreator = new CellularAutomata();
                    break;
            }

            var mazeWallScale = this.mazeWallPrefab.transform.localScale;
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
                        mazeWall.transform.localScale = mazeWallScale;
                    }
                    else
                    {
                        var mazeFloor = Instantiate(mazeWallPrefab,
                                                    new Vector3(x + mazeWallScale.x / 2, mazeWallScale.y / 2, y + mazeWallScale.z / 2),
                                                   Quaternion.identity) as GameObject;
                        mazeFloor.transform.parent = this.transform;
                        mazeFloor.transform.localScale = new Vector3(mazeWallScale.x, mazeWallScale.y * 0.2f, mazeWallScale.z);
                        var mazeFlooorMeshRenderer = mazeFloor.GetComponent<MeshRenderer>();
                        mazeFlooorMeshRenderer.material = this.floorMaterial;
                    }
                }
            }
            transform.localScale = MAZE_SCALE;
            yield return StartCoroutine(Activate());
            Player.transform.localPosition = startPosition;
        }

        public void Update()
        {
            if (Application.isEditor)
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
                        case MazeType.PREHISTORIC_MAZE:
                        case MazeType.NECROPOLIS_MAZE:
                        case MazeType.WORMSWORLD_MAZE:
                            this.MazeCreator = new Maze();
                            (this.MazeCreator as Maze).IsPrim = prim;
                            break;

                        case MazeType.SWAMP_MAZE:
                            this.MazeCreator = new CellularAutomata();
                            break;
                    }

                    this.MazeCreator.GenerateMaze(length, width);

                    transform.localScale = Vector3.one;
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
                                mazeWall.transform.localScale = mazeWallScale;
                            }
                            else
                            {
                                var mazeFloor = Instantiate(mazeWallPrefab,
                                                            new Vector3(x + mazeWallScale.x / 2, mazeWallScale.y / 2, y + mazeWallScale.z / 2),
                                                           Quaternion.identity) as GameObject;
                                mazeFloor.transform.parent = this.transform;
                                mazeFloor.transform.localScale = new Vector3(mazeWallScale.x, mazeWallScale.y * 0.2f, mazeWallScale.z);
                                var mazeFlooorMeshRenderer = mazeFloor.GetComponent<MeshRenderer>();
                                mazeFlooorMeshRenderer.material = this.floorMaterial;
                            }
                        }
                    }
                    transform.localScale = new Vector3(4, 2, 4);
                }
            }
        }
    }
}
