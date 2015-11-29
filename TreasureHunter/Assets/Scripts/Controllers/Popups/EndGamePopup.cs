using UnityEngine;
using Treasure_Hunter.Abstract;
using UnityEngine.UI;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Controllers
{
    public class EndGamePopup : AbstractPopup
    {
        #region SCENE REFERENCES

        //Standalone
        public Image StandaloneYesButtonBackground;
        public Image StandaloneNoButtonBackground;
        public Text StandaloneMessage;
        public Text StandaloneYesButtonText;
        public Text StandaloneNoButtonText;
        //OVR
        public Image OVRYesButtonBackground;
        public Image OVRNoButtonBackground;
        public Text OVRMessage;
        public Text OVRYesButtonText;
        public Text OVRNoButtonText;

        #endregion

        #region INPUT HANDLING

        public void OnYesButtonClick()
        {
            PlayerPrefsManager.Instance.SaveAchievements();
            Application.Quit();
        }
        public void OnNoButtonClick()
        {
            Hide();
            if (SceneManager.Instance.MazeManager != null)
            {
                SceneManager.Instance.MazeManager.Player.AnyPopupIsVisible = false;
            }
            else if (SceneManager.Instance.BaseManager != null)
            {
                SceneManager.Instance.BaseManager.Player.AnyPopupIsVisible = false;
            }
        }

        #endregion

        protected override void SetAlphaChannel(float alpha)
        {
            base.SetAlphaChannel(alpha);
            StandaloneYesButtonBackground.color = new Color(StandaloneYesButtonBackground.color.r, StandaloneYesButtonBackground.color.g, StandaloneYesButtonBackground.color.b, alpha);
            StandaloneNoButtonBackground.color = new Color(StandaloneNoButtonBackground.color.r, StandaloneNoButtonBackground.color.g, StandaloneNoButtonBackground.color.b, alpha);
            StandaloneMessage.color = new Color(StandaloneMessage.color.r, StandaloneMessage.color.g, StandaloneMessage.color.b, alpha);
            StandaloneYesButtonText.color = new Color(StandaloneYesButtonText.color.r, StandaloneYesButtonText.color.g, StandaloneYesButtonText.color.b, alpha);
            StandaloneNoButtonText.color = new Color(StandaloneNoButtonText.color.r, StandaloneNoButtonText.color.g, StandaloneNoButtonText.color.b, alpha);
            OVRYesButtonBackground.color = new Color(OVRYesButtonBackground.color.r, OVRYesButtonBackground.color.g, OVRYesButtonBackground.color.b, alpha);
            OVRNoButtonBackground.color = new Color(OVRNoButtonBackground.color.r, OVRNoButtonBackground.color.g, OVRNoButtonBackground.color.b, alpha);
            OVRMessage.color = new Color(OVRMessage.color.r, OVRMessage.color.g, OVRMessage.color.b, alpha);
            OVRYesButtonText.color = new Color(OVRYesButtonText.color.r, OVRYesButtonText.color.g, OVRYesButtonText.color.b, alpha);
            OVRNoButtonText.color = new Color(OVRNoButtonText.color.r, OVRNoButtonText.color.g, OVRNoButtonText.color.b, alpha);
        }
    }
}
