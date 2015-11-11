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

        public Sprite AttackImage;
        public Sprite RopeImage;
        public Sprite JumpImage;

        #endregion

        #region MODIFICATIONS OF ACTIONS

        public void RemoveAction(ActionType _type)
        {
            PlayerPrefsManager.Instance.Actions.DecreaseChargesOfAction(_type);
        }

        public void AddAction(PlayerAction _action)
        {
            PlayerPrefsManager.Instance.Actions.AddAction(_action);
        }

        public void AddChargesToAction(PlayerAction _action)
        {
            PlayerPrefsManager.Instance.Actions.AddChargesToAction(_action);
        }

        public void DecreaseChargesOfAction(ActionType actionType)
        {
            PlayerPrefsManager.Instance.Actions.DecreaseChargesOfAction(actionType);
        }

        #endregion

        public List<PlayerAction> GetAvailableActions()
        {
            return PlayerPrefsManager.Instance.Actions.GetAvailableActions();
        }

        public Sprite GetActionIcon(ActionType type)
        {
            switch(type)
            {
                case ActionType.ATTACK:
                    return AttackImage;
                case ActionType.ROPE:
                    return RopeImage;
                case ActionType.JUMP:
                    return JumpImage;
                default:
                    return null;
            }
        }
    }
}
