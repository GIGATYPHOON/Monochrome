using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Realtime;

public class Player_Info : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerInfo;

    public Player player;

    public void SetPlayer(Player p)
    {
        playerInfo.text = p.NickName;
        player = p;
    }
}
