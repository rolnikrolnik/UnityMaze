using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Abstract;
using System.Collections.Generic;
using Treasure_Hunter.Models;

namespace Treasure_Hunter.Managers
{
    public class BaseManager : LevelManager
    {
        #region SCENE REFERENCES

        public MazeChoicePopup MazeChoicePopup;
        public AchievementsPopup AchievementsPopup;
        public EndGamePopup EndGamePopup;
        public AchievementsManager AchievementsManager;
        public PlayerController Player;

        #endregion

        private List<Achievement> achievementPopupsQueue = new List<Achievement>();

        #region MONO BEHAVIOUR

        private void Update()
        {
            if (achievementPopupsQueue != null && achievementPopupsQueue.Count > 0 &&
                !AchievementsPopup.gameObject.activeInHierarchy && Player.IsEnabled)
            {
                AchievementsPopup.Show(achievementPopupsQueue[0], true);
                achievementPopupsQueue.RemoveAt(0);
                Player.DisablePlayer();
            }
        }

        #endregion

        public override void MoveUIToCanvas()
        {
            MovePopupToCanvas(MazeChoicePopup.transform);
            MovePopupToCanvas(AchievementsPopup.transform);
            MovePopupToCanvas(EndGamePopup.transform);
        }

        public void EnqueueAchievementPopup(Achievement achievement)
        {
            if(achievementPopupsQueue==null)
            {
                achievementPopupsQueue = new List<Achievement>();
            }
            achievementPopupsQueue.Add(new Achievement(achievement));
        }
    }
}
