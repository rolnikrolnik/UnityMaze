using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Managers;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Utils;

namespace Treasure_Hunter.Controllers
{
    public class MazeChoicePopupController : MonoBehaviour
    {
        #region CLASS SETTINGS

        private static Vector3 POSITION_CORRECTION = new Vector3(73.2f, 0, -113.22f);
        private const float ANIMATION_TIME = 0.5f;

        #endregion

        #region SCENE REFERENCES

        public MazeChoicePopup MazeChoicePopup;

        #endregion

        private MazeType mazeType;
        private Coroutine currentCoroutine;

        #region MONO BEHAVIOUR

        void OnTriggerEnter(Collider other)
        {
            if (MazeChoicePopup==null)
            {
                MazeChoicePopup = FindObjectOfType<MazeChoicePopup>();
            }
            if (other.tag == Globals.PLAYER_TAG)
            {
                if(currentCoroutine!=null)
                {
                    StopCoroutine(currentCoroutine);
                }
                MazeChoicePopup.gameObject.SetActive(true);
                currentCoroutine = StartCoroutine(AlphaAnimation(true));
            }
        }

        void OnTriggerStay(Collider other)
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

        void OnTriggerExit(Collider other)
        {
            if (other.tag == Globals.PLAYER_TAG)
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }
                currentCoroutine = StartCoroutine(AlphaAnimation(false));
            }
        }

        #endregion

        #region MAZE TYPE

        private MazeType SetMazeDependingOnThePlayerPosition(Vector3 deltaPosition)
        {
            if(deltaPosition.x>0&&deltaPosition.z>0)
            {
                return MazeType.SwampMaze;
            }
            else if (deltaPosition.x <= 0 && deltaPosition.z > 0)
            {
                return MazeType.WormsWorldMaze;
            }
            else if (deltaPosition.x > 0 && deltaPosition.z <= 0)
            {
                return MazeType.PrehistoricMaze;
            }
            else
            {
                return MazeType.NecropolisMaze;
            }
        }

        #endregion

        #region ANIMATIONS

        private IEnumerator AlphaAnimation(bool isShowing)
        {
            float startAlpha = MazeChoicePopup.OVRBackground.color.a;
            for (float time = isShowing ?startAlpha:0; time < ANIMATION_TIME; time += Time.deltaTime)
            {
                float alpha = isShowing ? time / ANIMATION_TIME : (1 - time / ANIMATION_TIME) * startAlpha;
                MazeChoicePopup.SetAlphaChannel(alpha);
                yield return 0;
            }
            MazeChoicePopup.SetAlphaChannel(isShowing ? 1 : 0);
            MazeChoicePopup.gameObject.SetActive(isShowing);
        }

        #endregion

        #region INPUT HANDLING

        public void OnPlayClick()
        {
            SceneManager.Instance.LoadMaze(mazeType);
        }

        #endregion
    }
}
