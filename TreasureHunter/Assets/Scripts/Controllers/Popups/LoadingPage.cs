using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Controllers
{
    public class LoadingPage : MonoBehaviour
    {
        #region SCENE REFERENCES

        //Self Components
        public GameObject GameObject;

        //Other Gameobjects Components
        public Image StandaloneBackground;
        public Image OVRBackground;

        #endregion

        public void Show()
        {
            GameObject.SetActive(true);
            StartCoroutine(Animation(SceneManager.LOADING_PAGE_ANIMATION, true));
        }

        public void Hide()
        {
            StartCoroutine(Animation(SceneManager.LOADING_PAGE_ANIMATION, false));
        }

        private IEnumerator Animation(float animationTime, bool isShow)
        {
            for(float time=0;time<animationTime;time+=Time.deltaTime)
            {
                StandaloneBackground.color = new Color(StandaloneBackground.color.r, StandaloneBackground.color.g, StandaloneBackground.color.b, isShow ? time / animationTime : (1 - time / animationTime));
                OVRBackground.color = new Color(OVRBackground.color.r, OVRBackground.color.g, OVRBackground.color.b, isShow ? time / animationTime : (1 - time / animationTime));
                yield return 0;
            }
            GameObject.SetActive(isShow);
        }
    }
}
