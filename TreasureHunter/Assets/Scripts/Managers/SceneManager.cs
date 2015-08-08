using UnityEngine;
using System.Collections;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Controllers;

namespace Treasure_Hunter.Managers
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }

        #region TIME_CONSTANTS

        public const float LOADING_PAGE_ANIMATION = 1;
        public const float CAMERA_ANIMTION = 2;

        #endregion

        #region SCENE REFERENCES

        //Self Components
        public GameObject GameObject;

        //Other Gameobjects Components
        public Transform PagesContainer;
        public CameraController Camera;
        public BaseManager BaseManager;
        public LoadingPageController LoadingPage;

        #endregion

        #region MONO BEHAVIOUR
        // Use this for initialization
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(GameObject);
        }

        // Update is called once per frame
        private void Start()
        {
            StartCoroutine(LoadBase());
        }

        #endregion

        private IEnumerator LoadBase()
        {
            yield return 0;
            Application.LoadLevelAdditive((int)LevelEnums.BaseLevel);
            while (BaseManager == null)
            {
                yield return 0;
                BaseManager = FindObjectOfType<BaseManager>();
            }
            BaseManager.Palace.SetActive(true);
            yield return 0;
            BaseManager.Terrain.SetActive(true);
            yield return 0;
            BaseManager.Player.GameObject.SetActive(true);
            LoadingPage.Hide();
            BaseManager.MoveMazeChoicePopupToCanvas();
            yield return new WaitForSeconds(LOADING_PAGE_ANIMATION);
            BaseManager.Player.Init();
            yield return 0;
        }

        public void LoadMaze(MazeType mazeType)
        {

        }
    }
}