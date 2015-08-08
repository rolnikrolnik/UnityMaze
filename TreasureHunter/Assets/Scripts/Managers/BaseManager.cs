using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;

namespace Treasure_Hunter.Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region SCENE REFERENCES

        public GameObject Palace;
        public GameObject Terrain;
        public PlayerController Player;
        public MazeChoicePopup MazeChoicePopup;

        #endregion

        public void MoveMazeChoicePopupToCanvas()
        {
            MazeChoicePopup.transform.parent = SceneManager.Instance.PagesContainer;
            MazeChoicePopup.transform.localRotation = Quaternion.identity;
            MazeChoicePopup.transform.localScale = Vector3.one;
            MazeChoicePopup.transform.localPosition = Vector3.zero;
            RectTransform rectTransform = MazeChoicePopup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }
}
