using Treasure_Hunter.Enumerations;
using UnityEngine;

namespace Treasure_Hunter.Controllers
{
    public class MovementControllers : MonoBehaviour
    {
        public MovementDirections Direction;
        public bool IsActive { get; private set; }

        private void OnMouseOver()
        {
            IsActive = true;
        }

        private void OnMouseExit()
        {
            IsActive = false;
        }
    }
}
