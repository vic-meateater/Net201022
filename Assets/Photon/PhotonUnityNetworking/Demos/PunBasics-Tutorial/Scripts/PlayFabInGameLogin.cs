using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;
using Task = UnityEditor.VersionControl.Task;

namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649
    public class PlayFabInGameLogin : MonoBehaviour
    {
        private const string TitleId = "921EC";
        private const string AuthGuidKey = "auth_guid";

        private string _userPlayFabId;

        private Action<int> _setPlayerHealth;

        public void SetPlayerHealth(Action<int> setPlayerHealth)
        {
            _setPlayerHealth = setPlayerHealth;
        }
        
        public void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = TitleId;
            }

            var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
            var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());
            
            var request = new LoginWithCustomIDRequest
            {
                CustomId = id,
                CreateAccount = !needCreation
            };
            PlayFabClientAPI.LoginWithCustomID(request, success =>
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                OnLoginSuccess(success);
            }, OnLoginFailed);
        }

        private void OnLoginFailed(PlayFabError res)
        {
            var errorMessage = res.GenerateErrorReport();
            Debug.LogError($"Error: {errorMessage}");
        }

        private void OnLoginSuccess(LoginResult res)
        {
            Debug.Log("Logging complete");
            _userPlayFabId = res.PlayFabId;
            SetUserData(res.PlayFabId);
            GetInventory();
        }

        private void SetUserData(string PlayFabId)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>
                    {
                        {"time_recieve_daily_reward",DateTime.UtcNow.ToString()},
                        {"player_hp", "100"}
                    }
                },
                result =>
                {
                    Debug.Log("UpdateUserData complete");
                    LogUserData(PlayFabId, "time_recieve_daily_reward");
                    GetPlayerHP(PlayFabId, "player_hp");
                },
                OnLoginFailed);
        }

        private void GetPlayerHP(string playFabId, string key)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest
                {
                    PlayFabId = playFabId
                },
                result =>
                {
                    Debug.Log($"{key} = {result.Data[key].Value}");
                    _setPlayerHealth?.Invoke(Convert.ToInt32(result.Data[key].Value));
                },
                OnLoginFailed);
        }

        private void LogUserData(string playFabId, string key)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest
                {
                    PlayFabId = playFabId
                },
                result =>
                {
                    Debug.Log($"{key} = {result.Data[key].Value}");
                },
                OnLoginFailed);
        }

        private void MakePurchase()
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
                {
                    CatalogVersion = "TestCatalog",
                    ItemId ="shp",
                    Price = 100,
                    VirtualCurrency = "CO"
                }, result =>
                {
                    Debug.Log($"Complete purchase");
                },
                OnLoginFailed);
        }

        private void GetInventory()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), 
                result => LogSuccess(result.Inventory), OnLoginFailed);
        }

        private void LogSuccess(IEnumerable<ItemInstance> resultInventory)
        {
            var list= resultInventory.Aggregate("", (current, item) => current + $"{item.DisplayName}, ");
            Debug.Log(list);
        }
        
    }
}