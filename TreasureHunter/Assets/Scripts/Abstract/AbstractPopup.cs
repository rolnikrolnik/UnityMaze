using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Utils;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Abstract
{
    public abstract class AbstractPopup : MonoBehaviour
    {
        #region SCENE REFERENCES

        //Standalone
        public Image StandaloneBackground;
        //OVR
        public Image OVRBackground;

        #endregion

        private Coroutine currentCoroutine;

        #region ANIMATIONS

        private IEnumerator AlphaAnimation(bool isShowing)
        {
            float startAlpha = OVRBackground.color.a;
            float animationTime = SceneManager.POPUP_ANIMATION_TIME;
            if (isShowing)
            {
                for (float time = startAlpha * animationTime; time < animationTime; time += Time.deltaTime)
                {
                    SetAlphaChannel(time / animationTime);
                    yield return 0;
                }
                SetAlphaChannel(1);
            }
            else
            {
                for (float time = (1 - startAlpha) * animationTime; time < animationTime; time += Time.deltaTime)
                {
                    SetAlphaChannel(1 - time / animationTime);
                    yield return 0;
                }
                SetAlphaChannel(0);
                gameObject.SetActive(false);
            }
        }

        protected virtual void SetAlphaChannel(float alpha)
        {
            StandaloneBackground.SetAlphaChannel(alpha);
            OVRBackground.SetAlphaChannel(alpha);
        }

        #endregion

        public void Show()
        {
            gameObject.SetActive(true);
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(AlphaAnimation(true));
        }

        public void Hide()
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(AlphaAnimation(false));
        }
    }
}
