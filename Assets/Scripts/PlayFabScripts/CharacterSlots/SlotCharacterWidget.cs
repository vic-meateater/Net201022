using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFabScripts.CharacterSlots
{
    public class SlotCharacterWidget : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [SerializeField] private GameObject _emptySlot;
        [SerializeField] private GameObject _characterSlot;

        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private TMP_Text _levelLabel;
        [SerializeField] private TMP_Text _goldLabel;
        [SerializeField] private TMP_Text _mightLabel;
        [SerializeField] private TMP_Text _defenceLabel;
        [SerializeField] private TMP_Text _healthLabel;
        [SerializeField] private TMP_Text _manaLabel;
    
        public Button SlotButton => _button;

        public void ShowInfoCharacterSlot(string nameValue, string level, string gold,string might,string defence,string health,string mana)
        {
            _nameLabel.text = $"Name: {nameValue}";
            _levelLabel.text = $"Level: {level}";
            _goldLabel.text = $"Gold: {gold}";
            _mightLabel.text = $"Might: {might}";
            _defenceLabel.text= $"Defence: {defence}";
            _healthLabel.text= $"Health: {health}";
            _manaLabel.text= $"Mana: {mana}";
            
            
            _characterSlot.SetActive(true);
            _emptySlot.SetActive(false);
        }

        public void ShowEmptySlot()
        {
            _characterSlot.SetActive(false);
            _emptySlot.SetActive(true);
        }
    }
}
