using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Controllers
{
    public class LoadingPageController : MonoBehaviour
    {
        #region SCENE REFERENCES

        //Self Components
        public GameObject GameObject;

        //Other Gameobjects Components
        public Image Background;

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
                Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, isShow ? time / animationTime : (1 - time / animationTime));
                yield return 0;
            }
            GameObject.SetActive(isShow);
        }
    }
}
