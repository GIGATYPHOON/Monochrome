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
    private float damageMultiplier = 1.0f;
    public bool isVulnerable = true;

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

        HPBarSync(HP, MaxHP);

        if(playercontrolled)
        {
            hpbar.GetComponentInChildren<Image>().color = Color.yellow;

            if (!photonView.IsMine)
                return;

            hpbar.GetComponentInChildren<Image>().color = Color.green;

        }
    }



    void HPBarSync(float H, float MaxH)
    {       
        if (hpbar != null)
            hpbar.GetComponentInChildren<Image>().fillAmount = H / MaxH;
    }



    public void LoseHP(float HPtoLose)
    {
        if (!isVulnerable) return;

        HP -= HPtoLose * damageMultiplier;
        onHit.Invoke();
    }

    public float returnHP()
    {
        return HP;
    }

    public float returnMaxHP()
    {
        return MaxHP;
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
