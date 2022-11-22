using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField]
    private int playerIndex = 0;
    [SerializeField]
    private GameObject connected, waitingForConnection;
    [SerializeField]
    private TextMeshProUGUI playerName, playerScore;
    [SerializeField]
    private Image playerIcon;

    private void OnEnable()
    {
        Network_Manager.OnPlayerNumberingUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        Network_Manager.OnPlayerNumberingUpdated -= UpdateUI;
    }

    // Start is called before the first frame update
    private void Start()
    {
        connected.SetActive(false);
        waitingForConnection.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateUI();
    }

    //Setting the values based on the player list
    private void UpdateUI()
    {
        //Check if there is an available player with our index from the player list
        if (playerIndex <= PhotonNetwork.PlayerList.Length - 1)
        {
            Player player = PhotonNetwork.PlayerList[playerIndex];

            //Make sure that the player number is already assigned before updating any information
            if (player.GetPlayerNumber() == -1)
                return;

            connected.SetActive(true);
            waitingForConnection.SetActive(false);

            //DebugManager.Instance.Log(string.Format("player number/actor id : name = {0}/{1} : {2}",
            //    player.GetPlayerNumber(), player.ActorNumber, player.NickName));
            playerName.text = player.NickName;
            playerScore.text = player.GetScore().ToString("0000");
            playerIcon.sprite = Network_Manager.Instance.GetPlayerSprite(player.GetPlayerNumber());
        }
        //If there is no player with the index
        else
        {
            connected.SetActive(false);
            waitingForConnection.SetActive(true);
        }
    }
}

