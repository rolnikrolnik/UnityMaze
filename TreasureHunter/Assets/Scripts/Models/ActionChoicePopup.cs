using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Utils;
using Treasure_Hunter.Models;
using System.Collections.Generic;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Controllers
{
    public class ActionChoicePopup : MonoBehaviour
    {
        #region CLASS SETTINGS

        private const float ANIMATION_TIME = 0.5f;

        #endregion

        #region SCENE REFERENCES

        //Standalone
        public Image StandaloneBackground;
        public ActionItem[] StandaloneItems;
        public ActionItem StandaloneSelectedItem;
        //OVR
        public Image OVRBackground;
        public ActionItem[] OVRItems;
        public ActionItem OVRSelectedItem;

        #endregion

        private Coroutine currentCoroutine;
        private List<PlayerAction> availableActions;
        private int SelectedItemNumber;

        #region ANIMATIONS

        private IEnumerator AlphaAnimation(bool isShowing)
        {
            float startAlpha = OVRBackground.color.a;
            float animationTime = ANIMATION_TIME;
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
        private void SetAlphaChannel(float alpha)
        {
            for (int i = 0; i < StandaloneItems.Length; i++)
            {
                StandaloneItems[i].SetAlphaChannel(alpha);
                OVRItems[i].SetAlphaChannel(alpha);
            }
            StandaloneBackground.SetAlphaChannel(alpha);
            StandaloneSelectedItem.SetAlphaChannel(alpha);
            OVRBackground.SetAlphaChannel(alpha);
            OVRSelectedItem.SetAlphaChannel(alpha);
        }

        #endregion

        #region ACTIONS SERVICE

        public void AddAction(PlayerAction _action)
        {
            if (!availableActions.Exists(action => action.Type == _action.Type))
            {
                availableActions.Add(_action);
                AsignActionsToItems();
            }
        }

        public void RemoveAction(ActionType _type)
        {
            availableActions.RemoveAll(action=>action.Type==_type);
        }

        public void SelectAsMain(PlayerAction model)
        {
            if(StandaloneSelectedItem.Model.Type!=model.Type)
            {
                SelectedItemNumber = availableActions.FindIndex(action => action.Type == model.Type);
                StandaloneSelectedItem.AddModel(availableActions[SelectedItemNumber]);
                OVRSelectedItem.AddModel(availableActions[SelectedItemNumber]);
            }
        }

        private void AsignActionsToItems()
        {
            for(int i = 0;i<StandaloneItems.Length;i++)
            {
                if (i < availableActions.Count)
                {
                    StandaloneItems[i].AddModel(availableActions[i]);
                    OVRItems[i].AddModel(availableActions[i]);
                }
                else
                {
                    StandaloneItems[i].AddModel(null);
                    OVRItems[i].AddModel(null);
                }
            }
            SelectedItemNumber = 0;
            StandaloneSelectedItem.AddModel(availableActions.Count>0 ? availableActions[0] : null);
            OVRSelectedItem.AddModel(availableActions.Count > 0 ? availableActions[0] : null); 
        }

        #endregion

        public PlayerAction SelectAction()
        {
            Hide();
            return StandaloneSelectedItem.Model;
        }

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

        public void Init()
        {
            availableActions = new List<PlayerAction>(); //wczytać informacje o posiadanych przedmiotach (z actionManagera, ktory to wczyta z playerprefsow?)
            AsignActionsToItems();
        }
    }
}
