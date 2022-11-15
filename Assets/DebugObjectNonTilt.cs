using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugObjectNonTilt : MonoBehaviour
{
    public GameObject thing;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = thing.transform.position;
        transform.rotation =Quaternion.Euler (Vector3.zero);
    }


}
