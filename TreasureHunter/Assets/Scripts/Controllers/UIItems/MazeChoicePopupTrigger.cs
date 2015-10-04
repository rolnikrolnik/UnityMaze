using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Managers;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Utils;

namespace Treasure_Hunter.Controllers
{
    public class MazeChoicePopupTrigger : MonoBehaviour
    {
        #region CLASS SETTINGS

        private static Vector3 POSITION_CORRECTION = new Vector3(73.2f, 0, -113.22f);

        #endregion

        #region SCENE REFERENCES

        public MazeChoicePopup MazeChoicePopup;

        #endregion

        private MazeType mazeType;

        #region MONO BEHAVIOUR

        private void Start()
        {
            if (MazeChoicePopup == null)
            {
                MazeChoicePopup = FindObjectOfType<MazeChoicePopup>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == Globals.PLAYER_TAG)
            {
                if (MazeChoicePopup==null)
                {
                    MazeChoicePopup = FindObjectOfType<MazeChoicePopup>();
                }
                MazeChoicePopup.Show();
                MazeChoicePopup.CheckMazeType(mazeType);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == Globals.PLAYER_TAG)
            {
                Vector3 position = transform.position + POSITION_CORRECTION;
                Vector3 playerPosition = other.transform.position;
                MazeType correctType = SetMazeDependingOnThePlayerPosition(position-playerPosition);
                if(correctType!=mazeType)
                {
                    mazeType = correctType;
                    MazeChoicePopup.CheckMazeType(mazeType);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Globals.PLAYER_TAG)
            {
                MazeChoicePopup.Hide();
            }
        }

        #endregion

        #region MAZE TYPE

        private MazeType SetMazeDependingOnThePlayerPosition(Vector3 deltaPosition)
        {
            if(deltaPosition.x>0&&deltaPosition.z>0)
            {
                return MazeType.PREHISTORIC_MAZE;
            }
            else if (deltaPosition.x <= 0 && deltaPosition.z > 0)
            {
                return MazeType.NECROPOLIS_MAZE;
            }
            else if (deltaPosition.x > 0 && deltaPosition.z <= 0)
            {
                return MazeType.WORMSWORLD_MAZE;
            }
            else
            {
                return MazeType.SWAMP_MAZE;
            }
        }

        #endregion
    }
}
