using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject texttochange;


    void Start()
    {

    }

    private void OnEnable()
    {

    }

    private void Awake()
    {




        if (PhotonNetwork.MasterClient.GetWin() == true)
        {
            texttochange.GetComponent<TextMeshProUGUI>().text = "Y O U   W I N";
        }
        else
        {
            texttochange.GetComponent<TextMeshProUGUI>().text = "Y O U   L O S E";

        }


    }

    // Update is called once per frame
    void Update()
    {

    }


}
