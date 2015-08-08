using UnityEngine;
using System.Collections;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Controllers
{
    public class OwnOVRCamera : OVRCamera
    {
        public Camera Camera;
        public OVRCameraController OVRCameraController;
        private Quaternion CameraOrientation = Quaternion.identity;

        public void Init()
        {
            Start();
			//Component should be disabled. Orientation is set only at the beginning
            SetOrientation();
        }

        private void SetOrientation()
        {
            Quaternion q = Quaternion.identity;
            Vector3 dir = Vector3.forward;

            if (Camera.depth == 0.0f)
            {
                if (OVRCameraController.TrackerRotatesY == true)
                {

                    Vector3 a = Camera.transform.rotation.eulerAngles;
                    a.x = 0;
                    a.z = 0;
                    transform.parent.transform.eulerAngles = a;
                }
                if (OVRCameraController != null)
                {
                    if (OVRCameraController.EnableOrientation == true)
                    {
                        if (OVRCameraController.PredictionOn == false)
                            OVRDevice.GetOrientation(0, ref CameraOrientation);
                        else
                            OVRDevice.GetPredictedOrientation(0, ref CameraOrientation);
                    }
                }
                OVRDevice.ProcessLatencyInputs();
            }
            float yRotation = 0.0f;
            OVRCameraController.GetYRotation(ref yRotation);
            q = Quaternion.Euler(0.0f, yRotation, 0.0f);
            dir = q * Vector3.forward;
            q.SetLookRotation(dir, Vector3.up);
            Quaternion orientationOffset = Quaternion.identity;
            OVRCameraController.GetOrientationOffset(ref orientationOffset);
            q = orientationOffset * q;
            if (OVRCameraController != null)
                q = q * CameraOrientation;
            Camera.transform.rotation = q;
            Camera.transform.position =
            Camera.transform.parent.transform.position + NeckPosition;
            Camera.transform.position += q * EyePosition;	
        }
    }
}
