using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Managers;
using Treasure_Hunter.Abstract;
using System;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Models;

namespace Treasure_Hunter.Controllers
{
    public class AchievementsPopup : AbstractPopup
    {
        #region CLASS SETTINGS

        private const string FIRST_DESCRIPTION_SENTENCE = "Congratulations, you gained {0} level of the {1} achievement. ";
        private const string KILLING_MONSTERS_FINISHED_DESCRIPTION = "You reached maximum level killing more than {0} {1}.";
        private const string KILLING_MONSTERS_DESCRIPTION =  "You killed {0} {1}, to next level you need to kill {2}.";
        private const string LOSING_MAZES_FINISHED_DESCRIPTION = "You reached maximum level losing more than {0} {1}.";
        private const string LOSING_MAZES_DESCRIPTION = "You lost {0} {1}, to next level you need to lose {2}.";
        private const string WINNING_MAZES_FINISHED_DESCRIPTION = "You reached maximum level winning more than {0} {1}.";
        private const string WINNING_MAZES_DESCRIPTION = "You won {0} {1}, to next level you need to win {2}.";
        private const string USING_ACTIONS_FINISHED_DESCRIPTION = "You reached maximum level performing more than {0} {1}.";
        private const string USING_ACTIONS_DESCRIPTION = "You performed {0} {1}, to next level you need to perform {2}.";

        #endregion

        #region SCENE REFERENCES

        //Standalone
        public Image StandaloneButtonBackground;
        public Image StandaloneGoblet;
        public Text StandalonePopupTitle;
        public Text StandaloneAchievementTitle;
        public Text StandaloneAchievementDescription;
        public Text StandaloneButtonText;
        //OVR
        public Image OVRButtonBackground;
        public Image OVRGoblet;
        public Text OVRPopupTitle;
        public Text OVRAchievementTitle;
        public Text OVRAchievementDescription;
        public Text OVRButtonText;
        //Managers
        public AchievementsManager AchievementsManager;

        #endregion

        public Achievement CurrentAchievement {get; private set;}

        #region INPUT HANDLING

        public void OnContinueButtonClick()
        {
            SceneManager.Instance.BaseManager.Player.EnablePlayer();
            Hide();
            if (SceneManager.Instance.BaseManager != null)
            {
                SceneManager.Instance.BaseManager.Player.AnyPopupIsVisible = false;
            }
        }

        #endregion

        public void Show(Achievement achievement, bool showButton)
        {
            base.Show();
            CurrentAchievement = achievement;
            StandaloneGoblet.sprite = AchievementsManager.GetSpriteFromEnum(achievement.IconEnum);
            OVRGoblet.sprite = StandaloneGoblet.sprite;
            StandaloneAchievementTitle.text = CurrentAchievement.Title + (CurrentAchievement.CurrentThreshold == 1 ? " I" : CurrentAchievement.CurrentThreshold == 2 ? " II" : " III");
            OVRPopupTitle.text = StandaloneAchievementTitle.text;
            StandaloneAchievementDescription.text = SetAchievementDescription();
            OVRAchievementDescription.text = StandaloneAchievementDescription.text;
            StandaloneButtonBackground.gameObject.SetActive(showButton);
            OVRButtonBackground.gameObject.SetActive(showButton);
            if (SceneManager.Instance.BaseManager != null)
            {
                SceneManager.Instance.BaseManager.Player.AnyPopupIsVisible = showButton;
            }
        }

