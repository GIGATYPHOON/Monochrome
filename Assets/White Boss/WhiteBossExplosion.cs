using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBossExplosion : MonoBehaviour
{
    [SerializeField]
    AudioClip ExplosionSFX;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(ExplosionSFX, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Entity>()?.LoseHP(5.0f);
           

        }

    }

    public void Die()
    {
        Destroy(this.gameObject);
        
    }
}
