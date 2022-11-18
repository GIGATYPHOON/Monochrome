using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoss : MonoBehaviour
{
    [SerializeField]
    private GameObject shield;


    [SerializeField]
    bool laser = false;

    [SerializeField]
    LayerMask mylayermask;

    [SerializeField]
    private GameObject whitelaserobject;

    [SerializeField]
    private GameObject whiteauraobject;

    [SerializeField]
    private GameObject whitelasertarget;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shield.GetComponent<Entity>().returnHP() <= 0)
        {
            shield.SetActive(false);
        }



        if(laser == true)
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
    


    private void whitelaser()
    {






        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, whitelasertarget.transform.position - this.transform.position, Mathf.Infinity, mylayermask);

        if (hit1)
        {

            Debug.DrawRay(this.transform.position, hit1.point - new Vector2( transform.position.x, transform.position.y), Color.green);


            whitelaserobject.gameObject.transform.position = (hit1.point + new Vector2(transform.position.x, transform.position.y) ) / 2;
            whitelaserobject.gameObject.transform.rotation = Quaternion.FromToRotation(this.transform.up, hit1.point - new Vector2(transform.position.x, transform.position.y));
            whitelaserobject.gameObject.transform.localScale = new Vector2( whitelaserobject.gameObject.transform.localScale.x, hit1.distance);

            print(hit1.point);

        }




    }

}
