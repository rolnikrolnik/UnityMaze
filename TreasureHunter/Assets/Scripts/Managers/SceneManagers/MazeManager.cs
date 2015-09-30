using System.Collections;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Abstract;
using Treasure_Hunter.Controllers;
using UnityEngine;

namespace Treasure_Hunter.Managers
{
    public class MazeManager : LevelManager
    {
        #region SCENE REFERENCES

        public ActionChoicePopup ActionChoicePopup;

        #endregion

        public override void MoveUIToCanvas()
        {
            ActionChoicePopup.transform.parent = SceneManager.Instance.PagesContainer;
            ActionChoicePopup.transform.localRotation = Quaternion.identity;
            ActionChoicePopup.transform.localScale = Vector3.one;
            ActionChoicePopup.transform.localPosition = Vector3.zero;
            RectTransform rectTransform = ActionChoicePopup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
        
        public IEnumerator GenerateMaze(MazeType mazeType)
        {
            //zmienić skybox'y
            yield return StartCoroutine(Activate());
        }
    }
}
