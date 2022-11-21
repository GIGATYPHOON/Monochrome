using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private float HP = 100;
    [SerializeField]
    private float MaxHP = 100;

    [SerializeField]
    private GameObject hpbar;

    public UnityEvent onHit;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hpbar != null)
            hpbar.GetComponentInChildren<Image>().fillAmount = HP / MaxHP;
    }

    public void LoseHP(float HPtoLose)
    {
        HP -= HPtoLose;
        onHit.Invoke();
    }

    public float returnHP()
    {
        return HP;
    }
}
