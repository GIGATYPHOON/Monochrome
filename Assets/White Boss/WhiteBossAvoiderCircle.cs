using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBossAvoiderCircle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9)

        {
            //transform.parent.transform.position += Vector3.Normalize(new Vector2(transform.parent.transform.position.x, transform.parent.transform.position.y) - collision.ClosestPoint(transform.position)) *5.3f * Time.deltaTime ; // top right

            transform.parent.transform.position = Vector2.MoveTowards(transform.parent.transform.position, collision.ClosestPoint(transform.position),  -1 * Time.deltaTime * (1f- Vector2.Distance(transform.parent.transform.position, collision.ClosestPoint(transform.position))));



            //print( 1-Vector2.Distance(transform.parent.transform.position, collision.ClosestPoint(transform.position)));
        }


    }
}
