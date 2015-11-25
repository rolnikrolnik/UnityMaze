using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Interfaces;
using System;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Abstract
{
    public class LevelManager : MonoBehaviour
    {
        #region SCENE REFERENCES

        public GameObject LevelRootObject;
        public GameObject[] ObjectsWhichNeedActivation;
        public MonoBehaviour[] ObjectsWhichNeedInitiation;

        #endregion

        public IEnumerator Activate()
        {
            for (int i = 0; i < ObjectsWhichNeedActivation.Length; i++)
            {
                ObjectsWhichNeedActivation[i].SetActive(true);
                yield return 0;
            }
        }

        public IEnumerator Init()
        {
			for (int i = 0; i < ObjectsWhichNeedInitiation.Length; i++)
            {
                (ObjectsWhichNeedInitiation[i] as IInitiation).Init();
                yield return 0;
            }
        }

        public virtual void MoveUIToCanvas()
        {

        }

        protected void MovePopupToCanvas(Transform popup)
        {
            popup.SetParent(SceneManager.Instance.PagesContainer);
            popup.localRotation = Quaternion.identity;
            popup.localScale = Vector3.one;
            popup.localPosition = Vector3.zero;
            RectTransform rectTransform = popup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            popup.gameObject.SetActive(false);
        }
    }
}
