using UnityEngine;
using UnityEngine.UI;

namespace Treasure_Hunter.Controllers
{
    public class ButtonCollider : MonoBehaviour
    {
        public Button ButtonComponent;

        private void OnMouseOver()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                ButtonComponent.onClick.Invoke();
                Debug.Log("Button pressed");
            }
        }
    }
}