using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Models
{
    public class ActionsData
    {
        #region CLASS SETTINGS

        private const char PROPERTIES_SEPARATOR = ',';
        private const char ITEMS_SEPARATOR = '@';

        #endregion

        private List<PlayerAction> availableActions = new List<PlayerAction>();

        #region MODIFICATIONS OF ACTIONS

        public void RemoveAction(ActionType _type)
        {
            availableActions.RemoveAll((a) => a.Type == _type);
        }

        public void AddAction(PlayerAction _action)
        {
            availableActions.Add(_action);
        }

        public void AddChargesToAction(PlayerAction _action)
        {
            int index = availableActions.FindIndex((a) => a.Type == _action.Type);
            if (index >= 0 && index < availableActions.Count)
            {
                availableActions[index].Charges = _action.Charges;
            }
        }

        public void DecreaseChargesOfAction(ActionType actionType)
        {
            int index = availableActions.FindIndex((a) => a.Type == actionType);
            if (index >= 0 && index < availableActions.Count)
            {
                availableActions[index].Charges--;
            }
        }

        #endregion

        public List<PlayerAction> GetAvailableActions()
        {
            return availableActions;
        }

        public string GetStringToSaveActions()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ActionsToString());
            return builder.ToString();
        }

        public void AsignDataToCorrectDictionaries(string plaintext)
        {
            AsignDataToActionsList(plaintext);
        }

        private void AsignDataToActionsList(string data)
        {
            availableActions.Clear();
            availableActions.Add(new PlayerAction(ActionType.ATTACK, 0, false));
            availableActions.Add(new PlayerAction(ActionType.JUMP, 0, false));
            if (data != "")
            {
                string[] items = data.Split(ITEMS_SEPARATOR);
                for (int i = 0; i < items.Length; i++)
                {
                    string[] properties = items[i].Split(PROPERTIES_SEPARATOR);
                    availableActions.Add(new PlayerAction((ActionType)Convert.ToInt32(properties[0]), Convert.ToInt32(properties[2]), Convert.ToBoolean(properties[1])));
                }
            }
        }

        private string ActionsToString()
        {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i<availableActions.Count;i++)
            {
                builder.Append((int)availableActions[i].Type + PROPERTIES_SEPARATOR + availableActions[i].hasLimitedCharges.ToString() + PROPERTIES_SEPARATOR + availableActions[i].Charges + ITEMS_SEPARATOR);
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }
    }
}
