using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WhiteBossAvoider : MonoBehaviour
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

    [SerializeField]
    private LayerMask avoidlayermask;

    [SerializeField]
    private bool BacktoCenter = false;

    private void OnEnable()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z);

        //this.transform.position = Vector3.ClampMagnitude(transform.position, 8f);


        vinna();

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

        if(BacktoCenter == false)
        {
            resultDir = (2 * this.transform.position) - targetPlayer.transform.position;
        }
        else
        {
  
             resultDir = Vector3.zero - this.transform.position;

        }



        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.up, 2.3f, avoidlayermask);

        var ray2 = new Ray2D(this.transform.position, transform.up * 2.3f);

        Debug.DrawRay(this.transform.position, transform.up * 2.3f, Color.magenta);

        if (hit1)
        {

            if (LayerMask.LayerToName(hit1.transform.gameObject.layer) == "Terrain" || LayerMask.LayerToName(hit1.transform.gameObject.layer) == "Null")
            {
                Quaternion deflectRotation = Quaternion.FromToRotation(-ray2.direction, hit1.normal);
                Vector2 MirrorPoint = deflectRotation * hit1.normal * 1f;


                resultDir = MirrorPoint;
                Debug.DrawRay(hit1.point, resultDir, Color.blue);

                print(MirrorPoint);

            }

        }




            return resultDir.normalized;

   
        



    }





    public void SetTarget(Transform target)
    {
        targetPlayer = target;
    }



    private void vinna()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -14f, 14f), Mathf.Clamp(transform.position.y, -8f, 8f)) ;


        if(Mathf.Abs( transform.position.x) >= 13.9f || Mathf.Abs(transform.position.y) >= 7.9f)
        {
            BacktoCenter = true;
        }
        
        if (BacktoCenter == true && Vector3.Distance( transform.position, Vector3.zero) < 5.5f)
        {
            BacktoCenter = false;
        }

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.right, 2.8f, 1 << LayerMask.NameToLayer("Null"));

        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, transform.right, -2.8f, 1 << LayerMask.NameToLayer("Null"));


        if (hit2 && hit3)
        {

            transform.rotation = Quaternion.FromToRotation(Vector3.up, -targetDir);
            print("wtf");
        }
    }







}