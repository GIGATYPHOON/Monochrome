using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class Network_Manager : SingletonPun<Network_Manager>
{
    private const string playerPefabName = "Player";



    [SerializeField]
    private Sprite[] playerSprites;

    //Create our own event/delegate for updating the player index
    public delegate void PlayerNumberingStatus();
    public static event PlayerNumberingStatus OnPlayerNumberingUpdated;

    public override void OnEnable()
    {
        base.OnEnable();
        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayerNumbering;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayerNumbering;
    }

    private void Start()
    {
        //Check if we are connected to the Photon Network
        if (!PhotonNetwork.IsConnected)
        {
            //Load the Title screen instead
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }

        //Instantiate a player gameObject to represent our local client


        PhotonNetwork.Instantiate(playerPefabName, Vector3.up * 100f, Quaternion.identity);

    }

    private void UpdatePlayerNumbering()
    {
        OnPlayerNumberingUpdated?.Invoke();
    }

    public Sprite GetPlayerSprite(int playerNumber)
    {
        return playerSprites[playerNumber];
    }
}
