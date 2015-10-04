using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Models
{
    public class AchievementsData
    {
        #region CLASS SETTINGS

        private const char PROPERTIES_SEPARATOR = ',';
        private const char ITEMS_SEPARATOR = '@';
        private const char DICTIONARIES_SEPARATOR = '*';

        #endregion

        private Dictionary<string, int> lastSavedThresholdsOfAchievements = new Dictionary<string, int>();
        private Dictionary<MonsterType, int> killedMonsters = new Dictionary<MonsterType, int>();
        private Dictionary<MazeType, int> wonMazes = new Dictionary<MazeType, int>();
        private Dictionary<MazeType, int> lostMazes = new Dictionary<MazeType, int>();
        private Dictionary<ActionType, int> performedActions = new Dictionary<ActionType, int>();

        #region DICTIONARY TO STRING METHODS

        private string ThresholdsOfAchievementsToString()
        {
            StringBuilder builder = new StringBuilder();
            Dictionary<string,int>.Enumerator dictionaryEnumarator = lastSavedThresholdsOfAchievements.GetEnumerator();
            while(dictionaryEnumarator.MoveNext())
            {
                builder.Append(dictionaryEnumarator.Current.Key + PROPERTIES_SEPARATOR + dictionaryEnumarator.Current.Value + ITEMS_SEPARATOR);
            }
			if(builder.Length>0)
			{
				builder.Remove(builder.Length-1, 1);
			}
            return builder.ToString();
        }

        private string KilledMonstersToString()
        {
            StringBuilder builder = new StringBuilder();
            Dictionary<MonsterType, int>.Enumerator dictionaryEnumarator = killedMonsters.GetEnumerator();
            while (dictionaryEnumarator.MoveNext())
            {
                builder.Append(((int)dictionaryEnumarator.Current.Key).ToString() + PROPERTIES_SEPARATOR + dictionaryEnumarator.Current.Value.ToString() + ITEMS_SEPARATOR);
            }
			if(builder.Length>0)
			{
				builder.Remove(builder.Length-1, 1);
			}
            return builder.ToString();
        }

        private string WonMazesToString()
        {
            StringBuilder builder = new StringBuilder();
            Dictionary<MazeType, int>.Enumerator dictionaryEnumarator = wonMazes.GetEnumerator();
            while (dictionaryEnumarator.MoveNext())
            {
				builder.Append(((int)dictionaryEnumarator.Current.Key).ToString() + PROPERTIES_SEPARATOR + dictionaryEnumarator.Current.Value.ToString() + ITEMS_SEPARATOR);
            }
			if(builder.Length>0)
			{
				builder.Remove(builder.Length-1, 1);
			}
            return builder.ToString();
        }

        private string LostMazesToString()
        {
            StringBuilder builder = new StringBuilder();
            Dictionary<MazeType, int>.Enumerator dictionaryEnumarator = lostMazes.GetEnumerator();
            while (dictionaryEnumarator.MoveNext())
            {
				builder.Append(((int)dictionaryEnumarator.Current.Key).ToString() + PROPERTIES_SEPARATOR + dictionaryEnumarator.Current.Value.ToString() + ITEMS_SEPARATOR);
            }
			if(builder.Length>0)
			{
				builder.Remove(builder.Length-1, 1);
			}
            return builder.ToString();
        }

        private string PerformedActionsToString()
        {
            StringBuilder builder = new StringBuilder();
            Dictionary<ActionType, int>.Enumerator dictionaryEnumarator = performedActions.GetEnumerator();
            while (dictionaryEnumarator.MoveNext())
            {
				builder.Append(((int)dictionaryEnumarator.Current.Key).ToString() + PROPERTIES_SEPARATOR + dictionaryEnumarator.Current.Value.ToString() + ITEMS_SEPARATOR);
            }
			if(builder.Length>0)
			{
				builder.Remove(builder.Length-1, 1);
			}
            return builder.ToString();
        }

        #endregion

        #region ASIGN DATA TO DICTIONARIES

        public void AsignDataToCorrectDictionaries(string plaintext)
        {
            string[] dictionaryData = plaintext.Split(DICTIONARIES_SEPARATOR);
            AsignDataToThresholdsOfAchievements(dictionaryData[0]);
            AsignDataToKilledMonsters(dictionaryData[1]);
            AsignDataToWonMazes(dictionaryData[2]);
            AsignDataToLostMazes(dictionaryData[3]);
            AsignDataToPerformedActions(dictionaryData[4]);
        }

        private void AsignDataToThresholdsOfAchievements(string dictionaryData)
        {
            lastSavedThresholdsOfAchievements.Clear();
            if(dictionaryData!="")
            {
                string[] items = dictionaryData.Split(ITEMS_SEPARATOR);
                for(int i = 0;i<items.Length;i++)
                {
                    string[] properties = items[i].Split(PROPERTIES_SEPARATOR);
                    lastSavedThresholdsOfAchievements.Add(properties[0], Convert.ToInt32(properties[1]));
                }
            }
        }

        private void AsignDataToKilledMonsters(string dictionaryData)
        {
            killedMonsters.Clear();
            if (dictionaryData != "")
            {
                string[] items = dictionaryData.Split(ITEMS_SEPARATOR);
                for (int i = 0; i < items.Length; i++)
                {
                    string[] properties = items[i].Split(PROPERTIES_SEPARATOR);
                    killedMonsters.Add((MonsterType)Convert.ToInt32(properties[0]), Convert.ToInt32(properties[1]));
                }
            }
        }

        private void AsignDataToWonMazes(string dictionaryData)
        {
            wonMazes.Clear();
            if (dictionaryData != "")
            {
                string[] items = dictionaryData.Split(ITEMS_SEPARATOR);
                for (int i = 0; i < items.Length; i++)
                {
                    string[] properties = items[i].Split(PROPERTIES_SEPARATOR);
                    wonMazes.Add((MazeType)Convert.ToInt32(properties[0]), Convert.ToInt32(properties[1]));
                }
            }
        }

        private void AsignDataToLostMazes(string dictionaryData)
        {
            lostMazes.Clear();
            if (dictionaryData != "")
            {
                string[] items = dictionaryData.Split(ITEMS_SEPARATOR);
                for (int i = 0; i < items.Length; i++)
                {
                    string[] properties = items[i].Split(PROPERTIES_SEPARATOR);
                    lostMazes.Add((MazeType)Convert.ToInt32(properties[0]), Convert.ToInt32(properties[1]));
                }
            }
        }

        private void AsignDataToPerformedActions(string dictionaryData)
        {
            performedActions.Clear();
            if (dictionaryData != "")
            {
                string[] items = dictionaryData.Split(ITEMS_SEPARATOR);
                for (int i = 0; i < items.Length; i++)
                {
                    string[] properties = items[i].Split(PROPERTIES_SEPARATOR);
                    performedActions.Add((ActionType)Convert.ToInt32(properties[0]), Convert.ToInt32(properties[1]));
                }
            }
        }

        #endregion

        #region SETTERS

        public void ChangeThreshold(string title, int threshold)
        {
            if(lastSavedThresholdsOfAchievements.ContainsKey(title))
            {
                lastSavedThresholdsOfAchievements[title] = threshold;
            }
            else
            {
                lastSavedThresholdsOfAchievements.Add(title, threshold);
            }
        }

        public void AddKilledMonster(MonsterType monster)
        {
            if (monster != MonsterType.NONE)
            {
                if (killedMonsters.ContainsKey(monster))
                {
                    killedMonsters[monster]++;
                }
                else
                {
                    killedMonsters.Add(monster, 1);
                }
            }
        }

        public void AddWonMaze(MazeType maze)
        {
            if (maze != MazeType.NONE)
            {
                if (wonMazes.ContainsKey(maze))
                {
                    wonMazes[maze]++;
                }
                else
                {
                    wonMazes.Add(maze, 1);
                }
            }
        }

        public void AddLostMaze(MazeType maze)
        {
            if (maze != MazeType.NONE)
            {
                if (lostMazes.ContainsKey(maze))
                {
                    lostMazes[maze]++;
                }
                else
                {
                    lostMazes.Add(maze, 1);
                }
            }
        }

        public void AddPerformedAction(ActionType action)
        {
            if (action != ActionType.NONE)
            {
                if (performedActions.ContainsKey(action))
                {
                    performedActions[action]++;
                }
                else
                {
                    performedActions.Add(action, 1);
                }
            }
        }

        #endregion

        #region GETTERS

        public string GetStringToSaveDictionaries()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ThresholdsOfAchievementsToString());
            builder.Append(DICTIONARIES_SEPARATOR);
            builder.Append(KilledMonstersToString());
            builder.Append(DICTIONARIES_SEPARATOR);
            builder.Append(WonMazesToString());
            builder.Append(DICTIONARIES_SEPARATOR);
            builder.Append(LostMazesToString());
            builder.Append(DICTIONARIES_SEPARATOR);
            builder.Append(PerformedActionsToString());
            return builder.ToString();
        }

        public int GetCurrentThreshold(string title)
        {
            if (lastSavedThresholdsOfAchievements.ContainsKey(title))
            {
                return lastSavedThresholdsOfAchievements[title];
            }
            else
            {
                lastSavedThresholdsOfAchievements.Add(title, 0);
                return 0;
            }
        }

        public int GetKilledMonsters(MonsterType monster)
        {
            if (monster == MonsterType.NONE)
            {
                int allKilledMonsters = 0;
                Dictionary<MonsterType, int>.Enumerator dictionaryEnumarator = killedMonsters.GetEnumerator();
                while (dictionaryEnumarator.MoveNext())
                {
                    allKilledMonsters += dictionaryEnumarator.Current.Value;
                }
                return allKilledMonsters;
            }
            else if (killedMonsters.ContainsKey(monster))
            {
                return killedMonsters[monster];
            }
            else
            {
                killedMonsters.Add(monster, 0);
                return 0;
            }
        }

        public int GetWonMazes(MazeType maze)
        {
            if (maze == MazeType.NONE)
            {
                int allWonMazes = 0;
                Dictionary<MazeType, int>.Enumerator dictionaryEnumarator = wonMazes.GetEnumerator();
                while (dictionaryEnumarator.MoveNext())
                {
                    allWonMazes += dictionaryEnumarator.Current.Value;
                }
                return allWonMazes;
            }
            else if (wonMazes.ContainsKey(maze))
            {
                return wonMazes[maze];
            }
            else
            {
                wonMazes.Add(maze, 0);
                return 0;
            }
        }

        public int GetLostMazes(MazeType maze)
        {
            if (maze == MazeType.NONE)
            {
                int allLostMazes = 0;
                Dictionary<MazeType, int>.Enumerator dictionaryEnumarator = lostMazes.GetEnumerator();
                while (dictionaryEnumarator.MoveNext())
                {
                    allLostMazes += dictionaryEnumarator.Current.Value;
                }
                return allLostMazes;
            }
            else if (lostMazes.ContainsKey(maze))
            {
                return lostMazes[maze];
            }
            else
            {
                lostMazes.Add(maze, 0);
                return 0;
            }
        }

        public int GetPerformedAction(ActionType action)
        {
            if (action == ActionType.NONE)
            {
                int allPerformedActions = 0;
                Dictionary<ActionType, int>.Enumerator dictionaryEnumarator = performedActions.GetEnumerator();
                while (dictionaryEnumarator.MoveNext())
                {
                    allPerformedActions += dictionaryEnumarator.Current.Value;
                }
                return allPerformedActions;
            }
            else if (performedActions.ContainsKey(action))
            {
                return performedActions[action];
            }
            else
            {
                performedActions.Add(action, 0);
                return 0;
            }
        }

        #endregion
    }
}