        private string SetAchievementDescription()
        {
            string firstSentence = String.Format(FIRST_DESCRIPTION_SENTENCE, CurrentAchievement.CurrentThreshold==1?"first":
                                                CurrentAchievement.CurrentThreshold==2?"second":"third", CurrentAchievement.Title);
            if (CurrentAchievement.CurrentThreshold < 3)
            {
                switch (CurrentAchievement.Goal)
                {
                    case AchievementGoal.KILLING_MONSTERS:
                        string monsterType = GetStringFromMonsterType((MonsterType)CurrentAchievement.GoalParameter);
                        return firstSentence + String.Format(KILLING_MONSTERS_DESCRIPTION, CurrentAchievement.CurrentValue, monsterType,
                                                             CurrentAchievement.CurrentThreshold == 1 ? CurrentAchievement.SecondThreshold :
                                                             CurrentAchievement.ThirdThreshold);
                    case AchievementGoal.LOSING_MAZES:
                        string lostMazeType = GetStringFromMazeType((MazeType)CurrentAchievement.GoalParameter);
                        return firstSentence + String.Format(LOSING_MAZES_DESCRIPTION, CurrentAchievement.CurrentValue, lostMazeType,
                                                             CurrentAchievement.CurrentThreshold == 1 ? CurrentAchievement.SecondThreshold :
                                                             CurrentAchievement.ThirdThreshold);
                    case AchievementGoal.WINING_MAZES:
                        string wonMazeType = GetStringFromMazeType((MazeType)CurrentAchievement.GoalParameter);
                        return firstSentence + String.Format(WINNING_MAZES_DESCRIPTION, CurrentAchievement.CurrentValue, wonMazeType,
                                                             CurrentAchievement.CurrentThreshold == 1 ? CurrentAchievement.SecondThreshold :
                                                             CurrentAchievement.ThirdThreshold);
                    case AchievementGoal.USING_ACTIONS:
                        string permormedActions = GetStringFromActionType((ActionType)CurrentAchievement.GoalParameter);
                        return firstSentence + String.Format(USING_ACTIONS_DESCRIPTION, CurrentAchievement.CurrentValue, permormedActions,
                                                             CurrentAchievement.CurrentThreshold == 1 ? CurrentAchievement.SecondThreshold :
                                                             CurrentAchievement.ThirdThreshold);
                    default:
                        return "";
                }
            }
            else
            {
                switch (CurrentAchievement.Goal)
                {
                    case AchievementGoal.KILLING_MONSTERS:
                        string monsterType = GetStringFromMonsterType((MonsterType)CurrentAchievement.GoalParameter);
                        return firstSentence + String.Format(KILLING_MONSTERS_FINISHED_DESCRIPTION, CurrentAchievement.ThirdThreshold, monsterType);
                    case AchievementGoal.LOSING_MAZES:
                        string lostMazeType = GetStringFromMazeType((MazeType)CurrentAchievement.GoalParameter);
                        return firstSentence + String.Format(LOSING_MAZES_FINISHED_DESCRIPTION, CurrentAchievement.ThirdThreshold, lostMazeType);
                    case AchievementGoal.WINING_MAZES:
                        string wonMazeType = GetStringFromMazeType((MazeType)CurrentAchievement.GoalParameter);
                        return firstSentence + String.Format(WINNING_MAZES_FINISHED_DESCRIPTION, CurrentAchievement.ThirdThreshold, wonMazeType);
                    case AchievementGoal.USING_ACTIONS:
                        string permormedActions = GetStringFromActionType((ActionType)CurrentAchievement.GoalParameter);
                        return firstSentence + String.Format(USING_ACTIONS_FINISHED_DESCRIPTION, CurrentAchievement.ThirdThreshold, permormedActions);
                    default:
                        return "";
                }
            }
        }

        private string GetStringFromActionType(ActionType type)
        {
            switch (type)
            {
                case ActionType.ATTACK:
                    return "attacks";
                case ActionType.JUMP:
                    return "jumps";
                case ActionType.ROPE:
                    return "rope usages";
                default:
                    return "mazes";
            }
        }

        private string GetStringFromMazeType(MazeType type)
        {
            switch (type)
            {
                case MazeType.PREHISTORIC_MAZE:
                    return "prehistoric mazes";
                case MazeType.NECROPOLIS_MAZE:
                    return "necropolis mazes";
                case MazeType.SWAMP_MAZE:
                    return "swamp mazes";
                case MazeType.WORMSWORLD_MAZE:
                    return "worms world mazes";
                default:
                    return "mazes";
            }
        }

        private string GetStringFromMonsterType(MonsterType type)
        {
            switch(type)
            {
                case MonsterType.SKELETON:
                    return "skeletons";
                case MonsterType.TROLL:
                    return "trolls";
                case MonsterType.WORM:
                    return "worms";
                case MonsterType.DINOZAUR:
                    return "dinozaurs";
                default:
                    return "monsters";
            }
        }

        protected override void SetAlphaChannel(float alpha)
        {
            base.SetAlphaChannel(alpha);
            OVRButtonBackground.color = new Color(OVRButtonBackground.color.r, OVRButtonBackground.color.g, OVRButtonBackground.color.b, alpha);
            OVRGoblet.color = new Color(OVRGoblet.color.r, OVRGoblet.color.g, OVRGoblet.color.b, alpha);
            OVRPopupTitle.color = new Color(OVRPopupTitle.color.r, OVRPopupTitle.color.g, OVRPopupTitle.color.b, alpha);
            OVRAchievementTitle.color = new Color(OVRAchievementTitle.color.r, OVRAchievementTitle.color.g, OVRAchievementTitle.color.b, alpha);
            OVRAchievementDescription.color = new Color(OVRAchievementDescription.color.r, OVRAchievementDescription.color.g, OVRAchievementDescription.color.b, alpha);
            OVRButtonText.color = new Color(OVRButtonText.color.r, OVRButtonText.color.g, OVRButtonText.color.b, alpha);
            StandaloneButtonBackground.color = new Color(StandaloneButtonBackground.color.r, StandaloneButtonBackground.color.g, StandaloneButtonBackground.color.b, alpha);
            StandaloneGoblet.color = new Color(StandaloneGoblet.color.r, StandaloneGoblet.color.g, StandaloneGoblet.color.b, alpha);
            StandalonePopupTitle.color = new Color(StandalonePopupTitle.color.r, StandalonePopupTitle.color.g, StandalonePopupTitle.color.b, alpha);
            StandaloneAchievementTitle.color = new Color(StandaloneAchievementTitle.color.r, StandaloneAchievementTitle.color.g, StandaloneAchievementTitle.color.b, alpha);
            StandaloneAchievementDescription.color = new Color(StandaloneAchievementDescription.color.r, StandaloneAchievementDescription.color.g, StandaloneAchievementDescription.color.b, alpha); 
            StandaloneButtonText.color = new Color(StandaloneButtonText.color.r, StandaloneButtonText.color.g, StandaloneButtonText.color.b, alpha);
        }
    }
}
