using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;


namespace PlayFabScripts.Accounts.Windows
{
    public class CreateAccountWindow : AccountDataWindowBase
    {
        [SerializeField] private InputField _emailField;
        [SerializeField] private Button _submitButton;
        
        private string _email;

        
        protected override void SubscriptionsElementsUI()
        {
            base.SubscriptionsElementsUI();
            _emailField.onValueChanged.AddListener(UpdateEmail);
            _submitButton.onClick.AddListener(CreateAccount);
        }

        private void UpdateEmail(string email)
        {
            _email = email;
        }

        private async void CreateAccount()
        {
            _overlay.enabled = true;
            await RegisterUser();
        }

        private async Task RegisterUser()
        {
            var res = false;
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
                {
                    Username = _userName,
                    Email = _email,
                    Password = _userpasswd
                }, result =>
                {
                    Debug.Log($"Account for: {_userName} created");
                    _LoadingText.text = $"Account for: {_userName} created";
                    res = true;
                },
                error =>
                {
                    Debug.LogError($"Fail to create account: {error.ErrorMessage}");
                    _LoadingText.text = $"Fail to create account";
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