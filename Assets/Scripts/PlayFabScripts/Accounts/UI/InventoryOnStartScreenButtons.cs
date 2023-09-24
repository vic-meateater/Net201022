using Assets.Scripts.PlayFabScripts.Accounts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFabScripts.Accounts.UI
{
    public class InventoryOnStartScreenButtons : MonoBehaviour
    {
        [SerializeField] private IsInventoryOnStartScreenButtonPressed _rewPressed;
        [SerializeField] private IsInventoryOnStartScreenButtonPressed _fwdPressed;
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private ScrollRect _scrollField;

        
        private void FixedUpdate()
        {
            if(_rewPressed.IsHolded)
            {
                _scrollField.horizontalNormalizedPosition -= _scrollSpeed * Time.deltaTime;
            }
        
            if(_fwdPressed.IsHolded)
            {
                _scrollField.horizontalNormalizedPosition += _scrollSpeed * Time.deltaTime;
            }

        }
    }
}
