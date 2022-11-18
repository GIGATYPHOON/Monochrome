using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBossLaser : MonoBehaviour
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
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Entity>().LoseHP(1f);
        }
    }
}
