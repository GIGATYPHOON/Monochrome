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

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void EndScreee()
    {
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LeaveRoom();



        Destroy(this);

    }
}
