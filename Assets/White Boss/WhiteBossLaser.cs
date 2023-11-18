using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBossLaser : MonoBehaviour
{
    [SerializeField]
    AudioClip LaserSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        GetComponent<AudioSource>().PlayOneShot(LaserSFX, 0.5f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Entity>().LoseHP(5f);
        }
    }
}
