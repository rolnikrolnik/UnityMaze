using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;
using UnityEngine;

namespace Treasure_Hunter.TestScripts
{
    public class AchievementsManagerTest : MonoBehaviour
    {
        #region SCENE REFERENCES

        public AchievementsManager AchievementsManager;

        #endregion

        #region MONO BEHAVIOUR

        private int testcaseNumber = 0;
        private int AmountOfTestcases = 12;

        private void Start()
        {
            if (PlayerPrefsManager.Instance != null)
            {
                PlayerPrefsManager.Instance.Init();
                AchievementsManager.Init();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                ManageTestCases();
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                PlayerPrefsManager.Instance.SaveAchievements();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }

        #endregion

        #region ACHIEVEMENTS UI TESTS

        private void ManageTestCases()
        {
            switch (testcaseNumber)
            {
                case 0:
                    for (int i = 0; i <= AchievementsManager.achievementsList[0].Model.FirstThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddKilledMonster(MonsterType.SKELETON);
                    }
                    AchievementsManager.Init();
                    break;
                case 1:
                    for (int i = 0; i <= AchievementsManager.achievementsList[0].Model.SecondThreshold - AchievementsManager.achievementsList[0].Model.FirstThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddKilledMonster(MonsterType.DINOZAUR);
                    }
                    AchievementsManager.Init();
                    break;
                case 2:
                    for (int i = 0; i <= AchievementsManager.achievementsList[0].Model.ThirdThreshold - AchievementsManager.achievementsList[0].Model.SecondThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddKilledMonster(MonsterType.TROLL);
                    }
                    AchievementsManager.Init();
                    break;
                case 3:
                    for (int i = 0; i <= AchievementsManager.achievementsList[1].Model.FirstThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddWonMaze(MazeType.PREHISTORIC_MAZE);
                    }
                    AchievementsManager.Init();
                    break;
                case 4:
                    for (int i = 0; i <= AchievementsManager.achievementsList[1].Model.SecondThreshold - AchievementsManager.achievementsList[1].Model.FirstThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddWonMaze(MazeType.SWAMP_MAZE);
                    }
                    AchievementsManager.Init();
                    break;
                case 5:
                    for (int i = 0; i <= AchievementsManager.achievementsList[1].Model.ThirdThreshold - AchievementsManager.achievementsList[1].Model.SecondThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddWonMaze(MazeType.PREHISTORIC_MAZE);
                    }
                    AchievementsManager.Init();
                    break;
                case 6:
                    for (int i = 0; i <= AchievementsManager.achievementsList[2].Model.FirstThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddLostMaze(MazeType.PREHISTORIC_MAZE);
                    }
                    AchievementsManager.Init();
                    break;
                case 7:
                    for (int i = 0; i <= AchievementsManager.achievementsList[2].Model.SecondThreshold - AchievementsManager.achievementsList[2].Model.FirstThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddLostMaze(MazeType.SWAMP_MAZE);
                    }
                    AchievementsManager.Init();
                    break;
                case 8:
                    for (int i = 0; i <= AchievementsManager.achievementsList[2].Model.ThirdThreshold - AchievementsManager.achievementsList[2].Model.SecondThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddLostMaze(MazeType.PREHISTORIC_MAZE);
                    }
                    AchievementsManager.Init();
                    break;
                case 9:
                    for (int i = 0; i <= AchievementsManager.achievementsList[3].Model.FirstThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddPerformedAction(ActionType.JUMP);
                    }
                    AchievementsManager.Init();
                    break;
                case 10:
                    for (int i = 0; i <= AchievementsManager.achievementsList[3].Model.SecondThreshold - AchievementsManager.achievementsList[3].Model.FirstThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddPerformedAction(ActionType.ATTACK);
                    }
                    AchievementsManager.Init();
                    break;
                case 11:
                    for (int i = 0; i <= AchievementsManager.achievementsList[3].Model.ThirdThreshold - AchievementsManager.achievementsList[3].Model.FirstThreshold; i++)
                    {
                        PlayerPrefsManager.Instance.Achievements.AddPerformedAction(ActionType.JUMP);
                    }
                    AchievementsManager.Init();
                    break;
            }
            testcaseNumber++;
            if (testcaseNumber >= AmountOfTestcases)
            {
                testcaseNumber = 0;
            }
        }

        #endregion
    }
}
