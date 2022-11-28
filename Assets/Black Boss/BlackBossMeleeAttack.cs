using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBossMeleeAttack : MonoBehaviour
{
    AudioSource audioSource;
    
    [SerializeField]
    private float atkDmg;
    [SerializeField]
    private float knockbackAmt = 1.0f;
  
    //if this works

    [SerializeField]
    private BlackBoss blackBoss;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.transform.tag != "Player") return;

      
        collision.transform.GetComponent<Entity>().LoseHP(atkDmg);
        collision.transform.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position).normalized * knockbackAmt, ForceMode2D.Impulse);

        blackBoss.MarkNewTarget();
    }
}
