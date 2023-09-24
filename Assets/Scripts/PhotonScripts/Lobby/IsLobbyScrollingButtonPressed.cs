using UnityEngine;
using UnityEngine.EventSystems;

namespace PhotonScripts.Lobby
{
    public class IsLobbyScrollingButtonPressed : MonoBehaviour
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