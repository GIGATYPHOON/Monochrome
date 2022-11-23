using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBossAura : MonoBehaviour
{
    [SerializeField]
    private float minFalloffDist;
    [SerializeField]
    private float minDamage;
    [SerializeField]
    private float maxFalloffDist;
    [SerializeField]
    private float maxDamage;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag != "Player") return;

        Transform collidedPlayer = collision.transform;

        float playerDist = Vector2.Distance(collidedPlayer.position, transform.position);

        if (playerDist >= maxFalloffDist)
            collidedPlayer.GetComponent<Entity>().LoseHP(minDamage);

        else if (playerDist <= minFalloffDist)
            collidedPlayer.GetComponent<Entity>().LoseHP(maxDamage);

        else
            collidedPlayer.GetComponent<Entity>().LoseHP(Mathf.Lerp(maxDamage, minDamage, ( (playerDist - minFalloffDist) / (maxFalloffDist - minFalloffDist) ) ));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxFalloffDist);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minFalloffDist);
    }
}
