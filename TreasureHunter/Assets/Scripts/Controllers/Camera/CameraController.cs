using UnityEngine;
using System.Collections;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Controllers
{
    public class CameraController : MonoBehaviour
    {
        #region CLASS SETTINGS

        private static Vector3 MAZE_START_CAMERA_POSITION = new Vector3(-13, 104, -5);
        private static Vector3 MAZE_START_CAMERA_ROTATION = new Vector3(30, 70, 0);
        private static Vector3 BASE_START_CAMERA_POSITION = new Vector3(71.19169f, 60, 204.5f);
        private static Vector3 BASE_START_CAMERA_ROTATION = new Vector3(30, 60, 0);

        #endregion

        #region SCENE REFERENCES

        //Self Components
        public Transform Transform;
        public GameObject GameObject;

        //Other Gameobjects Components
        public OwnOVRCamera UILeftOVRCamera;
        public OwnOVRCamera UIRightOVRCamera;
        public GameObject OVRCamera;
        public Camera UILeftEyeCamera;
        public Camera UIStandaloneCamera;
        public Canvas UICanvas;

        #endregion

        public DisplayMode currentDisplayMode = DisplayMode.OVRCamera;

        #region MONO BEHAVIOUR

        private void Start()
        {
            StartCoroutine(SetRenderModeToWorldAfterShortDelay());
            StartCoroutine(SetOVRCamerasOrientationAfterShortDelay());
            
        }

        private void Update()
        {
            CheckCameraState(); 
        }

        #endregion

        public void InitCameraInTheBase()
        {
            transform.parent = null;
            transform.localPosition = BASE_START_CAMERA_POSITION;
            transform.localRotation = Quaternion.Euler(BASE_START_CAMERA_ROTATION);
        }

        public void InitCameraInTheMaze()
        {
            transform.parent = null;
            transform.localPosition = MAZE_START_CAMERA_POSITION;
            transform.localRotation = Quaternion.Euler(MAZE_START_CAMERA_ROTATION);
        }

        private void CheckCameraState()
        {
            bool debug = false;// Input.GetKeyDown(KeyCode.Mouse0);
            if (debug)
            {
                if (currentDisplayMode == DisplayMode.OVRCamera && OVRDevice.SensorCount <= 0)
                {
                    SetStandaloneCameraAsMain();
                }
                else if (currentDisplayMode == DisplayMode.StandaloneCamera && OVRDevice.SensorCount > 0)
                {
                    SetOVRCameraAsMain();
                }
            }
        }

        private void SetOVRCameraAsMain()
        {
            currentDisplayMode = DisplayMode.OVRCamera;
            UIStandaloneCamera.gameObject.SetActive(false);
            OVRCamera.SetActive(true);
            UICanvas.worldCamera = UILeftEyeCamera;
            UICanvas.renderMode = RenderMode.ScreenSpaceCamera;
            SceneManager.Instance.LoadingPage.Background.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/2, Screen.height);
            if (SceneManager.Instance.BaseManager != null && SceneManager.Instance.BaseManager.MazeChoicePopup!=null)
            {
                SceneManager.Instance.BaseManager.MazeChoicePopup.OVRBackground.gameObject.SetActive(true);
                SceneManager.Instance.BaseManager.MazeChoicePopup.StandaloneBackground.gameObject.SetActive(false);
            }
            if (SceneManager.Instance.BaseManager != null && SceneManager.Instance.BaseManager.AchievementsPopup != null)
            {
                SceneManager.Instance.BaseManager.AchievementsPopup.OVRBackground.gameObject.SetActive(true);
                SceneManager.Instance.BaseManager.AchievementsPopup.StandaloneBackground.gameObject.SetActive(false);
            }
            if (SceneManager.Instance.MazeManager != null && SceneManager.Instance.MazeManager.ActionChoicePopup != null)
            {
                SceneManager.Instance.MazeManager.ActionChoicePopup.OVRBackground.transform.parent.gameObject.SetActive(true);
                SceneManager.Instance.MazeManager.ActionChoicePopup.StandaloneBackground.transform.parent.gameObject.SetActive(false);
            }
            StartCoroutine(SetRenderModeToWorldAfterShortDelay());
        }

        private void SetStandaloneCameraAsMain()
        {
            currentDisplayMode = DisplayMode.StandaloneCamera;
            UIStandaloneCamera.gameObject.SetActive(true);
            OVRCamera.SetActive(false);
            UICanvas.worldCamera = UIStandaloneCamera;
            UICanvas.renderMode = RenderMode.ScreenSpaceCamera;
            SceneManager.Instance.LoadingPage.Background.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            if (SceneManager.Instance.BaseManager != null && SceneManager.Instance.BaseManager.MazeChoicePopup != null)
            {
                SceneManager.Instance.BaseManager.MazeChoicePopup.OVRBackground.gameObject.SetActive(false);
                SceneManager.Instance.BaseManager.MazeChoicePopup.StandaloneBackground.gameObject.SetActive(true);
            }
            if (SceneManager.Instance.BaseManager != null && SceneManager.Instance.BaseManager.AchievementsPopup != null)
            {
                SceneManager.Instance.BaseManager.AchievementsPopup.OVRBackground.gameObject.SetActive(false);
                SceneManager.Instance.BaseManager.AchievementsPopup.StandaloneBackground.gameObject.SetActive(true);
            }
            if (SceneManager.Instance.BaseManager != null && SceneManager.Instance.MazeManager.ActionChoicePopup != null)
            {
                SceneManager.Instance.MazeManager.ActionChoicePopup.OVRBackground.gameObject.SetActive(false);
                SceneManager.Instance.MazeManager.ActionChoicePopup.StandaloneBackground.gameObject.SetActive(true);
            }
        }

        private IEnumerator SetRenderModeToWorldAfterShortDelay()
        {
            yield return 0;
            UICanvas.renderMode = RenderMode.WorldSpace;
        }

        private IEnumerator SetOVRCamerasOrientationAfterShortDelay()
        {
            yield return 0;
            UILeftOVRCamera.Init();
            UIRightOVRCamera.Init();
        }
    }
}
