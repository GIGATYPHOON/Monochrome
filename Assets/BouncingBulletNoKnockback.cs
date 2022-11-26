using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBulletNoKnockback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.transform.GetComponent<Entity>()?.LoseHP(2.0f);

            transform.parent.GetComponent<BouncingBullet>().bulletdies(); //To be changed to destroy over network

            print("wow");
        }
    }
}

