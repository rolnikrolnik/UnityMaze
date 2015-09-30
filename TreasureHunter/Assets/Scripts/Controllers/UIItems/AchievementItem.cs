using Treasure_Hunter.Managers;
using Treasure_Hunter.Models;
using Treasure_Hunter.Utils;
using UnityEngine;

namespace Treasure_Hunter.Controllers
{
    public class AchievementItem : MonoBehaviour
    {
        #region SCENE REFERENCES

        public MeshRenderer Border;
        public MeshRenderer Paper;
        public MeshRenderer Goblet;
        public TextMesh ProgressMesh;
        public TextMesh TitleMesh;
        //Manager
        public AchievementsManager AchievementsManager;

        #endregion

        #region PROPERTIES

        public Achievement Model { get; private set; }
        private static bool playerEnterOnTrigger = false;
        #endregion

        #region MONO BEHAVIOUR

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == Globals.PLAYER_TAG && !playerEnterOnTrigger)
            {
                if (SceneManager.Instance != null && SceneManager.Instance.BaseManager != null)
                {
                    SceneManager.Instance.BaseManager.AchievementsPopup.Show(Model, false);
                }
                playerEnterOnTrigger = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == Globals.PLAYER_TAG && !playerEnterOnTrigger)
            {
                if (SceneManager.Instance != null && SceneManager.Instance.BaseManager != null)
                {
                    SceneManager.Instance.BaseManager.AchievementsPopup.Show(Model, false);
                }
                playerEnterOnTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Globals.PLAYER_TAG)
            {
                if (SceneManager.Instance != null && SceneManager.Instance.BaseManager != null&&
                    SceneManager.Instance.BaseManager.AchievementsPopup.CurrentAchievement.Title.Equals(Model.Title))
                {
                    SceneManager.Instance.BaseManager.AchievementsPopup.Hide();
                    playerEnterOnTrigger = false;
                }
            }
        }

        #endregion

        public void SetData(string[] achievementParameters)
        {
            Model = new Achievement(achievementParameters);
            SetUiProperties();
        }

        private void SetUiProperties()
        {
            if(Model.CurrentThreshold==0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                Goblet.material = AchievementsManager.GetMaterialFromEnum(Model.IconEnum);
                TitleMesh.text = Model.Title + (Model.CurrentThreshold == 1 ? " I" : Model.CurrentThreshold == 2 ? " II" : " III");
                if (Model.CurrentThreshold < 3)
                {
                    ProgressMesh.text = Model.CurrentValue + "/" + (Model.CurrentThreshold == 1 ? Model.SecondThreshold : Model.ThirdThreshold);
                }
				else
				{
                    ProgressMesh.text = Model.ThirdThreshold + "+";
				}
            }
        }
    }
}
