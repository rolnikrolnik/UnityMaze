using Treasure_Hunter.Controllers;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;
using Treasure_Hunter.Models;
using UnityEngine;

namespace Treasure_Hunter.TestScripts
{
    public class EnemySceneTestManager : MonoBehaviour
    {
        #region SCENE REFERENCES

        public PlayerController Player;
        public ActionChoicePopup ActionPopup;

        #endregion

        private void Start()
        {
            Player.EnablePlayer();
            PlayerPrefsManager.Instance.Init();
            ActionPopup.AddAction(new PlayerAction(ActionType.ATTACK, 0, true));
            ActionPopup.AddAction(new PlayerAction(ActionType.JUMP, 0, true));
        }
    }
}
