using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotScript : MonoBehaviour
{
    [SerializeField]
    private float shotspeed;

    public bool startright;

    public float speedadd;

    public GameObject playershooting;

    [SerializeField]
    private float setlifetime = 10f;

    private float lifetime;



    void Start()
    {
        lifetime = setlifetime;


    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).Rotate(-1300f * Time.deltaTime, 0, 0);

        speedadd = playershooting.GetComponent<Rigidbody2D>().velocity.x;

        if (startright ==true)
        {
            GetComponent<Rigidbody2D>().velocity =( transform.right * (shotspeed + speedadd)) ;

        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = transform.right *    (-shotspeed + speedadd);
        }


        lifetime -= 10f * Time.deltaTime;

        if(lifetime <= 0)
        {
            bulletdies();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==6)
        {
            bulletdies();
        }

        if(collision.gameObject.name== "VinnaDebug")
        {
            collision.gameObject.GetComponent<Entity>().LoseHP(1f);
            bulletdies();
        }
    }

    private void bulletdies()
    {
        this.gameObject.SetActive(false);
        lifetime = setlifetime;
    }
}
