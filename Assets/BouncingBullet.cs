using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : MonoBehaviour
{
    private Vector2 direction;
    [SerializeField]
    private float directionMultiplier = 1.0f;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private int maxBounces = 3;
    [SerializeField]
    private int currentBounces;

    private Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        MoveBullet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MoveBullet()
    {
        rigidbody2D.AddForce(direction * directionMultiplier, ForceMode2D.Impulse);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" || currentBounces >= maxBounces)
        {
            collision.transform.GetComponent<Entity>()?.LoseHP(3.0f);

            Destroy(this.gameObject); //To be changed to destroy over network
        }

        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9)
        {
            currentBounces++;
        }
    }
}
