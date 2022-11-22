using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnHandler : MonoBehaviour
{
    int i = 0;

    [SerializeField]
    private GameObject playerspawns;

    void Start()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.transform.position = playerspawns.transform.GetChild(i).position;


            i++;
        }

        ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
