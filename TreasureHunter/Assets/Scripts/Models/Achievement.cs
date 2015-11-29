using System;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Models
{
    public class Achievement
    {
        public string Title { get; private set; }
        public AchievementIconEnum IconEnum { get; private set; }
        public AchievementGoal Goal { get; private set; }
        public int GoalParameter { get; private set; }
        public int FirstThreshold { get; private set; }
        public int SecondThreshold { get; private set; }
        public int ThirdThreshold { get; private set; }
        public int CurrentThreshold { get; private set; }
        public int CurrentValue { get; private set; }


        public Achievement(Achievement achievement)
        {
            Title = achievement.Title;
            Goal = achievement.Goal;
            FirstThreshold = achievement.FirstThreshold;
            SecondThreshold = achievement.SecondThreshold;
            ThirdThreshold = achievement.ThirdThreshold;
            GoalParameter = achievement.GoalParameter;
            CurrentThreshold = achievement.CurrentThreshold;
            CurrentValue = achievement.CurrentValue;
            IconEnum = achievement.IconEnum;
        }

        public Achievement(string[] achievementParameters)
        {
            Title = achievementParameters[0];
            Goal = (AchievementGoal)Convert.ToInt32(achievementParameters[1]);
            FirstThreshold = Convert.ToInt32(achievementParameters[2]);
            SecondThreshold = Convert.ToInt32(achievementParameters[3]);
            ThirdThreshold = Convert.ToInt32(achievementParameters[4]);
            GoalParameter = Convert.ToInt32(achievementParameters[5]);
            CurrentThreshold = PlayerPrefsManager.Instance.Achievements.GetCurrentThreshold(Title);
            CurrentValue = GetCurrentValue();
            CheckCurrentThreshold();
        }

        private int GetCurrentValue()
        {
            switch (Goal)
            {
                case AchievementGoal.KILLING_MONSTERS:
                    return PlayerPrefsManager.Instance.Achievements.GetKilledMonsters((MonsterType)GoalParameter);
                case AchievementGoal.WINING_MAZES:
                    return PlayerPrefsManager.Instance.Achievements.GetWonMazes((MazeType)GoalParameter);
                case AchievementGoal.LOSING_MAZES:
                    return PlayerPrefsManager.Instance.Achievements.GetLostMazes((MazeType)GoalParameter);
                case AchievementGoal.USING_ACTIONS:
                    return PlayerPrefsManager.Instance.Achievements.GetPerformedAction((ActionType)GoalParameter);
                default:
                    return 0;
            }
        }

        private void CheckCurrentThreshold()
        {
            int newThreshold = CurrentValue >= ThirdThreshold ? 3 :
                               CurrentValue >= SecondThreshold ? 2 :
                               CurrentValue >= FirstThreshold ? 1 : 0;
            IconEnum = newThreshold == 3 ? AchievementIconEnum.GOLD_GOBLET :
                       newThreshold == 2 ? AchievementIconEnum.SILVER_GOBLET :
                       newThreshold == 1 ? AchievementIconEnum.BRONZE_GOBLET : AchievementIconEnum.NONE;
            if (newThreshold > CurrentThreshold)
            {
                CurrentThreshold = newThreshold;
                if (SceneManager.Instance != null && SceneManager.Instance.BaseManager != null)
                {
					SceneManager.Instance.BaseManager.EnqueueAchievementPopup(this);
					PlayerPrefsManager.Instance.Achievements.ChangeThreshold(Title, CurrentThreshold);
                }
            }
        }

    }
}
