using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Events;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Linq;

public class Return : MonoBehaviour
{
    [SerializeField]
    GameObject EndScreen;


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void EndScreee()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel(0);

        PhotonNetwork.LeaveRoom();

        Destroy(this);

    }
}
