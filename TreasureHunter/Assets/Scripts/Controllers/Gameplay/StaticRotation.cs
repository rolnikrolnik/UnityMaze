using UnityEngine;
using System.Collections;

namespace Treasure_Hunter.Controllers
{
    public class StaticRotation : MonoBehaviour
    {

        public Vector3 Rotation;

        private void Update()
        {
            transform.Rotate(Rotation);
        }
    }
}