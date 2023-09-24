using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFabScripts.Accounts.Windows
{
    public class SignInWindow : AccountDataWindowBase
    {
        [SerializeField] private Button _signInButton;

        protected override void SubscriptionsElementsUI()
        {
            base.SubscriptionsElementsUI();
            _signInButton.onClick.AddListener(SignIn);
        }

        private async void SignIn()
        {
            _overlay.enabled = true;
            await ExecuteSigningIn();
        }

        private async Task ExecuteSigningIn()
        {
            var res = false;
            PlayFabClientAPI.LoginWithPlayFab(
                new LoginWithPlayFabRequest
                {
                    Username = _userName,
                    Password = _userpasswd
                }, result =>
                {
                    Debug.Log($"{_userName} entered game");
                    _LoadingText.text = $"{_userName} entered game";
                    res = true;
                },
                error =>
                {
                    Debug.LogError($"Fail to enter in game: {error.ErrorMessage}");
                    _LoadingText.text = $"Fail to enter in game";
                    res = false;
                });
            
            await Task.Delay(3000);
            _overlay.enabled = false;
            if (res)
            {
                EnterInGameScene(); 
            }
        }
    }
}