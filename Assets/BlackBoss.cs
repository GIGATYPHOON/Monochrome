using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoss : MonoBehaviour
{
    [SerializeField]
    private float invulnInterval = 3.0f;
    [SerializeField]
    private float vulnInterval = 4.0f;
    private float vulnIntervalTimer = 0.0f;

    [SerializeField]
    private float pullForce;
    [SerializeField]
    private float pullInterval = 5.0f;
    private float pullIntervalTimer = 0.0f;

    [SerializeField]
    private List<Transform> playerList; //To be replaced with Photon player list when implemented

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleVulnerability();
        HandlePlayerPulling();
    }

    private void HandleVulnerability()
    {
        if (vulnIntervalTimer > 0)
            vulnIntervalTimer -= Time.deltaTime;

        else
        {
            if (GetComponent<Entity>().isVulnerable)
            {
                GetComponent<Entity>().isVulnerable = false;
                vulnIntervalTimer = invulnInterval;
            }
            else if (!GetComponent<Entity>().isVulnerable)
            {
                GetComponent<Entity>().isVulnerable = true;
                vulnIntervalTimer = vulnInterval;
            }
        }
    }

    private void HandlePlayerPulling()
    {
        if (pullIntervalTimer > 0)
            pullIntervalTimer -= Time.deltaTime;

        else
        {
            foreach (Transform player in playerList)
            {
                player.GetComponent<Rigidbody2D>().AddForce((transform.position - player.position).normalized * pullForce, ForceMode2D.Impulse);
            }

            pullIntervalTimer = pullInterval;
        }
    }
}
