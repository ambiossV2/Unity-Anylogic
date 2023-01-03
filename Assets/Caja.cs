using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caja : MonoBehaviour
{
    bool taken;
    public GameObject takesObjects;


    void Start()
    {
        takesObjects = GameObject.FindWithTag("takes");
    }
    public void Detection()
    {
        if (!taken)
        {
            this.transform.position = takesObjects.transform.position;
            this.transform.parent = takesObjects.transform;
            taken = true;
        }
    }

    public void Drop()
    {
        this.transform.parent = null;
        //taken = false;
    }
    
}
