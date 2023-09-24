using System;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFabScripts.Accounts
{
    public class PlayFabLogin : MonoBehaviour
    {
        private const string TitleId = "921EC";
        private const string AuthGuidKey = "auth_guid";
        private void Start()
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
            SetUserData(res.PlayFabId);
            MakePurchase();
            GetInventory();
        }

        private void SetUserData(string PlayFabId)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    {"time_recieve_daily_reward",DateTime.UtcNow.ToString()}
                }
            },
                result =>
                {
                    Debug.Log("UpdateUserData complete");
                    GetUserData(PlayFabId, "time_recieve_daily_reward");
                },
                OnLoginFailed);
        }

        private void GetUserData(string playFabId, string key)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest
            {
                PlayFabId = playFabId
            },
                result =>
                {
                    Debug.Log($"{key}: {result.Data[key].Value}");
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
