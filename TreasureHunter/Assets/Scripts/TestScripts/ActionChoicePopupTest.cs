using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Models;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;


namespace Treasure_Hunter.TestScripts
{
    public class ActionChoicePopupTest : MonoBehaviour
    {
        #region SCENE REFERENCES

        public ActionChoicePopup ActionChoicePopup;

        #endregion

        private int testcaseNumber = 0;
        private int AmountOfTestcases = 1;

        void Start()
        {
            PlayerPrefsManager.Instance.Init();
            ActionChoicePopup.Init();
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                switch(testcaseNumber)
                {
                    case 0:
                        ActionChoicePopup.Show();
                        break;
                }
                testcaseNumber++;
                if(testcaseNumber>=AmountOfTestcases)
                {
                    testcaseNumber = 0;
                }
            }
        }
    }
}