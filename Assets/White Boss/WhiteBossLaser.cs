using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBossLaser : MonoBehaviour
{
    [SerializeField]
    AudioClip laser;

    private void OnEnable()
    {
        GetComponent<AudioSource>().PlayOneShot(laser, 0.5f);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Entity>().LoseHP(5f);
        }
    }
}
