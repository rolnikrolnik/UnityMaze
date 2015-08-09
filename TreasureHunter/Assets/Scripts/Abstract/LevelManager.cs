using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Interfaces;
using System;

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
    }
}
