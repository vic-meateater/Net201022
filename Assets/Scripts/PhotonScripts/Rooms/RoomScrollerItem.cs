using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonScripts.Rooms
{
    public class RoomScrollerItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomName;
        [SerializeField] private TMP_Text _roomMaxPlayers;
        [SerializeField] private TMP_Text _roomCurrentPlayers;
        [SerializeField] private Button _connectToRoom;

        public Action<string> _onConnectButtonPressed;

        private void Start()
        {
            _connectToRoom.onClick.AddListener(Connect);
        }

        private void Connect()
        {
            _onConnectButtonPressed?.Invoke(_roomName.text);
        }

        private void OnDestroy()
        {
            _connectToRoom.onClick.RemoveAllListeners();
        }

        public void SetRoomInfo(string roomName, string maxPlayers, string currentPlayers)
        {
            _roomName.text = roomName;
            _roomCurrentPlayers.text = currentPlayers;
            _roomMaxPlayers.text = maxPlayers;
        }
    }
}