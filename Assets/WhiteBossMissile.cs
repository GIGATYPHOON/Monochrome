using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBossMissile : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float rotationSpeed;

    public Transform targetPlayer;
    private Vector3 targetDir;
    private Quaternion targetRot;

    [SerializeField]
    private float raycastDistance;
    [SerializeField]
    private float avoidanceForce;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, resultDir, LayerMask.NameToLayer("Terrain"));

        var ray = new Ray2D(transform.position, resultDir);

        if (hit)
        {
            Debug.Log("Raycast hit " + hit.transform.name);

            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Terrain")
            {
                Quaternion deflectRotation = Quaternion.FromToRotation(-ray.direction, hit.normal);
                Vector2 MirrorPoint = deflectRotation * hit.normal * 1f;
                Debug.DrawRay(hit.point, MirrorPoint, Color.magenta);
                resultDir = MirrorPoint + hit.point;
            }
        }

        
        return resultDir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) //Make it check for player tag instead when a tag for player is implemented
        {
            Die();
            //Insert method to damage player here
        }
    }

    private void Die()
    {
        Destroy(this.gameObject); //Make this return to pool instead of destroying
    }

    public void SetTarget(Transform target)
    {
        targetPlayer = target;
    }
}
