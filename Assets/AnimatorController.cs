using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    
    void Update()
    {
        if (Input.GetKey("e"))
        {
            anim.Play("PickUp");
        }
    }
}
