using UnityEngine;
using System.Collections;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Controllers
{
    public class CameraController : MonoBehaviour
    {
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

        private void CheckCameraState()
        {
            bool debug = false;// Input.GetKeyDown(KeyCode.Mouse0);
//            if (debug)
//            {
                if (currentDisplayMode == DisplayMode.OVRCamera && OVRDevice.SensorCount <= 0)
                {
                    SetStandaloneCameraAsMain();
                }
                else if (currentDisplayMode == DisplayMode.StandaloneCamera && OVRDevice.SensorCount > 0)
                {
                    SetOVRCameraAsMain();
                }
//            }
        }

        private void SetOVRCameraAsMain()
        {
            currentDisplayMode = DisplayMode.OVRCamera;
            UIStandaloneCamera.gameObject.SetActive(false);
            OVRCamera.SetActive(true);
            UICanvas.worldCamera = UILeftEyeCamera;
            UICanvas.renderMode = RenderMode.ScreenSpaceCamera;
            if (SceneManager.Instance.BaseManager != null && SceneManager.Instance.BaseManager.MazeChoicePopup!=null)
            {
                SceneManager.Instance.BaseManager.MazeChoicePopup.OVRBackground.gameObject.SetActive(true);
                SceneManager.Instance.BaseManager.MazeChoicePopup.StandaloneBackground.gameObject.SetActive(false);
            }
            if (SceneManager.Instance.BaseManager != null && SceneManager.Instance.MazeManager.ActionChoicePopup != null)
            {
                SceneManager.Instance.MazeManager.ActionChoicePopup.OVRBackground.gameObject.SetActive(true);
                SceneManager.Instance.MazeManager.ActionChoicePopup.StandaloneBackground.gameObject.SetActive(false);
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
            if (SceneManager.Instance.BaseManager != null && SceneManager.Instance.BaseManager.MazeChoicePopup != null)
            {
                SceneManager.Instance.BaseManager.MazeChoicePopup.OVRBackground.gameObject.SetActive(false);
                SceneManager.Instance.BaseManager.MazeChoicePopup.StandaloneBackground.gameObject.SetActive(true);
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
