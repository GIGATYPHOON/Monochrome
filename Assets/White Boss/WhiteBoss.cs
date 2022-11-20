using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoss : MonoBehaviour
{
   
    [SerializeField]
    private GameObject shield;

    [SerializeField]
    private int phase = 1;
    #region spriteSerial
    [SerializeField]
    private Sprite phase1sprite;
    [SerializeField]
    private Sprite phase2sprite;
    [SerializeField]
    private Sprite phase3sprite;
    #endregion


    

    #region Lasers

    [SerializeField]
    bool laser = false;
    [SerializeField]
    LayerMask laserMask;
    [SerializeField]
    LayerMask bulletMask;

    [SerializeField]
    private GameObject whitelaserobject;

    [SerializeField]
    private GameObject whiteauraobject;

    [SerializeField]
    private GameObject whitelasertarget;

    #endregion
    public bool invincible = true;

    [SerializeField]
    private float missileCooldown;
    [SerializeField]
    private float currentMissileCooldown;
    [SerializeField]
    private GameObject missileObj;

   

    [SerializeField]
    private GameObject whiteavoidanceobject;
  

    #region bulletFields
    [SerializeField]
    private string bouncingbulletID;
    [SerializeField]
    private GameObject bulletspawnrotater;
    [SerializeField]
    private GameObject bulletspawn;

    
    [SerializeField]
    private bool canFireBullets;
    [SerializeField]
    private float bulletFireRate;
    [SerializeField]
    private GameObject bulletObj;
    #endregion

    public List<Transform> playerList; //To be removed when Photon player list is implemented

    void Start()
    {
        
    }

    private void OnEnable()
    {
        InvokeRepeating("FireBouncingBullets", 0.001f, bulletFireRate);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animator>().SetInteger("Phase", phase);


        if (shield.GetComponent<Entity>().returnHP() < 0)
        {
            shield.SetActive(false); 
            invincible = false;
           
            phase = 2;
            
        }
        if (GetComponent<Entity>().returnHP() < 51)
        {
           
            invincible = false;

            phase = 3;

        }


        switch (phase)

        {
            case 1:

                LaserCheck();
                canFireBullets = false;
                GetComponent<SpriteRenderer>().sprite = phase1sprite;
                whiteavoidanceobject.gameObject.transform.position = this.transform.position;
                whiteavoidanceobject.gameObject.SetActive(false);
                break;
            case 2:
                whiteavoidanceobject.gameObject.SetActive(true);
                whitelaserobject.gameObject.SetActive(false);
                whiteauraobject.gameObject.SetActive(false);
                canFireBullets = true;

                this.transform.position = whiteavoidanceobject.transform.position;

                GetComponent<SpriteRenderer>().sprite = phase2sprite;

                break;
            case 3:

                    //To-do: raycast to player position with distance to check if we want to run laser
                    LaserCheck();
                    

                  GetComponent<SpriteRenderer>().sprite = phase3sprite;

                whiteavoidanceobject.gameObject.SetActive(true);
                this.transform.position = whiteavoidanceobject.transform.position;
                canFireBullets = true;
                break;
        };
        
 
        TickMissleCooldown();      
    }



    private void whitelaser()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, whitelasertarget.transform.position - this.transform.position, Mathf.Infinity, laserMask);

        if (hit1)
        {

            Debug.DrawRay(this.transform.position, hit1.point - new Vector2(transform.position.x, transform.position.y), Color.green);


            whitelaserobject.gameObject.transform.position = (hit1.point + new Vector2(transform.position.x, transform.position.y)) / 2;
            whitelaserobject.gameObject.transform.rotation = Quaternion.FromToRotation(this.transform.up, hit1.point - new Vector2(transform.position.x, transform.position.y));
            whitelaserobject.gameObject.transform.localScale = new Vector2(0.75f, hit1.distance);



        }
    }

    private void FireBouncingBullets()
    {
        if (!canFireBullets)
            return;

        foreach (Transform player in playerList)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - this.transform.position, Mathf.Infinity, bulletMask);
            if (hit)
            {

                if(hit.collider.gameObject.tag == "Player")
                {


                    Debug.DrawRay(this.transform.position, hit.point - new Vector2(transform.position.x, transform.position.y), Color.green);
                    bulletspawnrotater.transform.rotation = Quaternion.FromToRotation(this.transform.up, hit.point - new Vector2(transform.position.x, transform.position.y));

                    GameObject bullet = ObjectPoolManager.Instance.GetPooledObject(bouncingbulletID);


                    if(bullet != null)
                    {
                        bullet.GetComponent<BouncingBullet>().SetDirection(player.position - transform.position);
                        bullet.transform.position = bulletspawn.transform.position;
                        bullet.transform.rotation = bulletspawn.transform.rotation;
                        bullet.SetActive(true);
                    }

                    break;
                }

            }
        }        
    }


    private void TickMissleCooldown()
    {
        if (currentMissileCooldown > 0)
            currentMissileCooldown -= Time.deltaTime;
    }

    public void FireMissiles()
    {
        if (currentMissileCooldown > 0)
            return;

        currentMissileCooldown = missileCooldown;

        foreach (Transform player in playerList)
        {
            GameObject missile = GameObject.Instantiate(missileObj, transform.position, Quaternion.identity); //To be replaced with PhotonNetwork.Instantiate
            missile.GetComponent<WhiteBossMissile>().SetTarget(player);
        }
    }
    private void LaserCheck()
    {
       
        if (laser == true)
        {

            whitelaserobject.gameObject.SetActive(true);
            whiteauraobject.gameObject.SetActive(false);
            whitelaser();
        }
        else
        {

            whitelaserobject.gameObject.SetActive(false);
            whiteauraobject.gameObject.SetActive(true);
        }
      

    }
}
