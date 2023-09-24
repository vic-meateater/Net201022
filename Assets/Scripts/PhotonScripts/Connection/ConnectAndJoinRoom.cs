using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using PhotonScripts.Rooms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonScripts.Connection
{
    public class ConnectAndJoinRoom : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
    {
        [SerializeField] private ServerSettings _serverSettings;
        [SerializeField] private TMP_Text _stateUiText;

        private const string MONEY_PROP_KEY = "C0";
        private const string MAP_PROP_KEY = "C1";
        
        private LoadBalancingClient _lbc;
        private readonly TypedLobby _sqlLobby = new TypedLobby("sqlLobby", LobbyType.SqlLobby);
        
        [SerializeField]private RoomScrollerItem _roomScrollerPrefab;
        [SerializeField] private Transform _roomScrollerParent;

        [SerializeField] private Button _createRoom;
        [SerializeField] private Toggle _isClosedRoom;
        
        [SerializeField] private Button _createRoomForFriends;

        private List<RoomScrollerItem> _rooms = new();
        private List<Friends> _friends = new();

        private void Start()
        {
            _lbc = new LoadBalancingClient();
            _lbc.AddCallbackTarget(this);

            if (!_lbc.ConnectUsingSettings(_serverSettings.AppSettings))
                Debug.LogError($"Error! Failed to connect");
            
            _createRoom.onClick.AddListener(CreateSimpleRoom);
            _createRoomForFriends.onClick.AddListener(CreateRoomForFriends);
            
            //Loading friends from file or other service
            //sample friends adding
            _friends.Add(new Friends("Num1", "First friend", "1"));
            _friends.Add(new Friends("Num2", "Second friend", "2"));
        }

        private void CreateRoomForFriends()
        {
            var expectedFriends = new string[_friends.Count];
            for (var i = 0; i < _friends.Count; i++)
            {
                expectedFriends[i] = _friends[i].UserID;
            }

            var roomOptions = new RoomOptions
            {
                MaxPlayers = 4,
                IsOpen = false
            };
            
            var enterRoomParams = new EnterRoomParams
           {
               RoomName = $"{_lbc.LocalPlayer.NickName} room for friends",
               RoomOptions = roomOptions,
               ExpectedUsers = expectedFriends
            };
           
           _lbc.OpCreateRoom(enterRoomParams);
        }

        private void CreateSimpleRoom()
        {
            var roomOptions = new RoomOptions
            {
                MaxPlayers = 4,
            };
            
            if (_isClosedRoom.isOn)
                roomOptions.IsOpen = false;
            
            var enterRoomParams = new EnterRoomParams
            {
                RoomName = $"{_lbc.LocalPlayer.UserId} - simple room",
                RoomOptions = roomOptions
            };
            
            _lbc.OpCreateRoom(enterRoomParams);
        }

        private void Update()
        {
            _lbc?.Service();
            
            if (_lbc == null) return;
           _stateUiText.text = $"State: {_lbc.State.ToString()},\nUserID: {_lbc.UserId}";
        }

        public void OnConnected()
        {
            
        }

        public void OnConnectedToMaster()
        {
            Debug.Log($"OnConnectedToMaster");
            _lbc.OpJoinLobby(TypedLobby.Default);
        }

        public void OnDisconnected(DisconnectCause cause)
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
            var log = $"Room Created:\nRoomName: {_lbc.CurrentRoom.Name}\n";
            log += $"Is open: {_lbc.CurrentRoom.IsOpen}\n";

            if (_lbc.CurrentRoom.ExpectedUsers != null)
            {
                log += $"Expected users: ";
                log = _lbc.CurrentRoom.ExpectedUsers.Aggregate(log, (current, user) => current + $"{user}, ");
            }

            Debug.Log(log);
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            Debug.Log($"OnJoinedRoom");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"OnJoinRandomFailed");
            _lbc.OpCreateRoom(new EnterRoomParams());
        }

        public void OnLeftRoom()
        {
        }

        public void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
        }

        public void OnLeftLobby()
        {
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate");
            ClearRooms();
            
            foreach (var room in roomList)
            {
                var elementRoom = Instantiate(_roomScrollerPrefab, _roomScrollerParent);
                elementRoom.SetRoomInfo(room.Name, room.MaxPlayers.ToString(), room.PlayerCount.ToString());
                elementRoom._onConnectButtonPressed += ConnectToSelectedRoom;
                _rooms.Add(elementRoom);
            }
        }

        private void ConnectToSelectedRoom(string roomName)
        {
            _lbc.OpJoinRoom(new EnterRoomParams
            {
                RoomName = roomName
            });
        }

        private void ClearRooms()
        {
            if (_rooms.Count!=0)
            {
                foreach (var room in _rooms)
                {
                    room._onConnectButtonPressed -= ConnectToSelectedRoom;
                    Destroy(room.gameObject);
                }
            }

            _rooms?.Clear();
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }
        
        private void OnDestroy()
        {
            ClearRooms();
            _createRoom.onClick.RemoveAllListeners();
            _friends.Clear();
            _lbc.RemoveCallbackTarget(this);
        }
    }
}
