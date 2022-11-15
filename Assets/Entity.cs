using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private float HP = 100;
    [SerializeField]
    private float MaxHP = 100;

    [SerializeField]
    private GameObject hpbar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpbar.GetComponentInChildren<Image>().fillAmount = HP / MaxHP;
    }

    public void LoseHP(float HPtoLose)
    {
        HP -= HPtoLose;
    }
}
