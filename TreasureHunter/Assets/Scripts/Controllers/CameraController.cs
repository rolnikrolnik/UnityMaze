using UnityEngine;
using System.Collections;

namespace Treasure_Hunter.Controllers
{
    public class CameraController : MonoBehaviour
    {
        #region SCENE REFERENCES
        
        //Self Components
        public Transform Transform;
        public GameObject GameObject;

        //Other Gameobjects Components
        public OVRCamera LeftEyeCamera;
        public OVRCamera RightEyeCamera;

        #endregion

        public void Deactivate()
        {
            LeftEyeCamera.enabled = false;
            RightEyeCamera.enabled = false;
        }

        public void Activate()
        {
            LeftEyeCamera.enabled = true;
            RightEyeCamera.enabled = true;
        }
    }
}
