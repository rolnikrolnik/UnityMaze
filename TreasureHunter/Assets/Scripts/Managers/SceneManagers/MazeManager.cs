using System.Collections;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Abstract;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Mazes;
using UnityEngine;


namespace Treasure_Hunter.Managers
{
    public class MazeManager : LevelManager
    {
        #region CLASS SETTINGS

        private static Vector3 MAZE_SCALE = new Vector3(4, 2, 4);

        #endregion

        #region SCENE REFERENCES

        public ActionChoicePopup ActionChoicePopup;
        public PlayerController Player;

        #endregion

        #region Public fields

        public int width;
        public int length;
        public float mapScale;
        public bool prim;
        public MazeType mazeType;

        public GameObject mazeWallPrefab;
        public Material floorMaterial;
        public Material wallMaterial;
        public Vector3 startPosition;

        private Vector3 MazeWallScale;



        #endregion

        #region Properties

        private IMaze MazeCreator { get; set; }

        #endregion

        public void Start()
        {
            MazeWallScale = this.mazeWallPrefab.transform.localScale;
        }

        public override void MoveUIToCanvas()
        {
            ActionChoicePopup.transform.parent = SceneManager.Instance.PagesContainer;
            ActionChoicePopup.transform.localRotation = Quaternion.identity;
            ActionChoicePopup.transform.localScale = Vector3.one;
            ActionChoicePopup.transform.localPosition = Vector3.zero;
            RectTransform rectTransform = ActionChoicePopup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
        
        public void GenerateMaze(MazeType _mazeType)
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

            this.MazeCreator.GenerateMaze(length, width);

            for (var x = 0; x < this.MazeCreator.GetLength(); x++)
            {
                for (var y = 0; y < this.MazeCreator.GetWidth(); y++)
                {
                    if (this.MazeCreator.IsPointAWall(x, y))
                    {
                        this.InstantiateMazeWall(x, y);
                    }
                    else
                    {

                        this.InstantiateMazeFloor(x, y);
                    }
                }
            }
            this.transform.localScale = MAZE_SCALE;
            //yield return StartCoroutine(Activate());
            Player.transform.localPosition = this.MazeCreator.GetPlayerCoords(MazeWallScale, MAZE_SCALE);
        }

        public void Update()
        {
            if (Application.loadedLevel != (int)LevelEnums.MazeLevel)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    this.transform.localScale = Vector3.one;

                    foreach (var wall in GameObject.FindGameObjectsWithTag("MazeWall"))
                    {
                        Destroy(wall);
                    }

                    this.GenerateMaze(this.mazeType);
                }
            }
        }

        private void InstantiateMazeWall(int x, int y)
        {
            var mazeWall = Instantiate(mazeWallPrefab,
                           new Vector3(x + MazeWallScale.x / 2, MazeWallScale.y / 2, y + MazeWallScale.z / 2),
                           Quaternion.identity) as GameObject;
            mazeWall.transform.parent = this.transform;
            mazeWall.transform.localScale = MazeWallScale;
        }

        private void InstantiateMazeFloor(int x, int y)
        {
            var mazeFloor = Instantiate(mazeWallPrefab,
                            new Vector3(x + MazeWallScale.x / 2, MazeWallScale.y / 2, y + MazeWallScale.z / 2),
                           Quaternion.identity) as GameObject;
            mazeFloor.transform.parent = this.transform;
            mazeFloor.transform.localScale = new Vector3(MazeWallScale.x, MazeWallScale.y * 0.2f, MazeWallScale.z);
            var mazeFlooorMeshRenderer = mazeFloor.GetComponent<MeshRenderer>();
            mazeFlooorMeshRenderer.material = this.floorMaterial;
        }
    }
}
