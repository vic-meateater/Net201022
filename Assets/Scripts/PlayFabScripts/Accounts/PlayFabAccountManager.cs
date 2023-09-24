using System;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabScripts.CharacterSlots;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFabScripts.Accounts
{
    public class PlayFabAccountManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleLabel;
        [SerializeField] private TMP_Text _howLongInGameLabel;
        [SerializeField] private Transform _itemScrollerTransform;
        [SerializeField] private InventoryElementOnStartScreenView _itemScrollerCellPrefab;

        [SerializeField] private GameObject _newCharacterCreatePanel;

        [SerializeField] private Button _createCharacterButton;
        [SerializeField] private TMP_InputField _inputField;

        [SerializeField] private List<SlotCharacterWidget> _slots;

        private string _characterName;
        
        private void Start()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
                                            OnGetAccountSuccess, OnError);
            //GetCatalogList();
            GetCharacters();

            foreach (var slot in _slots)
            {
                slot.SlotButton.onClick.AddListener(OpenPanelCreateCharacter);
            }
            
            _createCharacterButton.onClick.AddListener(CreateCharacterWithItem);
            _inputField.onValueChanged.AddListener(OnNameChange);
        }

        private void OnNameChange(string characterNameValue)
        {
            _characterName = characterNameValue;
        }

        private void CreateCharacterWithItem()
        {
            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
            {
                CharacterName = _characterName,
                ItemId = "hws"
            }, result =>
            {
                UpdateCharacterStatistics(result.CharacterId);
            },OnError);
        }

        private void UpdateCharacterStatistics(string characterId)
        {
            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
            {
                CharacterId = characterId,
                CharacterStatistics = new Dictionary<string, int>()
                {
                    {"Level",1},
                    {"Gold",0},
                    {"Might",10},
                    {"Defence",4},
                    {"Health",100},
                    {"Mana",80}
                    
                }
            }, result =>
            {
                Debug.Log($"Complete create character");
                ClosePanelCreateCharacter();
                GetCharacters();
            }, OnError);
        }

        private void OpenPanelCreateCharacter()
        {
            _newCharacterCreatePanel.SetActive(true);
        }
        
        private void ClosePanelCreateCharacter()
        {
            _newCharacterCreatePanel.SetActive(false);
        }

        private void GetCatalogList()
        {
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(),
                OnGetCatalogOnSuccess, OnError);
        }

        private void GetCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    Debug.Log($"{result.Characters.Count} characters loaded");
                    ShowCharactersInfo(result.Characters);
                },OnError);
        }

        private void ShowCharactersInfo(IReadOnlyList<CharacterResult> characters)
        {
            for (var index = 0; index < _slots.Count; index++)
            {
                if (characters.Count > index)
                {
                    var requestedIndex = index;
                    PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                        { 
                            CharacterId = characters[index].CharacterId
                        },
                        result =>
                        { 
                            var level = result.CharacterStatistics["Level"].ToString();
                            var gold = result.CharacterStatistics["Gold"].ToString();
                            var might = result.CharacterStatistics["Might"].ToString();
                            var defence = result.CharacterStatistics["Defence"].ToString();
                            var health = result.CharacterStatistics["Health"].ToString();
                            var mana = result.CharacterStatistics["Mana"].ToString();
                            _slots[requestedIndex].ShowInfoCharacterSlot(characters[requestedIndex].CharacterName, level, gold, might, defence, health, mana);
                        }, OnError);
                }
                else _slots[index].ShowEmptySlot();
                
            }
        }
    

        private void OnGetCatalogOnSuccess(GetCatalogItemsResult result)
        {
            ShowCatalog(result.Catalog);
            Debug.Log("CompleteLoadCatalog");
        }

        private void ShowCatalog(List<CatalogItem> catalog)
        {
            foreach (var item in catalog)
            {
                if (item.Bundle != null || item.Container != null) continue;
                
                var elementText = Instantiate(_itemScrollerCellPrefab, _itemScrollerTransform);
                elementText.SetName(item.DisplayName);
                elementText.SetDescription(item.Description);

                Debug.Log($"Item: {item.ItemId} - {item.DisplayName}");
            }
        }

        private void OnError(PlayFabError obj)
        {
            var errorMsg = obj.GenerateErrorReport();
            Debug.LogError(errorMsg);
        }

        private void OnGetAccountSuccess(GetAccountInfoResult result)
        {
            var accInfo = result.AccountInfo;
            _titleLabel.text = $"Welcome, {accInfo.Username}, {accInfo.PlayFabId}";
            _howLongInGameLabel.text = $"In game for: {(DateTime.Now - accInfo.Created).Days.ToString()} day(s).";
        }

        private void OnDestroy()
        {
            foreach (var slot in _slots)
            {
                slot.SlotButton.onClick.RemoveAllListeners();
            }
            _slots.Clear();
            
            _createCharacterButton.onClick.RemoveAllListeners();
            _inputField.onValueChanged.RemoveAllListeners();
        }
    }
}