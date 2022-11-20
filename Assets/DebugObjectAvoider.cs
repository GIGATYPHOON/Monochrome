using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class DebugObjectAvoider : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 20.0f;
    [SerializeField]
    private float rotationSpeed = 5.0f;
    [SerializeField]
    private float force = 50.0f;
    [SerializeField]
    private float minimumAvoidanceDistance = 20.0f;
    [SerializeField]
    private float toleranceRadius = 3.0f;

    private float currentSpeed;
    private Vector2 targetPoint;
    private Vector2 avoidtargetPoint;
    private Vector2 direction;
    private Quaternion targetRotation;

    private bool somethingintheway = false;

    private float octopusavoid = 0.6f;


    // Use this for initialization
    void Start()
    {
        targetPoint = Vector3.zero;
    }


    // Update is called once per frame
    private void Update()
    {
        //if(Vector2.Distance(this.transform.position, GameObject.Find("Player").transform.position) < 10f)
        //{
        //    targetPoint = GameObject.Find("Player").transform.position;
        //}
        //else
        //{
        //    targetPoint = Vector2.zero;
        //}

        targetPoint = (2*this.transform.position) - GameObject.Find("Player").transform.position;

        //targetPoint = new Vector2( Mathf.Clamp(targetPoint.x, -14f,14f), Mathf.Clamp(targetPoint.y, -6f, 10f));

        targetPoint = Vector2.ClampMagnitude(targetPoint, 12f);

        print(targetPoint);

        MoveAgent();
    }

    /// <summary>
    /// This is the function that will handle the movement of the vehicle
    /// No need to change anything in this function, unless you need to
    /// </summary>
    private void MoveAgent()
    {
        //Calculate the directional vector towards the target point
        if (somethingintheway == false)
        {
            direction = targetPoint - new Vector2( transform.position.x, transform.position.y);
        }
        else
        {
            direction = avoidtargetPoint - new Vector2(transform.position.x, transform.position.y);
        }

        //Normalize the directional vector so that it's magniture is no more than 1.0
        direction.Normalize();

        //Modify the direction by applying obstacle avoidance

        ApplyAvoidance(ref direction);
        ApplyAvoidance2(ref direction);

        vinna();

        if (Vector2.Distance(targetPoint, transform.position) < toleranceRadius)
            return;

        currentSpeed = movementSpeed * Time.deltaTime;



        targetRotation = Quaternion.FromToRotation(Vector3.up, direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);

        transform.position += transform.up * currentSpeed;

    }

    /// <summary>
    /// This is the function that will handle the user's input
    /// No need to change anything in this function, unless you need to
    /// </summary>
    private void CheckInput()
    {
        //Check whether the user has clicked the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            //Cast a ray from the main camera to the position of the mouse
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            //If the ray hits something, assign the hit point as the target point
            if (Physics.Raycast(ray, out mouseHit, 100.0f))
            {
                targetPoint = mouseHit.point;
            }
        }
    }

    private void ApplyAvoidance(ref Vector2 direction)
    {

        //TODO
        //1. Cast a raycast that will only hit the Obstacle Layer
        //2. The raycast's origin will start at the position of the player towards its forward vector
        //3. When the raycast hits an obstacle, get the normal of the surface hit and calculate the new direction vector


        Debug.DrawRay(transform.position, transform.up * 2.3f, Color.green);


        Debug.DrawRay(transform.position, transform.right * 0.4f, Color.red);

        Debug.DrawRay(transform.position, transform.right * -0.4f, Color.red);

        var ray = new Ray2D(this.transform.position, direction  );

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.up, 2.3f, 1 << LayerMask.NameToLayer("Phantom Hitbox"));

        //RaycastHit2D hit15 = Physics2D.Raycast(transform.position, transform.up + transform.right, 2.3f, -3);

        //RaycastHit2D hit16 = Physics2D.Raycast(transform.position, transform.up - transform.right, 2.3f, -3);

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.right, 0.8f, 1 << LayerMask.NameToLayer("Phantom Hitbox"));

        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, transform.right, -0.8f, 1 << LayerMask.NameToLayer("Phantom Hitbox"));

        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, transform.up, -1.3f, -3);
        /*
            * Cast a Raycast.
            * If it hits something:
            */
        if (hit1)
        {

            Quaternion deflectRotation = Quaternion.FromToRotation(-ray.direction, hit1.normal);
            Vector2 MirrorPoint = deflectRotation * hit1.normal * 1.3f;

            if (hit1.collider.gameObject)

            //Vector2 MirrorPoint = (hit.point + ((hit.point + hit.normal) - hit.point).normalized);

            //Debug.DrawRay(MirrorPoint, transform.up * 1f, Color.red);

            Debug.DrawRay(hit1.point,    MirrorPoint, Color.magenta, 0f);



            avoidtargetPoint = MirrorPoint + hit1.point;

            //print(hit1.point+ " then "+ MirrorPoint + " then " + targetPoint + " then " + somethingintheway + " then " + avoidtargetPoint);

            somethingintheway = true;

        }
        else
        {
            somethingintheway = false;
        }


        if (hit2 && hit3)
        {

            transform.rotation = Quaternion.FromToRotation(Vector3.up, -direction);

        }
        //else if(hit3)
        //{
        //    transform.position += transform.right * currentSpeed ;
        //}
        //else if (hit2)
        //{
        //    transform.position += transform.right * -currentSpeed ;
        //}

        //if(hit4)
        //{
        //    transform.position += transform.up * -currentSpeed * 3.5f;
        //}

    }




    private void ApplyAvoidance2(ref Vector2 direction)
    {

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.up, octopusavoid, 1 << LayerMask.NameToLayer("Phantom Hitbox"));

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.up, -octopusavoid, 1 << LayerMask.NameToLayer("Phantom Hitbox"));

        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, transform.right, octopusavoid*1.1f, 1 << LayerMask.NameToLayer("Phantom Hitbox"));

        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, transform.right, -octopusavoid * 1.1f, 1<< LayerMask.NameToLayer("Phantom Hitbox"));


        RaycastHit2D hit5 = Physics2D.Raycast(transform.position, transform.up + transform.right, octopusavoid, 1 << LayerMask.NameToLayer("Phantom Hitbox")); //top right

        RaycastHit2D hit6 = Physics2D.Raycast(transform.position, transform.up + transform.right, -octopusavoid, 1 << LayerMask.NameToLayer("Phantom Hitbox")); // down left

        RaycastHit2D hit7 = Physics2D.Raycast(transform.position, transform.right - transform.up , octopusavoid, 1 << LayerMask.NameToLayer("Phantom Hitbox")); //down right

        RaycastHit2D hit8 = Physics2D.Raycast(transform.position, transform.right - transform.up, -octopusavoid, 1 << LayerMask.NameToLayer("Phantom Hitbox")); // top left


        Debug.DrawRay(transform.position, transform.up * octopusavoid, Color.green);

        Debug.DrawRay(transform.position, transform.up * -octopusavoid, Color.green);

        Debug.DrawRay(transform.position, transform.right * octopusavoid * 1.1f, Color.green);

        Debug.DrawRay(transform.position, transform.right * -octopusavoid * 1.1f, Color.green);

        Debug.DrawRay(transform.position, (transform.up + transform.right) * octopusavoid, Color.green);

        Debug.DrawRay(transform.position, (transform.up + transform.right) * -octopusavoid, Color.green);

        Debug.DrawRay(transform.position, (transform.right -transform.up) * octopusavoid, Color.green);

        Debug.DrawRay(transform.position, (transform.right -transform.up) * -octopusavoid, Color.green);




        if (hit1)
        {

            transform.position += transform.up * -currentSpeed * 0.7f;

        }
        
        if (hit2)
        {
            transform.position += transform.up * currentSpeed * 0.7f;
        }
        
        
        if (hit3)
        {
            transform.position += transform.right * -currentSpeed * 0.7f;
        }

        if (hit4)
        {
            transform.position += transform.right * currentSpeed * 0.7f;
        }

        if (hit5)
        {
            transform.position += (transform.right + transform.up ) * -currentSpeed * 0.7f; //down left
        }

        if (hit6)
        {
            transform.position += (transform.right + transform.up) * currentSpeed * 0.7f; // top right
        }

        if (hit7)
        {
            transform.position += (transform.right - transform.up) * -currentSpeed * 0.7f; //top left
        }

        if (hit8)
        {
            transform.position += (transform.right - transform.up) * currentSpeed * 0.7f; // down right
        }

    }

    private void vinna()
    {
        Debug.DrawRay(transform.position, transform.up  * 1.3f, Color.yellow);


        RaycastHit2D hit15 = Physics2D.Raycast(transform.position, transform.up, 1.3f, 1 << LayerMask.NameToLayer("LookAtPlayer"));



        if (hit15)
        {


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, targetPoint - new Vector2(transform.position.x, transform.position.y)), rotationSpeed * Time.deltaTime);

        }
        else
        {

        }

    }
}