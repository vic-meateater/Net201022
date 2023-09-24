using System;
using UnityEngine;
using Photon.Pun;
using System.Threading.Tasks;
using Photon.Realtime;
using Unity.Jobs;
using UnityEditor.VersionControl;
using UnityEngine.UI;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    public Action<bool> OnConnectionSuccess;
    [SerializeField] private Button _disconnect;
    [SerializeField] private Button _connect;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _disconnect.interactable = false;
        _disconnect.gameObject.SetActive(false);
        _disconnect.onClick.AddListener(Disconnect);
        _connect.onClick.AddListener(Connect);
    }

    private void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }

        ChangeButtons();
    }

    private void ChangeButtons()
    {
        _connect.gameObject.SetActive(!_connect.IsActive());
        _disconnect.gameObject.SetActive(!_disconnect.IsActive());
        
        _connect.interactable = !_connect.interactable;
        _disconnect.interactable = !_disconnect.interactable;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        
        PhotonNetwork.JoinLobby();
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log($"Connected To Lobby: {PhotonNetwork.CurrentLobby.Name}");
        
        PhotonNetwork.JoinOrCreateRoom(
            $"{PhotonNetwork.CurrentLobby.Name}.roomName{SystemInfo.deviceUniqueIdentifier}", 
            new Photon.Realtime.RoomOptions { MaxPlayers = 2, IsVisible = true },
            Photon.Realtime.TypedLobby.Default);
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log($"Connected To Lobby: {PhotonNetwork.CurrentRoom.Name}");
        OnConnectionSuccess?.Invoke(PhotonNetwork.InRoom);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        OnConnectionSuccess?.Invoke(PhotonNetwork.IsConnected);
        Debug.Log($"Disconnected");
        ChangeButtons();
    }

    private void OnDestroy()
    {
        _disconnect.onClick.RemoveAllListeners();
        _connect.onClick.RemoveAllListeners();
    }
}
