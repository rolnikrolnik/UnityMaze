using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Controllers
{
    [RequireComponent(typeof(Camera))]
    public class OwnOVRCamera : OVRComponent
    {
        #region SCENE REFERENCES

        public Camera Camera;
        public OwnOVRCameraController OwnOVRCameraController;

        #endregion

        private static Quaternion CameraOrientation = Quaternion.identity;
        private RenderTexture CameraTexture = null;
        private Material ColorOnlyMaterial = null;
        private Color QuadColor = Color.red;
        private float CameraTextureScale = 1.0f;
        [HideInInspector]
        public Vector3 NeckPosition = new Vector3(0.0f, 0.0f, 0.0f);
        [HideInInspector]
        public Vector3 EyePosition = new Vector3(0.0f, 0.09f, 0.16f);

        #region MONO BEHAVIOUR

        new void Awake()
        {
            base.Awake();
            if (ColorOnlyMaterial == null)
            {
                ColorOnlyMaterial = new Material(
                    "Shader \"Solid Color\" {\n" +
                    "Properties {\n" +
                    "_Color (\"Color\", Color) = (1,1,1)\n" +
                    "}\n" +
                    "SubShader {\n" +
                    "Color [_Color]\n" +
                    "Pass {}\n" +
                    "}\n" +
                    "}"
                );
            }
        }

        new void Start()
        {
            base.Start();
            if (OwnOVRCameraController == null)
            {
                Debug.LogWarning("WARNING: OVRCameraController not found!");
            }
            if ((CameraTexture == null) && (CameraTextureScale != 1.0f))
            {
                int w = (int)(Screen.width / 2.0f * CameraTextureScale);
                int h = (int)(Screen.height * CameraTextureScale);
                if (Camera.hdr)
                {
                    CameraTexture = new RenderTexture(w, h, 24, RenderTextureFormat.ARGBFloat);
                }
                else
                {
                    CameraTexture = new RenderTexture(w, h, 24);
                }
                CameraTexture.antiAliasing = (QualitySettings.antiAliasing == 0) ? 1 : QualitySettings.antiAliasing;
            }
        }

        void OnPreCull()
        {
            if (OwnOVRCameraController.CallInPreRender == false)
            {
                SetCameraOrientation();
            }
        }

        void OnPreRender()
        {
            if (OwnOVRCameraController.CallInPreRender == true)
            {
                SetCameraOrientation();
            }
            if (OwnOVRCameraController.WireMode == true)
            {
                GL.wireframe = true;
            }
            if (CameraTexture != null)
            {
                Graphics.SetRenderTarget(CameraTexture);
                GL.Clear(true, true, Camera.backgroundColor);
            }
        }

        void OnPostRender()
        {
            if (OwnOVRCameraController.WireMode == true)
            {
                GL.wireframe = false;
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            RenderTexture SourceTexture = source;
            if (CameraTexture != null)
            {
                SourceTexture = CameraTexture;
            }
            Material material = null;
            if (OwnOVRCameraController.LensCorrection == true)
            {
                if (OwnOVRCameraController.Chromatic == true)
                {
                    material = GetComponent<OVRLensCorrection>().GetMaterial_CA(OwnOVRCameraController.PortraitMode);
                }
                else
                {
                    material = GetComponent<OVRLensCorrection>().GetMaterial(OwnOVRCameraController.PortraitMode);
                }
            }
            if (material != null)
            {
                Graphics.Blit(SourceTexture, destination, material);
            }
            else
            {
                Graphics.Blit(SourceTexture, destination);
            }
            LatencyTest(destination);
        }

        #endregion

        public void Init()
        {
            Start();
            //Component should be disabled. Orientation is set only at the beginning
            SetCameraOrientation();
        }

        void SetCameraOrientation()
        {
            Quaternion q = Quaternion.identity;
            Vector3 dir = Vector3.forward;
            if (Camera.depth == 0.0f)
            {
                if (OwnOVRCameraController.TrackerRotatesY == true)
                {
                    Vector3 a = Camera.transform.rotation.eulerAngles;
                    a.x = 0;
                    a.z = 0;
                    transform.parent.transform.eulerAngles = a;
                }
                if (OwnOVRCameraController != null)
                {
                    if (OwnOVRCameraController.EnableOrientation == true)
                    {
                        if (OwnOVRCameraController.PredictionOn == false)
                        {
                            OVRDevice.GetOrientation(0, ref CameraOrientation);
                        }
                        else
                        {
                            OVRDevice.GetPredictedOrientation(0, ref CameraOrientation);
                        }
                    }
                }
                OVRDevice.ProcessLatencyInputs();
            }
            float yRotation = 0.0f;
            OwnOVRCameraController.GetYRotation(ref yRotation);
            q = Quaternion.Euler(0.0f, yRotation, 0.0f);
            dir = q * Vector3.forward;
            q.SetLookRotation(dir, Vector3.up);
            Quaternion orientationOffset = Quaternion.identity;
            OwnOVRCameraController.GetOrientationOffset(ref orientationOffset);
            q = orientationOffset * q;
            if (OwnOVRCameraController != null)
            {
                q = q * CameraOrientation;
            }
            Camera.transform.rotation = q;
            Camera.transform.position =
            Camera.transform.parent.transform.position + NeckPosition;
            Camera.transform.position += q * EyePosition;
        }

        void LatencyTest(RenderTexture dest)
        {
            byte r = 0, g = 0, b = 0;
            string s = Marshal.PtrToStringAnsi(OVRDevice.GetLatencyResultsString());
            if (s != null)
            {
                string result =
                "\n\n---------------------\nLATENCY TEST RESULTS:\n---------------------\n";
                result += s;
                result += "\n\n\n";
                print(result);
            }
            if (OVRDevice.DisplayLatencyScreenColor(ref r, ref g, ref b) == false)
            {
                return;
            }
            RenderTexture.active = dest;
            Material material = ColorOnlyMaterial;
            QuadColor.r = (float)r / 255.0f;
            QuadColor.g = (float)g / 255.0f;
            QuadColor.b = (float)b / 255.0f;
            material.SetColor("_Color", QuadColor);
            GL.PushMatrix();
            material.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Vertex3(0.3f, 0.3f, 0);
            GL.Vertex3(0.3f, 0.7f, 0);
            GL.Vertex3(0.7f, 0.7f, 0);
            GL.Vertex3(0.7f, 0.3f, 0);
            GL.End();
            GL.PopMatrix();
        }

        public void SetPerspectiveOffset(ref Vector3 offset)
        {	
            Camera.ResetProjectionMatrix();
            Matrix4x4 om = Matrix4x4.identity;
            om.SetColumn(3, new Vector4(offset.x, offset.y, 0.0f, 1));
            if (OwnOVRCameraController != null && OwnOVRCameraController.PortraitMode == true)
            {
                Vector3 t = Vector3.zero;
                Quaternion r = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                Vector3 s = Vector3.one;
                Matrix4x4 pm = Matrix4x4.TRS(t, r, s);
                Camera.projectionMatrix = pm * om * Camera.projectionMatrix;
            }
            else
            {
                Camera.projectionMatrix = om * Camera.projectionMatrix;
            }
        }
    }
}