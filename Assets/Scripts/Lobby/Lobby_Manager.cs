using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Lobby_Manager : SingletonPun<Lobby_Manager>
{
    [SerializeField] private GameObject connectionPanel, loadingPanel, roomListPanel, playerListPanel, chooseboss;
    private byte maxPlayersPerRoom = 4;
    private DebugLog debugger;

    [SerializeField]
    private bool WhiteBossSelected = true;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        debugger = DebugLog.Instance;
        ShowPanel("Network");
    }

    public override void OnConnectedToMaster()
    {
        debugger.Log("Connected to the master server.", "green");
        debugger.Log("Trying to join default lobby.");
        //Join the lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        debugger.Log("Successfully joined lobby.", "green");
        ShowPanel("Room List");
    }

    public override void OnCreatedRoom()
    {
        debugger.Log("Room successfully created!", "green");
        ShowPanel("player listooo");
    }

    public override void OnJoinedRoom()
    {
        ShowPanel("player listooo");
    }
    public override void OnLeftRoom()
    {
        ShowPanel("Room List");
    }

    public void Connect()
    {
        // Check first if we are already connected to the Photon Network
        if (PhotonNetwork.IsConnected)
        {
            debugger.Log("Already connected to photon network.");
        }
        else
        {
            debugger.Log("Not yet connected to photon network. Trying to connect..");
            PhotonNetwork.ConnectUsingSettings();
            ShowPanel("Loading 100years");
        }
    }

    public void ShowPanel(string panelName)
    {
        loadingPanel.SetActive(panelName.Equals(loadingPanel.name));
        connectionPanel.SetActive(panelName.Equals(connectionPanel.name));
        roomListPanel.SetActive(panelName.Equals(roomListPanel.name));
        playerListPanel.SetActive(panelName.Equals(playerListPanel.name));

    }

    public void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayersPerRoom;
        options.IsVisible = true;
        options.IsOpen = true;

        debugger.Log("Trying to create a room...");
        ShowPanel("Loading 100years");
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName + "'s Room", options);
    }

    public void LeaveRoom()
    {
        debugger.Log("Trying to leave the room...");
        ShowPanel("Loading 100years");
        PhotonNetwork.LeaveRoom();
    }
    public void StartGame()
    {
        //Load the game scene here

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }

    }




    public void StartGAMME()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

        if (chooseboss.GetComponent<Toggle>().isOn)
        {
            PhotonNetwork.LoadLevel(1);
        }
        else
        {
            PhotonNetwork.LoadLevel(3);
        }


    }
}
