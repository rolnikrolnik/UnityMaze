using UnityEngine;

namespace Treasure_Hunter.TestScripts
{
    public class BaseSceneLights : MonoBehaviour
    {
        #region SCENE REFERENCES

        public Light MainLight;

        #endregion

        public Color NORMAL_COLOR;
        public Color ALTERNATIVE_COLOR;
        public float MaxIntensity;
        public float MinIntensity;
        public int NumberOfThresholds;
        private int currentTreshold;

        #region MONO BEHAVIOUR

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                currentTreshold = currentTreshold <= 0 ? 0 : (currentTreshold - 1);
                Debug.LogWarning("Minus pressed, currentTreshold=" + currentTreshold);
                SetColorAndIntensity();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                currentTreshold = currentTreshold >= NumberOfThresholds ? NumberOfThresholds : (currentTreshold + 1);
                Debug.LogWarning("Plus pressed, currentTreshold=" + currentTreshold);
                SetColorAndIntensity();
            }
        }

        #endregion

        private void SetColorAndIntensity()
        {
            MainLight.color = Color.Lerp(NORMAL_COLOR, ALTERNATIVE_COLOR, (float)currentTreshold / (float)NumberOfThresholds);
            MainLight.intensity = MinIntensity - (MinIntensity - MaxIntensity) * ((float)currentTreshold / (float)NumberOfThresholds);
        }
    }
}