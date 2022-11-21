using Photon.Pun.Demo.Asteroids;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro.Examples;

public class PlayerScript : MonoBehaviourPunCallbacks
{
    private PhotonView photonview;

    [SerializeField]
    private bool startsright;


    private bool facingright;


    [SerializeField]
    private GameObject groundchecker;

    [SerializeField]
    private GameObject playerbullet;
    public GameObject pooledBulleta;

    [SerializeField]
    private string bulletId;

    [SerializeField]
    private GameObject directionface;
    [SerializeField]
    private GameObject bulletsource;

    [SerializeField]
    private float jumpheight = 10f;

    [SerializeField]
    private GameObject camsforplayer;

    private int shotlimit = 8;
    private int shotstaken = 0;

    private float shotdelayset = 0.8f;
    private float shotdelay = 0f;

    private float reload = 50f;
    private float reloadtimer = 50f;

    private bool attacking = false;

    private int jumpcharges = 1;



    private void OnEnable()
    {
        base.OnEnable();

        if (!photonView.IsMine)
            return;

        camsforplayer.gameObject.SetActive(true);
    }

    void Start()
    {

        if (!photonView.IsMine)
            return;



        facingright = startsright;

        reload = reloadtimer;

    }

    // Update is called once per frame
    void Update()
    {
        photonView.RPC("Shoot", RpcTarget.All);
        if (!photonView.IsMine)
            return;

        if (attacking == false)
        {

            Facing();
        }


        
        Shoot();
        Jump();
    }

    private void LateUpdate()
    {
        
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        Movement();
    }

    void Movement()
    {




        //left and right movement
        if(groundchecker.GetComponent<GroundChecker>().onground == true)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), 0) *140f, ForceMode2D.Force);
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), 0) *140f, ForceMode2D.Force);
        }
        

        //fake friction generator
        if (groundchecker.GetComponent<GroundChecker>().onground == true)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                GetComponent<Rigidbody2D>().drag =7;
            }
            else
            {
                GetComponent<Rigidbody2D>().drag = 14;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().drag = 7;
        }






    }

    void Jump()
    {

        //jump

        if (groundchecker.GetComponent<GroundChecker>().onground == true && Input.GetButtonDown("Jump"))
        {

            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpheight * 100f, ForceMode2D.Force);
        }


        //jump cancel

        if (Input.GetButton("Jump") != true && GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.down * GetComponent<Rigidbody2D>().velocity.y * 6f, ForceMode2D.Force);
        }


        //another one

        if (GetComponent<Rigidbody2D>().velocity.y < 0 && groundchecker.GetComponent<GroundChecker>().onground == false)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.down * 80f / 6f, ForceMode2D.Force);
        }

        if (groundchecker.GetComponent<GroundChecker>().onground  == false && Input.GetButtonDown("Jump") && jumpcharges == 1)
        {
            jumpcharges -= 1;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpheight * 100f, ForceMode2D.Force);
        }

        if (groundchecker.GetComponent<GroundChecker>().onground)
        {
            jumpcharges = 1;
        }
    }


    void Facing()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            facingright = true;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            facingright = false;
        }

        if (facingright == true)
        {
            directionface.GetComponent<SpriteRenderer>().flipX = true;
            bulletsource.transform.localPosition = new Vector2(0.855f, 0);
        }
        else
        {

            directionface.GetComponent<SpriteRenderer>().flipX =  false;
            bulletsource.transform.localPosition = new Vector2(-0.855f, 0);
        }
    }


    [PunRPC]
    void Shoot()
    {

        if(Input.GetButton("Fire1") && /*shotstaken < shotlimit &&*/ shotdelay <= 0)
        {
            GameObject pooledBullet = ObjectPoolManager.Instance.GetPooledObject(bulletId);
            if (pooledBullet != null)
            {
                //Modify the bullet's position and rotation
                pooledBullet.transform.position = bulletsource.transform.position;
                pooledBullet.transform.rotation = bulletsource.transform.rotation;
                if (facingright == true)
                {

                    pooledBullet.GetComponent<PlayerShotScript>().startright = true;
                }
                else
                {
                    pooledBullet.GetComponent<PlayerShotScript>().startright = false;
                }

                //Enable the gameObject
                //pooledBullet.GetComponent<PlayerShotScript>().speedadd = this.GetComponent<Rigidbody2D>().velocity.x;

                pooledBullet.GetComponent<PlayerShotScript>().playershooting = this.gameObject;
                pooledBullet.SetActive(true);

                this.photonView.RPC("Shoot_RPC", RpcTarget.Others);

                shotdelay = shotdelayset;
                //shotstaken += 1;
            }
        }




        if (shotdelay > 0)
        {
            shotdelay -= 10f * Time.deltaTime;
        }




        //print(this.GetComponent<Rigidbody2D>().velocity.x);

        //if (shotstaken >= shotlimit)
        //{
        //    reload -= 50f * Time.deltaTime;
        //}

        //if(reload <= 0)
        //{
        //    shotstaken = 0;
        //    reload = reloadtimer;
        //}
    }

    private void Shoot_RPC()
    {
        pooledBulleta.SetActive(true);
    }


    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        // We own this player: send the others our data
    //        stream.SendNext(attacking);
    //    }
    //    else
    //    {
    //        // Network player, receive data
    //        attacking = (bool)stream.ReceiveNext();
    //    }
    //}

}
