using UnityEngine;

namespace Treasure_Hunter.Controllers
{
    public class LightSwordColor : MonoBehaviour
    {
        #region PROJECT REFERENCES

        public Material[] AvailableColors;
        public MeshRenderer Sword;

        #endregion

        #region MONO BEHAVIOUR

        private void Start()
        {
            Sword.material = AvailableColors[Random.Range(0, AvailableColors.Length - 1)];
        }

        #endregion
    }
}
