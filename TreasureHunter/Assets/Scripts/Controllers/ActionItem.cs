using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Utils;
using Treasure_Hunter.Models;
using Treasure_Hunter.Managers;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Controllers
{
    public class ActionItem : MonoBehaviour
    {
        #region SCENE REFERENCES

        public ActionChoicePopup ChoicePopup;
        public Image ActionBackground;
        public Image ActionImage;

        #endregion

        public PlayerAction Model { get; private set; }
        public bool IsSelectedAction;

        #region MONO BEHAVIOUR

        private void OnMouseEnter()
        {
            if (Model != null)
            {
                ChoicePopup.SelectAsMain(Model);
            }
        }

        #endregion

        public void AddModel(PlayerAction _model)
        {
            if (_model != null)
            {
                Model = _model;
                gameObject.SetActive(true);
                ActionImage.sprite = ActionsManager.Instance.GetActionIcon(Model.Type);
            }
            else if(!IsSelectedAction)
            {
                gameObject.SetActive(false);
            }
            else
            {
                ActionImage.sprite = ActionsManager.Instance.GetActionIcon(ActionType.None);
            }
        }

        public void SetAlphaChannel(float alpha)
        {
            ActionImage.SetAlphaChannel(alpha);
            ActionBackground.SetAlphaChannel(alpha);
        }
    }
}
