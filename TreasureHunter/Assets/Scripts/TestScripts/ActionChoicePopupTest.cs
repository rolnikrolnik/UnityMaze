using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Models;
using Treasure_Hunter.Enumerations;


namespace Treasure_Hunter.TestScripts
{
    public class ActionChoicePopupTest : MonoBehaviour
    {
        #region SCENE REFERENCES

        public ActionChoicePopup ActionChoicePopup;

        #endregion

        private int testcaseNumber = 0;
        private int AmountOfTestcases = 6;

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                switch(testcaseNumber)
                {
                    case 0:
                        ActionChoicePopup.Show();
                        break;
                    case 1:
                        ActionChoicePopup.Hide();
                        break;
                    case 2:
                        ActionChoicePopup.Show();
                        ActionChoicePopup.Init();
                        break;
                    case 3:
                        ActionChoicePopup.SelectAction();
                        break;
                    case 4:
                        ActionChoicePopup.AddAction(new PlayerAction(ActionType.JUMP));
                        ActionChoicePopup.AddAction(new PlayerAction(ActionType.ROPE));
                        ActionChoicePopup.Show();
                        break;
                    case 5:
                        ActionChoicePopup.RemoveAction(ActionType.ROPE);
                        ActionChoicePopup.Hide();
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