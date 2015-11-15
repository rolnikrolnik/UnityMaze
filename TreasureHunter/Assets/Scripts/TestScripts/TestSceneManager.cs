using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.TestScripts
{
    public class TestSceneManager : SceneManager
    {
        #region MONO BEHAVIOUR

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(GameObject);
        }

        private void Start()
        {
            if (BaseManager != null)
            {
                BaseManager.Player.Init();
            }
        }

        #endregion
    }
}
