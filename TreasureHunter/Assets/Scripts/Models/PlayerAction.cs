using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Models
{
    public class PlayerAction
    {
        public ActionType Type { get; private set; }
        public int Charges { get; private set; }
        public bool hasLimitedCharges { get; private set; }

        public PlayerAction(ActionType _type, int _charges = 0, bool _hasLimitedCharges = false)
        {
            Type = _type;
            Charges = _charges;
            hasLimitedCharges = _hasLimitedCharges;
        }
    }
}
