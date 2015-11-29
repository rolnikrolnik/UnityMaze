using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Interfaces;
using Treasure_Hunter.Managers;
using UnityEngine;

namespace Treasure_Hunter.Controllers
{
    public class TreasureController : MonoBehaviour, IInitiation
    {
        #region SCENE REFERENCES

        public GameObject[] TreasuresPacks;

        #endregion

        public void Init()
        {
            int allWonMazes = PlayerPrefsManager.Instance.Achievements.GetWonMazes(MazeType.NONE);
            for(int i = 0;i<TreasuresPacks.Length;i++)
            {
                TreasuresPacks[i].SetActive(i<allWonMazes);
            }
        }
    }
}
