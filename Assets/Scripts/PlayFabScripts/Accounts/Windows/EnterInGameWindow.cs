using UnityEngine;
using UnityEngine.UI;

namespace PlayFabScripts.Accounts.Windows
{
    public class EnterInGameWindow : MonoBehaviour
    {
        [SerializeField] private Button _signInButton;
        [SerializeField] private Button _createAccButton;
        
        [SerializeField] private Canvas _enterGameCanvas;
        [SerializeField] private Canvas _createAccountCanvas;
        [SerializeField] private Canvas _signInCanvas;

        private void Start()
        {
            _signInButton.onClick.AddListener(OpenSignInWindow);
            _createAccButton.onClick.AddListener(OpenCreateAccWindow);
            
            _signInCanvas.enabled = false;
            _createAccountCanvas.enabled = false;
            _enterGameCanvas.enabled = true;
        }

        private void OpenCreateAccWindow()
        {
            _createAccountCanvas.enabled = true;
            _enterGameCanvas.enabled = false;
        }

        private void OpenSignInWindow()
        {
            _signInCanvas.enabled = true;
            _enterGameCanvas.enabled = false;
        }

        private void OnDestroy()
        {
            _signInButton.onClick.RemoveAllListeners();
            _createAccButton.onClick.RemoveAllListeners();
        }
    }
}