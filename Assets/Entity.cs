using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class Entity : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private float HP = 100;
    [SerializeField]
    private float MaxHP = 100;

    [SerializeField]
    private GameObject hpbar;

    public UnityEvent onHit;

    [SerializeField]
    bool playercontrolled;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

            photonView.RPC("HPBarSync", RpcTarget.AllBuffered, HP, MaxHP);
    }


    [PunRPC]
    void HPBarSync(float H, float MaxH)
    {

        if (hpbar != null)
            hpbar.GetComponentInChildren<Image>().fillAmount = H / MaxH;
    }



    public void LoseHP(float HPtoLose)
    {
        HP -= HPtoLose;
        onHit.Invoke();
    }

    public float returnHP()
    {
        return HP;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(HP);
        }
        else
        {
            // Network player, receive data
            HP = (float)stream.ReceiveNext();
        }
    }

}
