using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Utils;
using Treasure_Hunter.Models;
using System.Collections.Generic;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;
using Treasure_Hunter.Abstract;

namespace Treasure_Hunter.Controllers
{
    public class ActionChoicePopup : AbstractPopup
    {
        #region CLASS SETTINGS

        private const float INACTIVE_SIZE = 0.2f;
        private const float ACTIVE_SIZE = 1;

        #endregion

        #region SCENE REFERENCES

        //Standalone
        public Image StandaloneHealthBarFill;
        public Image StandaloneHealthBarBackground;
        public Text StandaloneHealthBarLabel;
        public ActionItem[] StandaloneItems;
        public ActionItem StandaloneSelectedItem;
        //OVR
        public Image OVRHealthBarFill;
        public Image OVRHealthBarBackground;
        public Text OVRHealthBarLabel;
        public ActionItem[] OVRItems;
        public ActionItem OVRSelectedItem;
        //Manager
        public ActionsManager ActionsManager;

        #endregion

        private Coroutine currentCoroutine;
        private List<PlayerAction> availableActions;
        private int SelectedItemNumber;
        private bool isActive = false;

        #region MONO BEHAVIOUR

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (!isActive)
                {
                    SetActiveSize();
                    isActive = true;
                    Time.timeScale = 0.1f;
                    if( SceneManager.Instance.MazeManager!=null)
                    {
                        SceneManager.Instance.MazeManager.Player.AnyPopupIsVisible = true;
                    }
                }
            }
            else
            {
                if (isActive)
                {
                    SetInactiveSize();
                    isActive = false;
                    Time.timeScale = 1;
                    if (SceneManager.Instance.MazeManager != null)
                    {
                        SceneManager.Instance.MazeManager.Player.AnyPopupIsVisible = false;
                    }
                }
            }
        }

        #endregion

        #region ANIMATIONS

        protected override void SetAlphaChannel(float alpha)
        {
            base.SetAlphaChannel(alpha);
            for (int i = 0; i < StandaloneItems.Length; i++)
            {
                StandaloneItems[i].SetAlphaChannel(alpha);
                OVRItems[i].SetAlphaChannel(alpha);
            }
            StandaloneSelectedItem.SetAlphaChannel(alpha);
            OVRSelectedItem.SetAlphaChannel(alpha);
            OVRHealthBarFill.SetAlphaChannel(alpha);
            OVRHealthBarBackground.SetAlphaChannel(alpha);
            OVRHealthBarLabel.SetAlphaChannel(alpha);
            StandaloneHealthBarFill.SetAlphaChannel(alpha);
            StandaloneHealthBarBackground.SetAlphaChannel(alpha);
            StandaloneHealthBarLabel.SetAlphaChannel(alpha);
        }

        #endregion

        #region ACTIONS SERVICE

        public void AddAction(PlayerAction _action)
        {
            if (availableActions==null)
            {
                availableActions = new List<PlayerAction>();
            }
            if (!availableActions.Exists(action => action.Type == _action.Type))
            {
                availableActions.Add(_action);
                ActionsManager.AddAction(_action);
                AsignActionsToItems();
            }
            else if (_action.hasLimitedCharges)
            {
                availableActions.Find(action => action.Type == _action.Type).Charges += _action.Charges;
                ActionsManager.AddChargesToAction(_action);
            }
        }

        public void RemoveAction(ActionType _type)
        {
            ActionsManager.RemoveAction(_type);
            availableActions.RemoveAll(action=>action.Type==_type);
        }

        public void SelectAsMain(PlayerAction model)
        {
            if (isActive&&StandaloneSelectedItem.Model.Type != model.Type)
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
            if (StandaloneSelectedItem.Model != null && StandaloneSelectedItem.Model.hasLimitedCharges)
            {
                if(StandaloneSelectedItem.Model.Charges == 1)
                {
                    RemoveAction(StandaloneSelectedItem.Model.Type);
                }
                else
                {
                    StandaloneSelectedItem.Model.Charges--;
                    ActionsManager.DecreaseChargesOfAction(StandaloneSelectedItem.Model.Type);
                }
            }
            if (PlayerPrefsManager.Instance != null)
            {
                PlayerPrefsManager.Instance.Achievements.AddPerformedAction(StandaloneSelectedItem.Model.Type);
            }
            return StandaloneSelectedItem.Model!=null?StandaloneSelectedItem.Model:new PlayerAction(ActionType.JUMP,0, true);
        }

        public void Init()
        {
            gameObject.SetActive(true);
            availableActions = ActionsManager.GetAvailableActions();
            AsignActionsToItems();
        }

        public void SetInactiveSize()
        {
            transform.localScale = INACTIVE_SIZE * Vector3.one;
        }

        public void SetActiveSize()
        {
            transform.localScale = ACTIVE_SIZE * Vector3.one;
        }
    }
}
