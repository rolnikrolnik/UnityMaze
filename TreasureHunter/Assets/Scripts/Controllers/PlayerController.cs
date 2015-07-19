using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        #region SCENE REFERENCES

        //Self Components
        public GameObject GameObject;
        public ThirdPersonUserControl ThirdPersonUserControl;
        public ThirdPersonCharacter ThirdPersonCharacter;
        public Rigidbody RigidBody;
        public CapsuleCollider Collider;
        public Animator Animator;

        //Other Gameobjects Components
        public CameraController PlayerCamera;
        public Transform CameraPosition;

        #endregion

        #region INITIALIZATION

        public void Init()
        {
            StartCoroutine(InitPlayer(SceneManager.CAMERA_ANIMTION));
        }

        public IEnumerator InitPlayer(float animationTime)
        {
            SceneManager.Instance.Camera.Transform.parent = CameraPosition;
            Vector3 cameraPosition = SceneManager.Instance.Camera.Transform.localPosition;
            Vector3 cameraRotation = SceneManager.Instance.Camera.Transform.localRotation.eulerAngles;
            SceneManager.Instance.Camera.Deactivate();
            for (float time = 0; time < animationTime; time += Time.deltaTime)
            {
                float factor = time / animationTime;
                SceneManager.Instance.Camera.Transform.localPosition = Vector3.Lerp(cameraPosition, Vector3.zero, factor);
                SceneManager.Instance.Camera.Transform.localRotation = Quaternion.Euler(Vector3.Lerp(cameraRotation, Vector3.zero, factor));
                yield return 0;
            }
            SceneManager.Instance.Camera.Transform.localPosition = Vector3.zero;
            SceneManager.Instance.Camera.Transform.localRotation = Quaternion.Euler(Vector3.zero);
            PlayerCamera.GameObject.SetActive(true);
            SceneManager.Instance.Camera.GameObject.SetActive(false);
            ThirdPersonUserControl.enabled = true;
            ThirdPersonCharacter.enabled = true;
            Collider.enabled = true;
            Animator.enabled = true;
            RigidBody.isKinematic = false;
        }

        #endregion
    }
}
