using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBossMissile : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    AudioClip explosionSFX;

    public Transform targetPlayer;
    private Vector3 targetDir;
    private Quaternion targetRot;

    [SerializeField]
    private float raycastDistance;
    [SerializeField]
    private float avoidanceForce;

    [SerializeField]
    private LayerMask avoidlayermask;

    [SerializeField]
    private GameObject explosion;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z);



        if(this.GetComponent<Entity>().returnHP() <= 0)
        {
            Die();
        }



        MoveTowardsTarget();

        if (targetPlayer == null)
        {
            return;
        }
        RotateTowardsTarget();        
    }

    private void RotateTowardsTarget()
    {
        targetDir = ApplyAvoidance();
        targetRot = Quaternion.FromToRotation(Vector3.up, new Vector2(targetDir.x, targetDir.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    private void MoveTowardsTarget()
    {
        transform.position += transform.up * movementSpeed * Time.deltaTime;
    }

    private Vector3 ApplyAvoidance()
    {
        Vector3 resultDir;
        resultDir = targetPlayer.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, resultDir, avoidlayermask);

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.up, 2.3f, avoidlayermask);

        var ray = new Ray2D(transform.position, resultDir);

        var ray2 = new Ray2D(this.transform.position, transform.up * 2.3f);

        if (hit1)
        {

            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Terrain" || LayerMask.LayerToName(hit.transform.gameObject.layer) == "Null")
            {
                Quaternion deflectRotation = Quaternion.FromToRotation(-ray2.direction, hit.normal);
                Vector2 MirrorPoint = deflectRotation * hit.normal * 1f;


                resultDir = MirrorPoint;
                Debug.DrawRay(hit.point, MirrorPoint, Color.magenta);

            }
            
        }



        return resultDir.normalized;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") //Make it check for player tag instead when a tag for player is implemented
        {
            Die();
            //Insert method to damage player here
        }

        if (collision.gameObject.layer == 9)
        {
            Die();

            
            print("el dibalo");
        }
    }

    private void Die()
    {
        this.GetComponent<Entity>().SetHP(5f);

        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC("diesinnetworkalso", RpcTarget.All);
    }

    [PunRPC]
    void diesinnetworkalso()
    {

        Instantiate(explosion, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }

    public void SetTarget(Transform target)
    {
        targetPlayer = target;
    }
}
