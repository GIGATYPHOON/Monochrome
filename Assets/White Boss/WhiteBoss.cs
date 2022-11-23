using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class WhiteBoss : MonoBehaviourPunCallbacks
{
   
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private float ProximityCheck = 0.5f;
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
    private float bulletspread;

    [SerializeField]
    private int bulletcount;

    [SerializeField]
    private int bulletcountlimit;

    [SerializeField]
    private float barragetimer;
    [SerializeField]
    private float currentbarragetimer;


    [SerializeField]
    private bool canFireBullets;
    [SerializeField]
    private float bulletFireRate;
    [SerializeField]
    private GameObject bulletObj;
    #endregion



    public GameObject[] playerList; //To be removed when Photon player list is implemented

    [SerializeField]
    List<GameObject> playersinsight = new List<GameObject>();

    void Start()
    {
        
    }

    private void OnEnable()
    {
        base.OnEnable();


        if (!photonView.IsMine)
            return;

            InvokeRepeating("FireBouncingBullets", 0.001f, bulletFireRate);

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animator>().SetInteger("Phase", phase);

        playerList = GameObject.FindGameObjectsWithTag("Player");

        if (shield.GetComponent<Entity>().returnHP() < 0)
        {

            shield.SetActive(false); 
            invincible = false;
           
            phase = 2;
            
        }
        if (GetComponent<Entity>().returnHP()/100f < 0.4f)
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
                whiteauraobject.transform.localScale = new Vector3(17f, 17f, 1f);
                break;
            case 2:
                whiteavoidanceobject.gameObject.SetActive(true);
                whitelaserobject.gameObject.SetActive(false);
                whiteauraobject.gameObject.SetActive(false);
                canFireBullets = true;

                this.transform.position = whiteavoidanceobject.transform.position;

                GetComponent<SpriteRenderer>().sprite = phase2sprite;
                missileCooldown = 7f;

                break;
            case 3:

                //To-do: raycast to player position with distance to check if we want to run laser
                foreach (GameObject player in playerList)
                {

                    if (Vector3.Distance(player.transform.position, this.transform.position) < 7f)
                    {
                        whiteauraobject.SetActive(true);
                    }
                    else
                    {
                        whiteauraobject.SetActive(false);
                    }
                }
                
                GetComponent<SpriteRenderer>().sprite = phase3sprite;
                whiteauraobject.transform.localScale = new Vector3(9f, 9f, 1f);
                whiteavoidanceobject.gameObject.SetActive(true);
                this.transform.position = whiteavoidanceobject.transform.position;
                canFireBullets = true;
                missileCooldown = 5f;
                break;
        };
        
 
        TickMissleCooldown();  

        if(bulletcount>= bulletcountlimit)
        {
            currentbarragetimer -= Time.deltaTime;
        }

        if(currentbarragetimer <= 0f)
        {
            currentbarragetimer = barragetimer;
            bulletcount = 0;

        }

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



        float oldDistance = 0;

        foreach (GameObject player in playerList)
        {
            


            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - this.transform.position, Mathf.Infinity, bulletMask);
            if (hit)
            {
                float spread;
                spread = Random.Range(-bulletspread, bulletspread);

                if (hit.collider.gameObject.tag == "Player")
                {
                    if(!playersinsight.Contains( hit.collider.gameObject))
                    {
                        playersinsight.Add(hit.collider.gameObject);
                    }


                    playersinsight.OrderBy(x => Vector2.Distance(this.transform.position, x.transform.position)).ToList();



                    Debug.DrawRay(this.transform.position, hit.point - new Vector2(transform.position.x, transform.position.y), Color.green);



                    //Debug.Log(playersinsight[0].GetComponent<PhotonView>().Owner.GetPlayerNumber() + " is closest");



                    bulletspawnrotater.transform.rotation = Quaternion.FromToRotation(this.transform.up, hit.point - new Vector2(transform.position.x, transform.position.y));


                    GameObject bullet = ObjectPoolManager.Instance.GetPooledObject(bouncingbulletID);



                    if (bullet != null && bulletcount < bulletcountlimit)
                    {
                        Vector3 spreadplayerpos = new Vector2(playersinsight[0].transform.position.x + spread, playersinsight[0].transform.position.y + spread);


                        bullet.GetComponent<BouncingBullet>().SetDirection(spreadplayerpos - transform.position);
                        bullet.transform.position = bulletspawn.transform.position;
                        bullet.transform.rotation = bulletspawn.transform.rotation;
                        PhotonNetwork.Instantiate("BouncingBullet", bulletspawn.transform.position, bulletspawn.transform.rotation);
                        bulletcount += 1;

                    }

                    //print(playersinsight[0].GetComponent<PhotonView>().Owner.GetPlayerNumber());


                    break;
                }
                else
                {
                    playersinsight.Clear();
                }


            }

        }        
    }



    [PunRPC]
    void BouncingShotgun(Vector3 played, Quaternion rotationw, float spread, Vector3 bullspawn, Quaternion bullrot)
    {

        bulletspawnrotater.transform.rotation = rotationw;


        GameObject bullet = ObjectPoolManager.Instance.GetPooledObject(bouncingbulletID);



        if (bullet != null && bulletcount < bulletcountlimit)
        {
            Vector3 spreadplayerpos = new Vector2(played.x + spread, played.y + spread);


            bullet.GetComponent<BouncingBullet>().SetDirection(spreadplayerpos - transform.position);
            bullet.transform.position = bullspawn;
            bullet.transform.rotation = bullrot;
            //bullet.SetActive(true);
            PhotonNetwork.Instantiate("BouncingBullet", bullspawn, bullrot);
            bulletcount += 1;

        }
    }

    //void BouncingShotgun2()
    //{

    //    bulletspawnrotater.transform.rotation = rotationw;


    //    GameObject bullet = ObjectPoolManager.Instance.GetPooledObject(bouncingbulletID);



    //    if (bullet != null && bulletcount < bulletcountlimit)
    //    {
    //        Vector3 spreadplayerpos = new Vector2(played.x + spread, played.y + spread);


    //        bullet.GetComponent<BouncingBullet>().SetDirection(spreadplayerpos - transform.position);
    //        bullet.transform.position = bulletspawn.transform.position;
    //        bullet.transform.rotation = bulletspawn.transform.rotation;
    //        bullet.SetActive(true);
    //        bulletcount += 1;

    //    }
    //}




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

        foreach (GameObject player in playerList)
        {
            GameObject missile = PhotonNetwork.Instantiate("Homer", transform.position, Quaternion.identity); //To be replaced with PhotonNetwork.Instantiate
            missile.GetComponent<WhiteBossMissile>().SetTarget(player.transform);
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







//bulletspawnrotater.transform.rotation = Quaternion.FromToRotation(this.transform.up, hit.point - new Vector2(transform.position.x, transform.position.y));


//GameObject bullet = ObjectPoolManager.Instance.GetPooledObject(bouncingbulletID);



//if (bullet != null && bulletcount < bulletcountlimit)
//{
//    Vector3 spreadplayerpos = new Vector2(playersinsight[0].transform.position.x + spread, playersinsight[0].transform.position.y + spread);


//    bullet.GetComponent<BouncingBullet>().SetDirection(spreadplayerpos - transform.position);
//    bullet.transform.position = bulletspawn.transform.position;
//    bullet.transform.rotation = bulletspawn.transform.rotation;
//    bullet.SetActive(true);
//    bulletcount += 1;

//}