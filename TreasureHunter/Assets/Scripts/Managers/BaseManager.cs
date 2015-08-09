using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Abstract;

namespace Treasure_Hunter.Managers
{
    public class BaseManager : LevelManager
    {
        #region SCENE REFERENCES

        public MazeChoicePopup MazeChoicePopup;

        #endregion

        public override void MoveUIToCanvas()
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
