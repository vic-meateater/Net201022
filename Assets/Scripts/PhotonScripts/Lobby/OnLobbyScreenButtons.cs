using UnityEngine;
using UnityEngine.UI;

namespace PhotonScripts.Lobby
{
    public class OnLobbyScreenButtons : MonoBehaviour
    {
        [SerializeField] private IsLobbyScrollingButtonPressed _upPressed;
        [SerializeField] private IsLobbyScrollingButtonPressed _downPressed;
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private ScrollRect _scrollField;

        
        private void FixedUpdate()
        {
            if(_upPressed.IsHolded)
            {
                _scrollField.horizontalNormalizedPosition -= _scrollSpeed * Time.deltaTime;
            }
        
            if(_downPressed.IsHolded)
            {
                _scrollField.horizontalNormalizedPosition += _scrollSpeed * Time.deltaTime;
            }

        }
    }
}