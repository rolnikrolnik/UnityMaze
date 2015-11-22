using UnityEngine;
using System.Collections;
using Treasure_Hunter.Utils;
using Treasure_Hunter.Managers;

namespace Treasure_Hunter.Controllers
{
    public class FinalRoomController : MonoBehaviour
    {
        #region MONO BEHAVIOUR

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == Globals.PLAYER_TAG)
            {
                //Show information about end of the level
                //Win animation
                SceneManager.Instance.BackToBase();
            }
        }

        #endregion
    }
}