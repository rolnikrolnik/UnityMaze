using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using Treasure_Hunter.Managers;
using Treasure_Hunter.Interfaces;
using Treasure_Hunter.Models;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Controllers
{
    public class PlayerController : MonoBehaviour, IInitiation
    {
        #region CLASS SETTINGS

        private const string ATTACK_ANIMATION_PARAMETER_NAME = "Attack";
        private const string SPEED_ANIMATION_PARAMETER_NAME = "Speed";
        private const string MOVEMENT_AXIS = "Vertical";
        private const string ROTATION_AXIS = "Horizontal";
        private const float MAX_SPEED = 0.5f;
        private const float AVERAGE_SPEED = 20;
        private const float GRAVITY = 0.5f;
        private const float MAX_ROTATION_SPEED = 3;
        private const float PLAYER_HEIGHT = 1.5f;
        private const float JUMP_STRENGTH = 1;
        private const float FULL_ANGLE = 360;

        #endregion

        #region SCENE REFERENCES

        //Self Components
        public GameObject GameObject;
        public CharacterController ChController;
        public Animator Animator;

        //Other Gameobjects Components
        public Transform CameraPosition;

        #endregion

        public bool IsEnabled { get; set; }
        private bool attack = false;
        private bool jump = false;
        private float jumpForce = 0;
        private float speed = 0;
        private float verticalSpeed = 0;

        private bool isGrounded
        {
            get
            {
                RaycastHit hit;
                return Physics.Raycast(transform.position, -transform.forward, out hit, PLAYER_HEIGHT);
            }
        }

        #region MONO BEHAVIOUR

        private void Update()
        {
            if(IsEnabled)
            {
                ApplyAction();
                CheckCurrentAction();
                speed = Input.GetAxis(MOVEMENT_AXIS) * MAX_SPEED;
                Animator.SetFloat(SPEED_ANIMATION_PARAMETER_NAME, Mathf.Abs(speed) / MAX_SPEED);
                ChController.Move((-transform.up * speed + transform.forward * verticalSpeed) * Time.deltaTime * AVERAGE_SPEED);
                transform.Rotate(0, 0, Input.GetAxis(ROTATION_AXIS) * MAX_ROTATION_SPEED);
            }
        }

        #endregion

        #region INITIALIZATION

        public void Init()
        {
            StartCoroutine(InitPlayer(SceneManager.CAMERA_ANIMTION));
        }

        public IEnumerator InitPlayer(float animationTime)
        {
            SceneManager.Instance.Camera.Transform.parent = CameraPosition;
            //set camera position
            Vector3 cameraPosition = SceneManager.Instance.Camera.Transform.localPosition;
            Vector3 cameraRotation = SceneManager.Instance.Camera.Transform.localRotation.eulerAngles;
            cameraRotation = new Vector3(cameraRotation.x>FULL_ANGLE*0.5f?(cameraRotation.x - FULL_ANGLE):cameraRotation.x,
                                         cameraRotation.y > FULL_ANGLE * 0.5f ? (cameraRotation.y - FULL_ANGLE) : cameraRotation.y,
                                         cameraRotation.z > FULL_ANGLE * 0.5f ? (cameraRotation.z - FULL_ANGLE) : cameraRotation.z);
            for (float time = 0; time < animationTime; time += Time.deltaTime)
            {
                float factor = time / animationTime;
                SceneManager.Instance.Camera.Transform.localPosition = Vector3.Lerp(cameraPosition, Vector3.zero, factor);
                SceneManager.Instance.Camera.Transform.localRotation = Quaternion.Euler(Vector3.Lerp(cameraRotation, Vector3.zero, factor));
                yield return 0;
            }
            SceneManager.Instance.Camera.Transform.localPosition = Vector3.zero;
            SceneManager.Instance.Camera.Transform.localRotation = Quaternion.Euler(Vector3.zero);
            //Activate player components
            ChController.enabled = true;
            Animator.enabled = true;
            IsEnabled = true;
        }

        public void DisablePlayer()
        {
            IsEnabled = false;
            ChController.enabled = false;
            Animator.enabled = false;
        }

        public void EnablePlayer()
        {
            ChController.enabled = true;
            Animator.enabled = true;
            IsEnabled = true;
        }

        #endregion

        #region ACTIONS

        private void CheckCurrentAction()
        {
            if (attack)
            {
                attack = Input.GetKey(KeyCode.Mouse0);
                Animator.SetBool(ATTACK_ANIMATION_PARAMETER_NAME, attack);
            }
            else if (jump)
            {
                jumpForce -= (GRAVITY + JUMP_STRENGTH)*Time.deltaTime;
                if(jumpForce<-GRAVITY)
                {
                    jump = false;
                    verticalSpeed = -GRAVITY;
                }
                verticalSpeed = jumpForce;
            }
            else
            {
                verticalSpeed = -GRAVITY;
            }
        }

        private void ApplyJump()
        {
            if (isGrounded)
            {
                jump = true;
                jumpForce = JUMP_STRENGTH;
            }
        }

        private void ApplyAttack()
        {
            Animator.SetBool(ATTACK_ANIMATION_PARAMETER_NAME, true);
            attack = true;
        }

        public void ApplyAction()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(SceneManager.Instance.MazeManager!=null)
                {
                    PlayerAction action = SceneManager.Instance.MazeManager.ActionChoicePopup.SelectAction();
                    switch(action.Type)
                    {
                        case ActionType.ATTACK:
                            ApplyAttack();
                            break;
                        case ActionType.JUMP:
                            ApplyJump();
                            break;
                    }
                }
                else
                {
                    ApplyJump();
                }
            }
        }

        #endregion

        public void MakeHit()
        {

        }
    }
}
