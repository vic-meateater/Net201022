using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.PlayFabScripts.Accounts.UI
{
    public class IsInventoryOnStartScreenButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        private bool _isHolded;
        public bool IsHolded => _isHolded;

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            _isHolded = true;
        }

        public void OnPointerUp(PointerEventData pointerEventData)
        {
            _isHolded = false;
        }
    }
}