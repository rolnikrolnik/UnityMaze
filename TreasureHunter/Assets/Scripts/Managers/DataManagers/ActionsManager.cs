using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Enumerations;
using System.Collections.Generic;
using Treasure_Hunter.Models;

namespace Treasure_Hunter.Managers
{
    public class ActionsManager : MonoBehaviour
    {
        #region PROJECT REFERENCES

        public Sprite Placeholder;
        public Sprite RopeImage;

        #endregion

        #region MODIFICATIONS OF ACTIONS

        public void RemoveAction(ActionType _type)
        {
            throw new System.NotImplementedException();
        }

        public void AddAction(PlayerAction _action)
        {
            throw new System.NotImplementedException();
        }

        public void AddChargesToAction(PlayerAction _action)
        {
            throw new System.NotImplementedException();
        }

        public void DecreaseChargesOfAction(ActionType actionType)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public List<PlayerAction> GetAvailableActions()
        {
            throw new System.NotImplementedException();
            //PlayerPrefsManager.Instance.GetAvailableActions
        }

        public Sprite GetActionIcon(ActionType type)
        {
            switch(type)
            {
                case ActionType.ATTACK:
                    return Placeholder;
                case ActionType.ROPE:
                    return RopeImage;
                case ActionType.JUMP:
                    return Placeholder;
                default:
                    return Placeholder;
            }
        }
    }
}
