using UnityEngine;
using System.Collections;
using Treasure_Hunter.Utils;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Controllers
{
    public class FinalRoomController : MonoBehaviour
    {
        #region CLASS SETTINGS

        private const float ANIMATION_TIME = 3.0f;
        private const float ROTATION_ANGLE = 120;
        #endregion

        #region SCENE REFERENCES

        public GameObject CongratulationText;
        public ParticleSystem EndGameParticles;

        #endregion

        #region MONO BEHAVIOUR

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == Globals.PLAYER_TAG)
            {
                StartCoroutine(WonMaze());
            }
        }

        #endregion
        private IEnumerator WonMaze()
        {
            PlayerPrefsManager.Instance.Achievements.AddWonMaze(SceneManager.Instance.MazeManager.MazeType);
            yield return StartCoroutine(WinningAnimation());
            SceneManager.Instance.BackToBase();
        }

        private IEnumerator WinningAnimation()
        {
            CongratulationText.SetActive(true);
            EndGameParticles.Play();
            for(float time = 0;time<ANIMATION_TIME;time+=Time.deltaTime)
            {
                CongratulationText.transform.rotation = Quaternion.Euler(0, time * ROTATION_ANGLE, 0);
                yield return 0;
            }
        }
    }
}