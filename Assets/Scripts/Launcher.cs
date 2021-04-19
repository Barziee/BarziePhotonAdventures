using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


namespace Barzie.Tiltan.FirstPUNGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("The maximum number of players per room")]
        [SerializeField] private byte maxPlayersPerRoom = 4;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField] private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField] private GameObject progressLabel;

        #endregion

        #region Private Fields
        string gameVersion = "1";
        #endregion

        #region Public Fields

        #endregion

        void Awake()
        {
            // Makes sure we can use PhotonNetwork.LoadLevel() on the master client
            // All clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        void Update()
        {

        }

        public void Conncet()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                // Attempt joining a random room
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // Connect this application instance to Photon Online Server
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            Debug.LogWarningFormat("OnDisconnected() was called by PUN with the reason {0}", cause);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called by PUN, creating new room. \nCalling: PhotonNetwork.CreateRoom");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() called by PUN, this client has joined a room");
        }

        #endregion
    }
}

