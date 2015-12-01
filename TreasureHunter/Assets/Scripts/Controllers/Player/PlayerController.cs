using UnityEngine;
using System;
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
        private const float PLAYER_HEIGHT = 2;
        private const float JUMP_STRENGTH = 1;
        private const float FULL_ANGLE = 360;
        private const float ROTATION = 1;
        private const float SPINE_X = 30;
        private const float MIN_SPINE_Y = 50;
        private const float MAX_SPINE_Y = 120;
        private const float OCULUS_MAX_X_ROTATION = 45;
        private const float OCULUS_MAX_Y_ROTATION = 30;

        #endregion

        #region SCENE REFERENCES

        //Self Components
        public GameObject GameObject;
        public CharacterController ChController;
        public Animator Animator;
        public PlayerAttack PlayerAttack;
        public MovementControllers[] MovementControllers;
        public Transform Spine;
        public GameObject MovementControllersParent;
        
        //Other Gameobjects Components
        public Transform CameraPosition;

        #endregion

        public bool IsEnabled { get; set; }
        private bool _isAnyPopupIsVisible = false;
        public bool AnyPopupIsVisible { get { return _isAnyPopupIsVisible; } set { _isAnyPopupIsVisible = value; MovementControllersParent.SetActive(!_isAnyPopupIsVisible); } }
        private bool Attack = false;
        private bool jump = false;
        private float jumpForce = 0;
        private float speed = 0;
        private float verticalSpeed = 0;
        private Vector3 startRotation = new Vector3(0, 75, 0);
        private Vector3 currentRotation = new Vector3(0, 75, 0);

        public AudioClip OuchAudioClip;
        public AudioClip JumpAudioClip;
        public AudioClip StepAudioClip;
        public AudioClip AttackAudioClip;

        private AudioSource _audioSource;

        private bool isGrounded
        {
            get
            {
                RaycastHit hit;
                return Physics.Raycast(transform.position+new Vector3(0,0.1f,0), -transform.forward, out hit, PLAYER_HEIGHT);
            }
        }

        #region MONO BEHAVIOUR

        void Start()
        {
            _audioSource = GetComponent<AudioSource>(); 
        }

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
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (SceneManager.Instance.MazeManager != null)
                    {
                        SceneManager.Instance.MazeManager.EndGamePopup.SetMessage("Do you really want go back to the base?");
                        SceneManager.Instance.MazeManager.EndGamePopup.Show();
                        AnyPopupIsVisible = true;
                    }
                    else if (SceneManager.Instance.BaseManager != null)
                    {
                        SceneManager.Instance.BaseManager.EndGamePopup.SetMessage("Do you really want leave the game?");
                        SceneManager.Instance.BaseManager.EndGamePopup.Show();
                        AnyPopupIsVisible = true;
                    }
                }
            }
        }

        private void LateUpdate()
        {
            if (IsEnabled)
            {
                if (SceneManager.Instance.Camera.currentDisplayMode == DisplayMode.OVRCamera)
                {
                    ApplyOculusRiftMotion();
                }
                else
                {
                    ApplyMouseMovement();
                }
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
            if (Attack)
            {
                Attack = Input.GetKey(KeyCode.Mouse0);
                Animator.SetBool(ATTACK_ANIMATION_PARAMETER_NAME, Attack);
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
                _audioSource.PlayOneShot(JumpAudioClip);
                PlayerPrefsManager.Instance.Achievements.AddPerformedAction(ActionType.JUMP);
            }
        }

        private void ApplyAttack()
        {
            Animator.SetBool(ATTACK_ANIMATION_PARAMETER_NAME, true);
            Attack = true;
        }

        private void ApplyMouseMovement()
        {
            Vector3 rotationChange = Vector3.zero;
            if(!AnyPopupIsVisible)
            {
                for(int i=0;i<MovementControllers.Length;i++)
                {
                    if(MovementControllers[i].IsActive)
                    {
                        rotationChange += ApplyMovementInDirection(MovementControllers[i].Direction);
                    }
                }
            }
            currentRotation = currentRotation + rotationChange;
            currentRotation = new Vector3(currentRotation.x > SPINE_X ? SPINE_X : currentRotation.x < -SPINE_X ? -SPINE_X : currentRotation.x,
                                          currentRotation.y > MAX_SPINE_Y ? MAX_SPINE_Y : currentRotation.y < MIN_SPINE_Y ? MIN_SPINE_Y : currentRotation.y,
                                          0);
            Spine.localRotation = Quaternion.Euler(currentRotation);
        }

        private void ApplyOculusRiftMotion()
        {
            Quaternion CameraOrientation = Quaternion.identity;
            OVRDevice.GetOrientation(0, ref CameraOrientation);
            float yRotation = CameraOrientation.eulerAngles.x<180?
                              (CameraOrientation.eulerAngles.x < OCULUS_MAX_Y_ROTATION ? CameraOrientation.eulerAngles.x : OCULUS_MAX_Y_ROTATION) :
                              (CameraOrientation.eulerAngles.x - 360 > -OCULUS_MAX_Y_ROTATION ? CameraOrientation.eulerAngles.x - 360 : -OCULUS_MAX_Y_ROTATION);
            float xRotation = CameraOrientation.eulerAngles.y < 180 ?
                              (CameraOrientation.eulerAngles.y < OCULUS_MAX_X_ROTATION ? CameraOrientation.eulerAngles.y : OCULUS_MAX_X_ROTATION) :
                              (CameraOrientation.eulerAngles.y - 360 > -OCULUS_MAX_X_ROTATION ? CameraOrientation.eulerAngles.y - 360 : -OCULUS_MAX_X_ROTATION);
            currentRotation = new Vector3(startRotation.x - xRotation, startRotation.y + yRotation, 0);
            Spine.localRotation = Quaternion.Euler(currentRotation);
        }

        private Vector3 ApplyMovementInDirection(MovementDirections direction)
        {
            return new Vector3(direction == MovementDirections.RIGHT ? -ROTATION : direction == MovementDirections.LEFT ? ROTATION : 0, 
                               direction == MovementDirections.UP ? -ROTATION : direction == MovementDirections.DOWN?ROTATION:0,
                               0);
        }

        public void ApplyAction()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !AnyPopupIsVisible)
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

        public void PlayStepSound()
        {
            if (isGrounded)
                _audioSource.PlayOneShot(StepAudioClip, 0.5F);
        }

        public void PlaySwordSound()
        {
            _audioSource.PlayOneShot(AttackAudioClip, 0.5F);
            PlayerPrefsManager.Instance.Achievements.AddPerformedAction(ActionType.ATTACK);
            PlayerAttack.MakeAttack();
        }

        #endregion
    }
}
