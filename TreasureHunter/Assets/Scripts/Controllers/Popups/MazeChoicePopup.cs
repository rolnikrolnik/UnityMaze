using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Abstract;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Controllers
{
    public class MazeChoicePopup : AbstractPopup
    {
        #region CLASS SETTINGS

        private const string NECROPOLIS_TITLE = "NECROPOLIS\n\nMAZE";
        private const string PREHISTORIC_TITLE = "PREHISTORIC\n\nMAZE";
        private const string WORMSWORLD_TITLE = "WORMS\nWORLD\nMAZE";
        private const string SWAMP_TITLE = "SWAMP\nMAZE";

        private const string NECROPOLIS_MESSAGE = "MESSAGE";
        private const string PREHISTORIC_MESSAGE = "MESSAGE";
        private const string WORMSWORLD_MESSAGE = "MESSAGE";
        private const string SWAMP_MESSAGE = "MESSAGE";

        #endregion

        #region PROJECT REFERENCES

        public Sprite PrehistoricMazeSprite;
        public Sprite NecropolisMazeSprite;
        public Sprite SwampMazeSprite;
        public Sprite WormsWorldMazeSprite;

        #endregion

        #region SCENE REFERENCES

        //Standalone
        public Image StandaloneButtonBackground;
        public Image StandaloneMazeLogo;
        public Text StandaloneTitle;
        public Text StandaloneMessage;
        public Text StandaloneButtonText;
        //OVR
        public Image OVRButtonBackground;
        public Image OVRMazeLogo;
        public Text OVRTitle;
        public Text OVRMessage;
        public Text OVRButtonText;

        #endregion

        private bool isEnabled = true;
        private MazeType mazeType;

        #region INPUT HANDLING

        public void OnPlayClick()
        {
            if (isEnabled)
            {
                isEnabled = false;
                Hide();
                SceneManager.Instance.LoadMaze(mazeType);
                if (SceneManager.Instance.BaseManager != null)
                {
                    SceneManager.Instance.BaseManager.Player.AnyPopupIsVisible = false;
                }
            }
        }

        #endregion

        private void SetData(string title, string message, Sprite logo)
        {
            OVRTitle.text = title;
            StandaloneTitle.text = title;
            OVRMessage.text = message;
            StandaloneMessage.text = message;
            OVRMazeLogo.sprite = logo;
            StandaloneMazeLogo.sprite = logo;
        }

        public void CheckMazeType(MazeType _mazeType)
        {
            mazeType = _mazeType;
            switch (mazeType)
            {
                case MazeType.PREHISTORIC_MAZE:
                    SetData(PREHISTORIC_TITLE, PREHISTORIC_MESSAGE, PrehistoricMazeSprite);
                    break;
                case MazeType.NECROPOLIS_MAZE:
                    SetData(NECROPOLIS_TITLE, NECROPOLIS_MESSAGE, NecropolisMazeSprite);
                    break;
                case MazeType.WORMSWORLD_MAZE:
                    SetData(WORMSWORLD_TITLE, WORMSWORLD_MESSAGE, WormsWorldMazeSprite);
                    break;
                case MazeType.SWAMP_MAZE:
                    SetData(SWAMP_TITLE, SWAMP_MESSAGE, SwampMazeSprite);
                    break;
            }
        }

        protected override void SetAlphaChannel(float alpha)
        {
            base.SetAlphaChannel(alpha);
            OVRButtonBackground.color = new Color(OVRButtonBackground.color.r, OVRButtonBackground.color.g, OVRButtonBackground.color.b, alpha);
            OVRMazeLogo.color = new Color(OVRMazeLogo.color.r, OVRMazeLogo.color.g, OVRMazeLogo.color.b, alpha);
            OVRTitle.color = new Color(OVRTitle.color.r, OVRTitle.color.g, OVRTitle.color.b, alpha);
            OVRMessage.color = new Color(OVRMessage.color.r, OVRMessage.color.g, OVRMessage.color.b, alpha);
            OVRButtonText.color = new Color(OVRButtonText.color.r, OVRButtonText.color.g, OVRButtonText.color.b, alpha);
            StandaloneButtonBackground.color = new Color(StandaloneButtonBackground.color.r, StandaloneButtonBackground.color.g, StandaloneButtonBackground.color.b, alpha);
            StandaloneMazeLogo.color = new Color(StandaloneMazeLogo.color.r, StandaloneMazeLogo.color.g, StandaloneMazeLogo.color.b, alpha);
            StandaloneTitle.color = new Color(StandaloneTitle.color.r, StandaloneTitle.color.g, StandaloneTitle.color.b, alpha);
            StandaloneMessage.color = new Color(StandaloneMessage.color.r, StandaloneMessage.color.g, StandaloneMessage.color.b, alpha);
            StandaloneButtonText.color = new Color(StandaloneButtonText.color.r, StandaloneButtonText.color.g, StandaloneButtonText.color.b, alpha);
        }
    }
}
