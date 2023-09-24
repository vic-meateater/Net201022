using TMPro;
using UnityEngine;
public class ConnectionStatus : MonoBehaviour
{
   private TMP_Text _status;
   [SerializeField]private PhotonLauncher _launcher;

   private void Awake()
   {
      _launcher= FindObjectOfType<PhotonLauncher>();
      _launcher.OnConnectionSuccess += ChangeConnectionStatus;
      _status = GetComponent<TMP_Text>();
   }

   private void ChangeConnectionStatus(bool state)
   {
      if (state)
      {
         _status.color=Color.green;
         _status.text = "Connected";
      }
      else
      {
         _status.color=Color.red;
         _status.text = "Disconnected";
      }
   }

   private void OnDestroy()
   {
      _launcher.OnConnectionSuccess -= ChangeConnectionStatus;
   }
}
