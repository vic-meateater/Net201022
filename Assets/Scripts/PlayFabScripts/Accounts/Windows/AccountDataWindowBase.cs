using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlayFabScripts.Accounts.Windows
{
    public class AccountDataWindowBase : MonoBehaviour
    {
        [SerializeField] private InputField _userNameField;
        [SerializeField] private InputField _userpasswdField;
        [SerializeField] protected Canvas _overlay;
        [SerializeField] protected TMP_Text _LoadingText;

        protected string _userName;
        protected string _userpasswd;

        private void Start()
        {
            SubscriptionsElementsUI();
            
        }

        protected virtual void SubscriptionsElementsUI()
        {
            _userNameField.onValueChanged.AddListener(UpdateUserName);
            _userpasswdField.onValueChanged.AddListener(UpdateUserPasswd);
        }

        private void UpdateUserPasswd(string pass)
        {
            _userpasswd = pass;
        }

        private void UpdateUserName(string userName)
        {
            _userName=userName;
        }

        protected void EnterInGameScene()
        {
            SceneManager.LoadScene(1);
        }
        private void OnDestroy()
        {
            _userNameField.onValueChanged.RemoveAllListeners();
            _userpasswdField.onValueChanged.RemoveAllListeners();
        }
    }
}