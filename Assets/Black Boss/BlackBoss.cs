using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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
    private float distanceToTarget;

    [SerializeField]
    private List<GameObject> playerList; //To be replaced with Photon player list when implemented
    [SerializeField]
    private GameObject blackBossAura;
    private Entity entity;
    [SerializeField]
    private Image pullTimer;
    [SerializeField]
    private Image pullTimerBG;
    [SerializeField]
    private Animator pullAnimator;
    [SerializeField]
    private SpriteRenderer blackBossSprite;

    [SerializeField]
    private Sprite phaseOneSprite, phaseTwoSprite, phaseThreeSprite, invulnSprite;


    [SerializeField]
    private LayerMask jumplayermask;


    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
        PhaseChange();
        pullIntervalTimer = pullInterval;
    }

    // Update is called once per frame
    void Update()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player").ToList();

        HandleVulnerability();
        HandlePlayerPulling();
        MoveToMarkedTarget();

        if (markedTarget != null)
        {
            distanceToTarget = Vector2.Distance(transform.position, markedTarget.position);

            if (canAttack && distanceToTarget <= meleeAtkTriggerDist)
            {
                Debug.Log("Attacking marked target");
                AttackMarkedTarget();
            }
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
                blackBossSprite.sprite = invulnSprite;
            }
            else if (!entity.isVulnerable)
            {
                entity.isVulnerable = true;
                vulnIntervalTimer = vulnInterval;
                blackBossSprite.sprite = phaseOneSprite;
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

            pullAnimator.Play("PullPlayers");

            pullIntervalTimer = pullInterval;
        }

        pullTimer.fillAmount = pullIntervalTimer / pullInterval;
    }

    public void MarkNewTarget()
    {
        GameObject newTarget = playerList.OrderByDescending(p => Vector2.Distance(transform.position, p.transform.position)).ToList()[0];

        markedTarget = newTarget.transform;
    }

    private void MoveToMarkedTarget()
    {
        if (markedTarget == null || !canMove || isAttacking) return;
        JumpCheck();
        float jump;

        if (JumpCheck() == true)
        {
            jump = 2.5f;
        }
        else
        {
            jump = 0;
        }


        Vector2 movement = movementSpeed * Time.deltaTime * (markedTarget.position - transform.position).normalized;
        transform.position += new Vector3(movement.x, jump, 0) ;
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
                blackBossSprite.sprite = phaseOneSprite;
                canAttack = false;
                canMove = false;
                canPull = true;
                pullTimer.enabled = true;
                pullTimerBG.enabled = true;
                break;

            case 2:
                MarkNewTarget();
                blackBossAura.SetActive(false);
                canInvuln = false;
                entity.isVulnerable = true;
                blackBossSprite.sprite = phaseTwoSprite;
                canAttack = true;
                canMove = true;
                canPull = false;
                pullTimer.enabled = false;
                pullTimerBG.enabled = false;
                break;

            case 3:
                MarkNewTarget();
                entity.damageMultiplier = 0.75f;
                blackBossAura.SetActive(true);
                canInvuln = false;
                blackBossSprite.sprite = phaseThreeSprite;
                canAttack = true;
                canMove = true;
                canPull = false;
                pullTimer.enabled = false;
                pullTimerBG.enabled = false;
                break;
        }
    }

    public void ToggleOnAura()
    {
        if (phase == 3)
            blackBossAura.SetActive(true);
    }

    public void ToggleOffAura()
    {
        if (phase == 3)
            blackBossAura.SetActive(false);
    }


    private bool JumpCheck()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, jumplayermask);

        Debug.DrawRay(transform.position, transform.up * 1000f, Color.green);
        if (hit1)
        {

            if (hit1.collider.gameObject.tag ==)
            {

                return true;

            }


        }
        else
        {
            
        }
        print("notwow");
        return false;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, meleeAtkTriggerDist);
    }
}
