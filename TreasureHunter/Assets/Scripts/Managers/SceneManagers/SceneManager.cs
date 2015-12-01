using UnityEngine;
using System.Collections;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Controllers;

namespace Treasure_Hunter.Managers
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; protected set; }

        #region TIME_CONSTANTS

        public const float POPUP_ANIMATION_TIME = 0.5f;
        public const float LOADING_PAGE_ANIMATION = 1;
        public const float CAMERA_ANIMTION = 2;


        #endregion

        #region SCENE REFERENCES

        //Self Components
        public GameObject GameObject;

        //Other Gameobjects Components
        public Transform PagesContainer;
        public CameraController Camera;
        public LoadingPage LoadingPage;
        public Skybox[] Skyboxes;

        public BaseManager BaseManager;
        public MazeManager MazeManager;

        #endregion

        #region PROJECT REFERENCES

        public Material BaseSkybox;
        public Material MazeSkybox;

        #endregion
        
        #region MONO BEHAVIOUR

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(GameObject);
        }

        private void Start()
        {
            PlayerPrefsManager.Instance.Init();
            StartCoroutine(LoadBase());
        }

		private void OnApplicationQuit() 
		{
			PlayerPrefsManager.Instance.SaveAchievements();
		}

        #endregion

        private IEnumerator LoadBase()
        {
            Camera.InitCameraInTheBase();
            yield return 0;
            Application.LoadLevelAdditive((int)LevelEnums.BaseLevel);
            while (BaseManager == null)
            {
                yield return 0;
                BaseManager = FindObjectOfType<BaseManager>();
            }
            for (int i = 0; i < Skyboxes.Length; i++)
            {
                Skyboxes[i].material = BaseSkybox;
            }
            yield return StartCoroutine(BaseManager.Activate());
            LoadingPage.Hide();
            BaseManager.MoveUIToCanvas();
            yield return new WaitForSeconds(LOADING_PAGE_ANIMATION);
            yield return StartCoroutine(BaseManager.Init());
        }

        public void BackToBase()
        {
            StartCoroutine(BackToBaseCoroutine());
        }

        public void LoadMaze(MazeType mazeType)
        {
            StartCoroutine(LoadMazeCoroutine(mazeType));
        }

        private IEnumerator BackToBaseCoroutine()
        {
            LoadingPage.Show();
            yield return new WaitForSeconds(LOADING_PAGE_ANIMATION);
            PlayerPrefsManager.Instance.SaveAchievements();
            Camera.transform.SetParent(null);
            while (MazeManager == null)
            {
                yield return 0;
                MazeManager = FindObjectOfType<MazeManager>();
            }
            Destroy(MazeManager.EndGamePopup.gameObject);
            Destroy(MazeManager.ActionChoicePopup.gameObject);
            Destroy(MazeManager.LevelRootObject.gameObject);
            yield return StartCoroutine(LoadBase());
        }

        private IEnumerator LoadMazeCoroutine(MazeType mazeType)
        {
            yield return new WaitForSeconds(POPUP_ANIMATION_TIME);
            LoadingPage.Show();
            yield return new WaitForSeconds(LOADING_PAGE_ANIMATION);
            Camera.InitCameraInTheMaze();
            Application.LoadLevelAdditive((int)LevelEnums.MazeLevel);
            while (BaseManager == null)
            {
                yield return 0;
                BaseManager = FindObjectOfType<BaseManager>();
            }
            Destroy(BaseManager.EndGamePopup.gameObject);
            Destroy(BaseManager.MazeChoicePopup.gameObject);
            Destroy(BaseManager.AchievementsPopup.gameObject);
            Destroy(BaseManager.LevelRootObject.gameObject);
            while (MazeManager == null)
            {
                yield return 0;
                MazeManager = FindObjectOfType<MazeManager>();
            }
            for (int i = 0; i < Skyboxes.Length; i++)
            {
                Skyboxes[i].material = MazeSkybox;
            }
            yield return StartCoroutine(MazeManager.GenerateMaze(mazeType));
            yield return StartCoroutine(MazeManager.Activate());
            LoadingPage.Hide();
            MazeManager.MoveUIToCanvas();
            yield return new WaitForSeconds(LOADING_PAGE_ANIMATION);
            yield return StartCoroutine(MazeManager.Init());
        }
    }
}