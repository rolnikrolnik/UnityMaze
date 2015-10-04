using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

		private float rotationSpeed = 100.0f;
		private float walkSpeed = 20.0f;

        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
			// read inputs
			float h = CrossPlatformInputManager.GetAxis("Horizontal");
			float v = CrossPlatformInputManager.GetAxis ("Vertical");
			bool crouch = Input.GetKey (KeyCode.C);

			var translation = v * walkSpeed;
			var rotation = h * rotationSpeed;

			translation *= Time.deltaTime;
			rotation *= Time.deltaTime;

	        if (Input.GetKey (KeyCode.LeftShift)) {
				translation *= 2.0f;
			}

			//transform.Translate (0, 0, translation);
			transform.Rotate (0, rotation, 0);

			var h_mouse = rotationSpeed * Input.GetAxis ("Mouse X");
			//var v_mouse = -verticalSpeed * Input.GetAxis ("Mouse Y");
			transform.Rotate (/*v_mouse*/0, h_mouse * 0.25f, 0);

            // pass all parameters to the character control script
			m_Character.Move(translation*transform.forward, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
