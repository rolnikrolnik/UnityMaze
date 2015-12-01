using Treasure_Hunter.Enumerations;
using UnityEngine;

namespace Treasure_Hunter.Controllers
{
    public class MovementControllers : MonoBehaviour
    {
        public MovementDirections Direction;
        public bool IsActive { get; private set; }
        private bool isHover;

        private void OnMouseOver()
        {
            Quaternion CameraOrientation = Quaternion.identity;
            OVRDevice.GetOrientation(0, ref CameraOrientation);
            IsActive = true;
            isHover = true;
        }

        private void OnMouseExit()
        {
            IsActive = false;
            isHover = false;
        }
    }
}
