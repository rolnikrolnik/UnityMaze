using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Treasure_Hunter.Controllers
{
    public class HealthBar : MonoBehaviour
    {
        #region SCENE REFERENCES

        public Slider HealthSlider;
        public Text TextLabel;

        #endregion

        public void SetValue(float value)
        {
            HealthSlider.value = value;
            TextLabel.text = (value*100).ToString("F", CultureInfo.InvariantCulture) + "%";
        }
    }
}
