using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class PlayerSpawnHandler : MonoBehaviourPunCallbacks
{
    int i = 0;

    [SerializeField]
    private GameObject[] spawns;

    [SerializeField]
    private GameObject[] players;

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay

        foreach (Player retard in PhotonNetwork.PlayerList)
        {
            print("before "+retard.GetPlayerNumber());

            retard.SetPlayerNumber(i);
            //GameObject.FindGameObjectsWithTag("Player")[retard.GetPlayerNumber()].transform.position = spawns[i].transform.position;

            players = GameObject.FindGameObjectsWithTag("Player");

            players[i].name = "Player " + i;


            players[1].transform.position = spawns[0].transform.position;


            //print(GameObject.FindGameObjectsWithTag("Player")[retard.GetPlayerNumber()] + " " + spawns[i].transform);

            print("after "+retard.GetPlayerNumber());
            i++;
        }
    }

    private void Awake()
    {

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(ExecuteAfterTime(0.5f));







    }


    private void Update()
    {



    }


}
