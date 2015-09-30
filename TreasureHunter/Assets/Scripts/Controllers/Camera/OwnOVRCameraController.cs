using UnityEngine;
using System.Collections.Generic;

namespace Treasure_Hunter.Controllers
{
    public class OwnOVRCameraController : OVRComponent
    {
        #region SCENE REFERENCES

        public Camera UICameraLeft;
        public Camera UICameraRight;
        public Camera CameraLeft;
        public Camera CameraRight;

        #endregion

        #region PROPERTIES

        [SerializeField]
        private float ipd = 0.064f; // in millimeters
        public float IPD
        {
            get { return ipd; }
            set { ipd = value; UpdateCamerasDirtyFlag = true; }
        }
        [SerializeField]
        private float verticalFOV = 90.0f; // in degrees
        public float VerticalFOV
        {
            get { return verticalFOV; }
            set { verticalFOV = value; UpdateCamerasDirtyFlag = true; }
        }
        [SerializeField]
        private Color backgroundColor = new Color(0.192f, 0.302f, 0.475f, 1.0f);
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; UpdateCamerasDirtyFlag = true; }
        }
        [SerializeField]
        private float nearClipPlane = 0.15f;
        public float NearClipPlane
        {
            get { return nearClipPlane; }
            set { nearClipPlane = value; UpdateCamerasDirtyFlag = true; }
        }
        [SerializeField]
        private float farClipPlane = 1000.0f;
        public float FarClipPlane
        {
            get { return farClipPlane; }
            set { farClipPlane = value; UpdateCamerasDirtyFlag = true; }
        }

        #endregion

        public Vector3 CameraRootPosition = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 NeckPosition = new Vector3(0.0f, 0.7f, 0.0f);
        public Vector3 EyeCenterPosition = new Vector3(0.0f, 0.15f, 0.09f);
        public Transform FollowOrientation = null;
        public bool UsePlayerEyeHeight = false;
        public bool TrackerRotatesY = false;
        public bool PortraitMode = false; 
        public bool EnableOrientation = true;
        public bool PredictionOn = true;
        public bool CallInPreRender = false;
        public bool WireMode = false;
        public bool LensCorrection = true;
        public bool Chromatic = true;

        private Quaternion OrientationOffset = Quaternion.identity;
        private bool UpdateCamerasDirtyFlag = false;
        private float LensOffsetLeft, LensOffsetRight = 0.0f;
        private float AspectRatio = 1.0f;
        private float DistK0, DistK1, DistK2, DistK3 = 0.0f;
        private float YRotation = 0.0f;
        private bool PrevPortraitMode = false;
        private bool PrevUsePlayerEyeHeight = false;

        #region MONO BEHAVIOUR

        new void Start()
        {
            base.Start();
            InitCameraControllerVariables();
            UpdateCamerasDirtyFlag = true;
            UpdateCameras();
            SetMaximumVisualQuality();
        }

        new void Update()
        {
            base.Update();
            UpdateCameras();
        }

        #endregion

        public void InitCameraControllerVariables()
        {
            OVRDevice.GetIPD(ref ipd);
            OVRDevice.CalculatePhysicalLensOffsets(ref LensOffsetLeft, ref LensOffsetRight);
            VerticalFOV = OVRDevice.VerticalFOV();
            AspectRatio = OVRDevice.CalculateAspectRatio();
            OVRDevice.GetDistortionCorrectionCoefficients(ref DistK0, ref DistK1, ref DistK2, ref DistK3);
            if (PortraitMode != true)
            {
                PortraitMode = OVRDevice.RenderPortraitMode();
            }
            PrevPortraitMode = false;
            if (FollowOrientation != null)
            {
                OrientationOffset = FollowOrientation.rotation;
            }
            else
            {
                OrientationOffset = transform.rotation;
            }
        }

        void UpdateCameras()
        {
            if (FollowOrientation != null)
            {
                OrientationOffset = FollowOrientation.rotation;
            }
            SetPortraitMode();
            UpdatePlayerEyeHeight();
            if (UpdateCamerasDirtyFlag == false)
            {
                return;
            }
            float distOffset = 0.5f + (LensOffsetLeft * 0.5f);
            float perspOffset = LensOffsetLeft;
            float eyePositionOffset = -IPD * 0.5f;
            ConfigureCamera(ref CameraLeft, distOffset, perspOffset, eyePositionOffset);
            ConfigureCamera(ref UICameraLeft, distOffset, perspOffset, eyePositionOffset);
            distOffset = 0.5f + (LensOffsetRight * 0.5f);
            perspOffset = LensOffsetRight;
            eyePositionOffset = IPD * 0.5f;
            ConfigureCamera(ref CameraRight, distOffset, perspOffset, eyePositionOffset);
            ConfigureCamera(ref UICameraRight, distOffset, perspOffset, eyePositionOffset);
            UpdateCamerasDirtyFlag = false;
        }

        // SetCamera
        bool ConfigureCamera(ref Camera camera, float distOffset, float perspOffset, float eyePositionOffset)
        {
            Vector3 PerspOffset = Vector3.zero;
            Vector3 EyePosition = EyeCenterPosition;
            camera.fieldOfView = VerticalFOV;
            camera.aspect = AspectRatio;
            camera.GetComponent<OVRLensCorrection>()._Center.x = distOffset;
            ConfigureCameraLensCorrection(ref camera);
            camera.nearClipPlane = NearClipPlane;
            camera.farClipPlane = FarClipPlane;
            PerspOffset.x = perspOffset;
            camera.GetComponent<OwnOVRCamera>().SetPerspectiveOffset(ref PerspOffset);
            camera.GetComponent<OwnOVRCamera>().NeckPosition = NeckPosition;
            EyePosition.x = eyePositionOffset;
            camera.GetComponent<OwnOVRCamera>().EyePosition = EyePosition;
            camera.backgroundColor = BackgroundColor;
            camera.nearClipPlane = NearClipPlane;
            camera.farClipPlane = FarClipPlane;
            return true;
        }

        // SetCameraLensCorrection
        void ConfigureCameraLensCorrection(ref Camera camera)
        {
            float distortionScale = 1.0f / OVRDevice.DistortionScale();
            float aspectRatio = OVRDevice.CalculateAspectRatio();
            float NormalizedWidth = 1.0f;
            float NormalizedHeight = 1.0f;
            OVRLensCorrection lc = camera.GetComponent<OVRLensCorrection>();
            lc._Scale.x = (NormalizedWidth / 2.0f) * distortionScale;
            lc._Scale.y = (NormalizedHeight / 2.0f) * distortionScale * aspectRatio;
            lc._ScaleIn.x = (2.0f / NormalizedWidth);
            lc._ScaleIn.y = (2.0f / NormalizedHeight) / aspectRatio;
            lc._HmdWarpParam.x = DistK0;
            lc._HmdWarpParam.y = DistK1;
            lc._HmdWarpParam.z = DistK2;
        }

        // SetPortraitMode
        void SetPortraitMode()
        {
            if (PortraitMode != PrevPortraitMode)
            {
                Rect r = new Rect(0, 0, 0, 0);
                if (PortraitMode == true)
                {
                    r.x = 0.0f;
                    r.y = 0.5f;
                    r.width = 1.0f;
                    r.height = 0.5f;
                    CameraLeft.rect = r;
                    UICameraLeft.rect = r;
                    r.x = 0.0f;
                    r.y = 0.0f;
                    r.width = 1.0f;
                    r.height = 0.499999f;
                    CameraRight.rect = r;
                    UICameraRight.rect = r;
                }
                else
                {
                    r.x = 0.0f;
                    r.y = 0.0f;
                    r.width = 0.5f;
                    r.height = 1.0f;
                    CameraLeft.rect = r;
                    UICameraLeft.rect = r;
                    r.x = 0.5f;
                    r.y = 0.0f;
                    r.width = 0.499999f;
                    r.height = 1.0f;
                    CameraRight.rect = r;
                    UICameraRight.rect = r;
                }
            }

            PrevPortraitMode = PortraitMode;
        }

        // UpdatePlayerEyeHeight
        void UpdatePlayerEyeHeight()
        {
            if ((UsePlayerEyeHeight == true) && (PrevUsePlayerEyeHeight == false))
            {
                float peh = 0.0f;
                if (OVRDevice.GetPlayerEyeHeight(ref peh) != false)
                {
                    NeckPosition.y = peh - CameraRootPosition.y - EyeCenterPosition.y;
                }
            }
            PrevUsePlayerEyeHeight = UsePlayerEyeHeight;
        }

        // SetCameras - Should we want to re-target the cameras
        public void SetCameras(ref Camera uiCameraLeft, ref Camera uiCameraRight, ref Camera cameraLeft, ref Camera cameraRight)
        {
            CameraLeft = cameraLeft;
            CameraRight = cameraRight;
            UICameraLeft = uiCameraLeft;
            UICameraRight = uiCameraRight;
            UpdateCamerasDirtyFlag = true;
        }

        // Get/SetIPD 
        public void GetIPD(ref float ipd)
        {
            ipd = IPD;
        }
        public void SetIPD(float ipd)
        {
            IPD = ipd;
            UpdateCamerasDirtyFlag = true;

        }

        //Get/SetVerticalFOV
        public void GetVerticalFOV(ref float verticalFOV)
        {
            verticalFOV = VerticalFOV;
        }
        public void SetVerticalFOV(float verticalFOV)
        {
            VerticalFOV = verticalFOV;
            UpdateCamerasDirtyFlag = true;
        }

        //Get/SetAspectRatio
        public void GetAspectRatio(ref float aspecRatio)
        {
            aspecRatio = AspectRatio;
        }
        public void SetAspectRatio(float aspectRatio)
        {
            AspectRatio = aspectRatio;
            UpdateCamerasDirtyFlag = true;
        }

        // Get/SetDistortionCoefs
        public void GetDistortionCoefs(ref float distK0,
                                       ref float distK1,
                                       ref float distK2,
                                       ref float distK3)
        {
            distK0 = DistK0;
            distK1 = DistK1;
            distK2 = DistK2;
            distK3 = DistK3;
        }
        public void SetDistortionCoefs(float distK0,
                                       float distK1,
                                       float distK2,
                                       float distK3)
        {
            DistK0 = distK0;
            DistK1 = distK1;
            DistK2 = distK2;
            DistK3 = distK3;
            UpdateCamerasDirtyFlag = true;
        }

        // Get/Set CameraRootPosition
        public void GetCameraRootPosition(ref Vector3 cameraRootPosition)
        {
            cameraRootPosition = CameraRootPosition;
        }
        public void SetCameraRootPosition(ref Vector3 cameraRootPosition)
        {
            CameraRootPosition = cameraRootPosition;
            UpdateCamerasDirtyFlag = true;
        }

        // Get/SetNeckPosition
        public void GetNeckPosition(ref Vector3 neckPosition)
        {
            neckPosition = NeckPosition;
        }
        public void SetNeckPosition(Vector3 neckPosition)
        {
            if (UsePlayerEyeHeight != true)
            {
                NeckPosition = neckPosition;
                UpdateCamerasDirtyFlag = true;
            }
        }

        // Get/SetEyeCenterPosition
        public void GetEyeCenterPosition(ref Vector3 eyeCenterPosition)
        {
            eyeCenterPosition = EyeCenterPosition;
        }
        public void SetEyeCenterPosition(Vector3 eyeCenterPosition)
        {
            EyeCenterPosition = eyeCenterPosition;
            UpdateCamerasDirtyFlag = true;
        }

        // Get/SetOrientationOffset
        public void GetOrientationOffset(ref Quaternion orientationOffset)
        {
            orientationOffset = OrientationOffset;
        }
        public void SetOrientationOffset(Quaternion orientationOffset)
        {
            OrientationOffset = orientationOffset;
        }

        // Get/SetYRotation
        public void GetYRotation(ref float yRotation)
        {
            yRotation = YRotation;
        }
        public void SetYRotation(float yRotation)
        {
            YRotation = yRotation;
        }

        // Get/SetTrackerRotatesY
        public void GetTrackerRotatesY(ref bool trackerRotatesY)
        {
            trackerRotatesY = TrackerRotatesY;
        }
        public void SetTrackerRotatesY(bool trackerRotatesY)
        {
            TrackerRotatesY = trackerRotatesY;
        }

        // GetCameraOrientationEulerAngles
        public bool GetCameraOrientationEulerAngles(ref Vector3 angles)
        {
            if (CameraRight == null)
            {
                return false;
            }
            angles = CameraRight.transform.rotation.eulerAngles;
            return true;
        }

        // GetCameraOrientation
        public bool GetCameraOrientation(ref Quaternion quaternion)
        {
            if (CameraRight == null)
            {
                return false;
            }
            quaternion = CameraRight.transform.rotation;
            return true;
        }

        // GetCameraPosition
        public bool GetCameraPosition(ref Vector3 position)
        {
            if (CameraRight == null)
            {
                return false;
            }
            position = CameraRight.transform.position;
            return true;
        }

        // Access camera

        // GetCamera
        public void GetCamera(ref Camera camera)
        {
            camera = CameraRight;
        }

        // AttachGameObjectToCamera
        public bool AttachGameObjectToCamera(ref GameObject gameObject)
        {
            if (CameraRight == null)
            {
                return false;
            }
            gameObject.transform.parent = CameraRight.transform;
            return true;
        }

        // DetachGameObjectFromCamera
        public bool DetachGameObjectFromCamera(ref GameObject gameObject)
        {
            if ((CameraRight != null) && (CameraRight.transform == gameObject.transform.parent))
            {
                gameObject.transform.parent = null;
                return true;
            }
            return false;
        }

        // Get Misc. values from CameraController

        // GetPlauerEyeHeight
        public bool GetPlayerEyeHeight(ref float eyeHeight)
        {
            eyeHeight = CameraRootPosition.y + NeckPosition.y + EyeCenterPosition.y;
            return true;
        }

        // SetMaximumVisualQuality
        public void SetMaximumVisualQuality()
        {
            QualitySettings.softVegetation = true;
            QualitySettings.maxQueuedFrames = 0;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            QualitySettings.vSyncCount = 1;
        }
    }
}

