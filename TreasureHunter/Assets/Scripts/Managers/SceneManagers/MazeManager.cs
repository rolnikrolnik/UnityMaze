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
        private static Vector3 TerrainTransform = new Vector3(1, 5, 1);

        #endregion

        #region SCENE REFERENCES

        public ActionChoicePopup ActionChoicePopup;
        public PlayerController Player;

        #endregion

        #region Fields

        private Vector3 mazeWallScale;
        private GameObject exitComponent;

        public int Width;
        public int Length;
        public int TrapProbability;
        public int MonsterProbability;
        public bool Prim;
        public MazeType MazeType;
        public GameObject WallPrefab;
        public GameObject FloorPrefab;
        public GameObject ExitPrefab;
        public GameObject StationaryTrapPrefab;
        public GameObject AcrossTrapPrefab;
        public GameObject DownTrapPrefab;
        public GameObject MonsterPrefab;

        #endregion

        #region Properties

        private IMaze Maze { get; set; }
        private MazeConverter MazeConverter { get; set; }

        #endregion

        public void Start()
        {
            mazeWallScale = this.WallPrefab.transform.localScale;
        }

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

        public void GenerateMaze(MazeType mazeType)
        {
            this.MazeType = mazeType;
            //zmienić skybox'y
            this.RestoreDefaultPositions();
            this.InstantiateMazeObject(mazeType);
            this.GenerateMazeComponents();
            this.ChangeMazeScale();
            StartCoroutine(this.CheckIfCorrect());
        }

        private void RestoreDefaultPositions()
        {
            this.transform.localScale = Vector3.one;
            var terrain = GameObject.Find(TerrainName);
            terrain.transform.position = Vector3.one;
        }

        private void InstantiateMazeObject(MazeType mazeType)
        {
            this.MazeType = mazeType;
            switch (mazeType)
            {
                case MazeType.PREHISTORIC_MAZE:
                case MazeType.NECROPOLIS_MAZE:
                case MazeType.WORMSWORLD_MAZE:
                    this.Maze = new Maze();
                    break;
                case MazeType.SWAMP_MAZE:
                    this.Maze = new CellularAutomata();
                    break;
                case MazeType.NONE:
                    throw new Exception("Maze Type not specified!");
            }

            this.Maze.GenerateMaze(this.Length, this.Width);
            this.MazeConverter = new MazeConverter(
                this.Maze,
                this.mazeWallScale,
                this.TrapProbability,
                this.MonsterProbability);
        }

        private void GenerateMazeComponents()
        {
            foreach (var mazeComponent in this.MazeConverter.MazeComponents)
            {
                var componentType = mazeComponent.Value;
                var componentVector = mazeComponent.Key;
                switch (componentType)
                {
                    case MazeComponentType.WALL:
                        this.InstantiateMazeComponent(componentVector, WallPrefab);
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
                        this.exitComponent = this.InstantiateMazeComponent(componentVector, ExitPrefab);
                        break;
                    default:
                        break;
                }
            }
        }

        private GameObject InstantiateMazeComponent(Vector3 vector, GameObject prefab)
        {
            GameObject mazeObject = new GameObject();
            try
            {
                mazeObject = Instantiate(prefab,
                    vector,
                    prefab.transform.rotation) as GameObject;
                mazeObject.transform.parent = this.transform;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return mazeObject;
        }

        private void ChangeMazeScale()
        {
            this.transform.localScale = MAZE_SCALE;
            Player.transform.localPosition = Vector3.Scale(this.MazeConverter.PlayerCoords, MAZE_SCALE);
            var terrain = GameObject.Find(TerrainName);
            terrain.transform.position = TerrainTransform;
        }

        private IEnumerator CheckIfCorrect()
        {
            Vector3 terrainPosition = GameObject.Find(TerrainName).transform.position;
            Vector3 startPosition = this.Player.transform.position;
            Vector3 exitPosition = this.exitComponent.transform.position;
            startPosition.y = terrainPosition.y;
            exitPosition.y = terrainPosition.y;

            NavMeshPath navMeshPath = new NavMeshPath();
            yield return 0;

            NavMesh.CalculatePath(startPosition, exitPosition, NavMesh.AllAreas, navMeshPath);
            for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
            {
                Debug.DrawLine(navMeshPath.corners[i], navMeshPath.corners[i + 1], Color.cyan, 2f, false);
            }

            if (navMeshPath.status != NavMeshPathStatus.PathComplete)
            {
                Debug.Log("POWTARZAM GENERACJE");
                foreach (var wall in GameObject.FindGameObjectsWithTag("MazeComponent"))
                {
                    DestroyImmediate(wall);
                }
                this.GenerateMaze(this.MazeType);
            }
        }
    }
}
