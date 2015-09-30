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
        #region SCENE REFERENCES

        //Standalone
        public ActionItem[] StandaloneItems;
        public ActionItem StandaloneSelectedItem;
        //OVR
        public ActionItem[] OVRItems;
        public ActionItem OVRSelectedItem;
        //Manager
        public ActionsManager ActionsManager;

        #endregion

        private Coroutine currentCoroutine;
        private List<PlayerAction> availableActions;
        private int SelectedItemNumber;

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
        }

        #endregion

        #region ACTIONS SERVICE

        public void AddAction(PlayerAction _action)
        {
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
            PlayerPrefsManager.Instance.Achievements.AddPerformedAction(StandaloneSelectedItem.Model.Type);
            return StandaloneSelectedItem.Model;
        }

        public void Init()
        {
            availableActions = ActionsManager.GetAvailableActions();
            AsignActionsToItems();
        }
    }
}
