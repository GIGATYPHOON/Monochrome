using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Events;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Linq;
using System.Diagnostics;

public class Return : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject EndScreen;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);


        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.JoinLobby();
        Destroy(this);
    }


    public void EndScreee()
    {

        PhotonNetwork.LeaveRoom();




        StartCoroutine(ExecuteAfterTime(1.0f));







    }
}
