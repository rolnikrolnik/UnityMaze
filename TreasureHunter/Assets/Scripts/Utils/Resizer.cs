using UnityEngine;
using UnityEngine.UI;

namespace Treasure_Hunter.Utils
{
    public class Resizer : MonoBehaviour
    {
        #region SCENE REFERENCES

        public RectTransform ResizerTransform;

        #endregion

        public float WidthToHeightRatio = 1;
        private float savedScreenWidth = 0;
        private Vector3 originalScale;
        private void Start()
        {
            originalScale = ResizerTransform.localScale;
            Resize();
        }

        private void Update()
        {
            if (Screen.width != Mathf.RoundToInt(savedScreenWidth))
            {
                Resize();
            }
        }

        private void Resize()
        {
            float screenWidth = (float)Screen.width;
            float screenHeight = (float)Screen.height;
            float currentWidth = screenHeight * WidthToHeightRatio;
            ResizerTransform.localScale = new Vector3((screenWidth / currentWidth) * originalScale.x, originalScale.y, originalScale.z);
            savedScreenWidth = Screen.width;
        }
    }
}