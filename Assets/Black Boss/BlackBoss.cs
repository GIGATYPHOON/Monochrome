using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlackBoss : MonoBehaviour
{
    [SerializeField]
    private int phase = 1;

    [SerializeField]
    private bool canInvuln = true;
    [SerializeField]
    private float invulnInterval = 3.0f;
    [SerializeField]
    private float vulnInterval = 4.0f;
    private float vulnIntervalTimer = 0.0f;

    [SerializeField]
    private bool canPull = true;
    [SerializeField]
    private float pullForce;
    [SerializeField]
    private float pullInterval = 5.0f;
    private float pullIntervalTimer = 0.0f;

    [SerializeField]
    private bool canMove = true;
    [SerializeField]
    private Transform markedTarget;
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private bool canAttack = true;
    [SerializeField]
    private bool isAttacking = false;
    [SerializeField]
    private float meleeAtkTriggerDist;
    public float distanceToTarget;

    [SerializeField]
    private List<GameObject> playerList; //To be replaced with Photon player list when implemented
    [SerializeField]
    private GameObject blackBossAura;
    private Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
        PhaseChange();
    }

    // Update is called once per frame
    void Update()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player").ToList();

        HandleVulnerability();
        HandlePlayerPulling();
        MarkNewTarget();
        MoveToMarkedTarget();

        distanceToTarget = Vector2.Distance(transform.position, markedTarget.position);
        if (markedTarget != null && canAttack && distanceToTarget <= meleeAtkTriggerDist)
        {
            Debug.Log("Attacking marked target");
            AttackMarkedTarget();
        }
    }

    private void HandleVulnerability()
    {
        if (!canInvuln || isAttacking) return;

        if (vulnIntervalTimer > 0)
            vulnIntervalTimer -= Time.deltaTime;

        else
        {
            if (entity.isVulnerable)
            {
                entity.isVulnerable = false;
                vulnIntervalTimer = invulnInterval;
            }
            else if (!entity.isVulnerable)
            {
                entity.isVulnerable = true;
                vulnIntervalTimer = vulnInterval;
            }
        }
    }

    private void HandlePlayerPulling()
    {
        if (!canPull || isAttacking) return;

        if (pullIntervalTimer > 0)
            pullIntervalTimer -= Time.deltaTime;

        else
        {
            foreach (GameObject player in playerList)
            {
                player.GetComponent<Rigidbody2D>().AddForce((transform.position - player.transform.position).normalized * pullForce, ForceMode2D.Impulse);
            }

            pullIntervalTimer = pullInterval;
        }
    }

    public void MarkNewTarget()
    {
        GameObject newTarget = playerList.OrderByDescending(p => Vector2.Distance(transform.position, p.transform.position)).ToList()[0];

        markedTarget = newTarget.transform;
    }

    private void MoveToMarkedTarget()
    {
        if (markedTarget == null || !canMove || isAttacking) return;

        Vector2 movement = movementSpeed * Time.deltaTime * (markedTarget.position - transform.position).normalized;
        transform.position += new Vector3(movement.x, 0, 0);
    }

    public void TeleportToMarkedTarget()
    {
        if (markedTarget == null) return;

        transform.position = markedTarget.position;
    }

    private void AttackMarkedTarget()
    {
        GetComponent<Animator>().Play("Attack1");
    }

    public void PhaseCheck()
    {
        float currentHPPercent = entity.returnHP() / entity.returnMaxHP();

        if (phase != 1 && currentHPPercent <= 1.0f && currentHPPercent > 0.8f)
        {
            phase = 1;
            PhaseChange();
        }            

        else if (phase != 2 && currentHPPercent <= 0.8f && currentHPPercent > 0.35f)
        {
            phase = 2;
            PhaseChange();
        }
          
        else if (phase != 3 && currentHPPercent <= 0.35f)
        {
            phase = 3;
            PhaseChange();
        }
    }

    private void PhaseChange()
    {
        switch (phase)
        {
            case 1:
                blackBossAura.SetActive(true);
                canInvuln = true;
                canAttack = false;
                canMove = false;
                canPull = true;
                break;

            case 2:
                MarkNewTarget();
                blackBossAura.SetActive(false);
                canInvuln = false;
                entity.isVulnerable = true;
                canAttack = true;
                canMove = true;
                canPull = false;
                break;

            case 3:
                blackBossAura.SetActive(true);
                canInvuln = false;
                canAttack = true;
                canMove = true;
                canPull = false;
                break;
        }
    }

    public void ToggleAura()
    {
        if (phase == 3)
            blackBossAura.SetActive(!blackBossAura.activeInHierarchy);
    }






    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, meleeAtkTriggerDist);
    }
}
