using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Interfaces;

namespace Treasure_Hunter.Managers
{
    public class AchievementsManager : MonoBehaviour, IInitiation
    {
        #region CLASS SETTINGS

        private const int ACHIEVEMENT_PARAMATERS_NUMBER = 6;

        #endregion

        #region SCENE REFERENCES

        public List<AchievementItem> achievementsList;

        #endregion

        #region PROJECT REFERENCES

        public Material GoldGoblet;
        public Material SilverGoblet;
        public Material BronzeGoblet;

        public Sprite GoldGobletSprite;
        public Sprite SilverGobletSprite;
        public Sprite BronzeGobletSprite;

        #endregion

        public void Init()
        {
            TextAsset achievementsThresholds = Resources.Load<TextAsset>("ConfigFiles/Achievements_Thresholds");
            string[] lines = achievementsThresholds.text.Split('\n');
            //omit first line
            for(int i = 1; i <lines.Length;i++)
            {
                string[] achievementParameters = lines[i].Split(',');
                if(achievementParameters.Length == ACHIEVEMENT_PARAMATERS_NUMBER)
                {
                    achievementsList[i - 1].SetData(achievementParameters);
                }
            }
        }

        public Material GetMaterialFromEnum(AchievementIconEnum IconEnum)
        {
            switch(IconEnum)
            {
                case AchievementIconEnum.BRONZE_GOBLET:
                    return BronzeGoblet;
                case AchievementIconEnum.SILVER_GOBLET:
                    return SilverGoblet;
                case AchievementIconEnum.GOLD_GOBLET:
                    return GoldGoblet;
                default:
                    return null;
            }
        }

        public Sprite GetSpriteFromEnum(AchievementIconEnum IconEnum)
        {
            switch (IconEnum)
            {
                case AchievementIconEnum.BRONZE_GOBLET:
                    return BronzeGobletSprite;
                case AchievementIconEnum.SILVER_GOBLET:
                    return SilverGobletSprite;
                case AchievementIconEnum.GOLD_GOBLET:
                    return GoldGobletSprite;
                default:
                    return null;
            }
        }
    }
}
