using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Events;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Linq;

public class Death_Manager : MonoBehaviourPunCallbacks
{

    //Check Health of Player and boss
    [SerializeField]
    GameObject WhiteBoss;

    GameObject[] Playersa;




    //If All players HP = 0 -> End Scene (Lose)

    //If boss HP=0 -> End scene (Win)

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        EndScreen();

    }


    public void EndScreen()
    {
        Playersa = GameObject.FindGameObjectsWithTag("Player");


        if (WhiteBoss.GetComponent<Entity>().returnHP() <= 0.0)
        {

            PhotonNetwork.MasterClient.SetWin(true);

            print(PhotonNetwork.MasterClient.GetWin() + "  " + PhotonNetwork.MasterClient.GetPlayerNumber());


            if (PhotonNetwork.MasterClient.GetWin() == true)
            {

                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel(2);
                Destroy(this);
            }

        }



        if (Playersa.Length == 0)
        {
            PhotonNetwork.MasterClient.SetWin(false);

            print(PhotonNetwork.MasterClient.GetWin() + "  " + PhotonNetwork.MasterClient.GetPlayerNumber());


            if (PhotonNetwork.MasterClient.GetWin() == false)
            {

                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel(2);
                Destroy(this);
            }

        }

    }
}
