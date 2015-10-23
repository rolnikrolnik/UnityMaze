using System;
using System.Collections;
using System.Linq;
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

        private static Vector3 MAZE_SCALE = new Vector3(10, 4, 10);
        private static string TerrainName = "Terrain";

        #endregion

        #region SCENE REFERENCES

        public ActionChoicePopup ActionChoicePopup;
        public PlayerController Player;

        #endregion

        #region Public fields

        public int width;
        public int length;
        public bool prim;
        public System.Random random;
        public MazeType mazeType;

        public GameObject WallPrefab;
        public GameObject FloorPrefab;
        public GameObject ExitPrefab;
        public GameObject StationaryTrapPrefab;
        public GameObject AcrossTrapPrefab;
        public GameObject DownTrapPrefab;
        public GameObject MonsterPrefab;


        public Vector3 startPosition;
        public Vector3 exitVector;

        private Vector3 MazeWallScale;



        #endregion

        #region Properties

        private IMaze MazeCreator { get; set; }

        #endregion

        public void Start()
        {
            MazeWallScale = this.WallPrefab.transform.localScale;
            random = new System.Random();
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
                    this.MazeCreator = new Maze(MazeWallScale);
                    ((Maze) this.MazeCreator).IsPrim = prim;
                    break;
                case MazeType.SWAMP_MAZE:
                    this.MazeCreator = new CellularAutomata();
                    break;
            }

            this.MazeCreator.GenerateMaze(length, width);

            foreach (var mazeComponent in this.MazeCreator.MazeComponents)
            {
                var componentType = mazeComponent.Value;
                var componentVector = mazeComponent.Key;
                switch (componentType)
                {
                    case MazeComponentType.WALL:
                        this.InstantiateMazeComponent(componentVector, WallPrefab);
                        break;
                    case MazeComponentType.FLOOR:
                        //this.InstantiateMazeComponent(componentVector, FloorPrefab);
                        break;
                    case MazeComponentType.STATIONARY_TRAP:
                        this.InstantiateMazeComponent(componentVector, StationaryTrapPrefab);
                        break;
                    case MazeComponentType.MONSTER:
                        this.InstantiateMazeComponent(componentVector, MonsterPrefab);
                        break;
                    case MazeComponentType.ACROSS_TRAP:
                        this.InstantiateMazeComponent(componentVector, AcrossTrapPrefab);
                        break;
                    case MazeComponentType.DOWN_TRAP:
                        this.InstantiateMazeComponent(componentVector, DownTrapPrefab);
                        break;
                    case MazeComponentType.EXIT:
                        this.InstantiateMazeComponent(componentVector, ExitPrefab);
                        break;
                    default:
                        break;
                }
            }

            this.ChangeMazeScale();
            //yield return StartCoroutine(Activate());
        }

        private void InstantiateMazeComponent(Vector3 vector, GameObject prefab)
        {
            try
            {
                var mazeObject = Instantiate(prefab,
                    vector,
                    Quaternion.identity) as GameObject;
                mazeObject.transform.parent = this.transform;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void ChangeMazeScale()
        {
            this.transform.localScale = MAZE_SCALE;
            Player.transform.localPosition = Vector3.Scale(this.MazeCreator.GetPlayerCoords(), MAZE_SCALE);
            var terrain = GameObject.Find(TerrainName);
            terrain.transform.position = new Vector3(
                terrain.transform.position.x,
                terrain.transform.position.y + MAZE_SCALE.y,
                terrain.transform.position.z);
        }

        public void Update()
        {
            if (Application.loadedLevel != (int)LevelEnums.MazeLevel)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    this.transform.localScale = Vector3.one;

                    foreach (var wall in GameObject.FindGameObjectsWithTag("MazeComponent"))
                    {
                        Destroy(wall);
                    }

                    this.GenerateMaze(this.mazeType);
                }
            }
        }

    }
}
