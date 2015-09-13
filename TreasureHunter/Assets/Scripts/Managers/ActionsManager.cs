using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Managers
{
    public class ActionsManager : MonoBehaviour
    {
        public static ActionsManager Instance { get; private set; }

        #region PROJECT REFERENCES

        public Sprite Placeholder;
        public Sprite RopeImage;

        #endregion

        #region MONO BEHAVIOUR

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        public Sprite GetActionIcon(ActionType type)
        {
            switch(type)
            {
                case ActionType.Attack:
                    return Placeholder;
                case ActionType.Rope:
                    return RopeImage;
                case ActionType.Jump:
                    return Placeholder;
                default:
                    return Placeholder;
            }
        }
    }
}
