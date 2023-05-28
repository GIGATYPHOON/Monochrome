using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
public class PlayerList : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Player_Info playerPrefab;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private TextMeshProUGUI roomName;
    [SerializeField]
    private Button playGame;
    [SerializeField]
    private Toggle chooseboss;

    private List<Player_Info> listings = new List<Player_Info>();

    public override void OnEnable()
    {
        base.OnEnable();
        UpdateCurrentRoomPlayers();
        HandleUI();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        //Destroy each prefab in the listings
        foreach (Player_Info entry in listings)
            Destroy(entry.gameObject);
        //clear the list
        listings.Clear();
    }

    //Whenever the local player exits the room
    public override void OnLeftRoom()
    {
        //Destroy each prefab in the listings
        foreach (Player_Info entry in listings)
            Destroy(entry.gameObject);
        //clear the list
        listings.Clear();
        Lobby_Manager.Instance.ShowPanel("player listoo");
    }

    //When another joins the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    //When another exits
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Check if the master client has been transferred
        HandleUI();
        //Get the index of the player that has the Player information as of the otherPlayer
        int playerIndex = listings.FindIndex(p => p.player == otherPlayer);

        //If we found a player, delete from the listings
        if (playerIndex != -1)
        {
            Destroy(listings[playerIndex].gameObject);
            listings.RemoveAt(playerIndex);
        }
    }

    private void HandleUI()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        //Only enable the start button if you are the master client of the room
        playGame.gameObject.SetActive(PhotonNetwork.CurrentRoom.MasterClientId == PhotonNetwork.LocalPlayer.ActorNumber);
        chooseboss.gameObject.SetActive(PhotonNetwork.CurrentRoom.MasterClientId == PhotonNetwork.LocalPlayer.ActorNumber);
    }

    private void UpdateCurrentRoomPlayers()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            AddPlayerListing(p);
        }
    }

    private void AddPlayerListing(Player p)
    {
        //Add player listing
        Player_Info info = Instantiate(playerPrefab, content);
        info.SetPlayer(p);
        listings.Add(info);
    }
}

