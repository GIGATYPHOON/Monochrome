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

    private bool isreflected = false;

    private Vector3 setvelocity;



    void Start()
    {

        lifetime = setlifetime;


    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).Rotate(-1300f * Time.deltaTime, 0, 0);

        speedadd = playershooting.GetComponent<Rigidbody2D>().velocity.x;


        if(isreflected )
        {
            //GetComponent<Rigidbody2D>().velocity = setvelocity;
        }
        else
        {
            if (startright == true)
            {
                GetComponent<Rigidbody2D>().velocity = (transform.right * (shotspeed + speedadd));

            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = transform.right * (-shotspeed + speedadd);
            }
        }



        lifetime -= 10f * Time.deltaTime;

        if(lifetime <= 0)
        {
            bulletdies();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==6 || collision.gameObject.layer == 9)
        {
            bulletdies();
        }

        if(collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<WhiteBoss>())
            {
                if (collision.gameObject.GetComponent<WhiteBoss>().invincible)
                {

                }

                else
                {
                    collision.gameObject.GetComponent<Entity>().LoseHP(1f);
                    bulletdies();
                }
            }
            else
            {
                collision.gameObject.GetComponent<Entity>().LoseHP(1f);
                bulletdies();
            }
                
            
        }

        if (collision.gameObject.tag == "Player" && isreflected == true)
        {
            collision.gameObject.GetComponent<Entity>().LoseHP(1f);
            bulletdies();
        }

    }

    private void bulletdies()
    {
        this.gameObject.SetActive(false);
        lifetime = setlifetime;
        setvelocity = Vector3.zero;
        isreflected = false;
    }

    public void reflect()
    {
        //setvelocity = (Vector3.Normalize(GameObject.Find("Player").transform.position - this.transform.position) * 12f);
        isreflected = true;

    }
}
