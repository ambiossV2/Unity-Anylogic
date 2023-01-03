using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class MovimientoUdp : MonoBehaviour
{
    public float t;
    public Vector3 startPosition;

    public float timeToReachTarget = 1;
    public GameObject enemy;

    public RealTimeItem item;


    Animator anim;
    List<Vector3> posiciones = new List<Vector3>();
    List<float> velocidades = new List<float>();
    //JSONReader jsonreader;
    //int j = 0;

    void Start()
    {
        //jsonreader = GetComponent<JSONReader>();
        anim = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {

        if (item == null) return;

        if (item.agent.coordenadas.Count < 9) transform.position = item.Position;
        else transform.position = Vector3.Lerp(transform.position, item.Position, t * Time.deltaTime);

        transform.LookAt(new Vector3(item.Target.x, transform.position.y, item.Target.z));

        //   transform.LookAt(new Vector3(item.Position.x,transform.position.y, item.Position.z));
        //transform.localScale = new Vector3(anchuras[indexSiguiente-1], 1, alturas[indexSiguiente-1]);

        bool isWalking = anim.GetBool("isWalking");
        bool isRunning = anim.GetBool("isRunning");
        bool HoldBox = anim.GetBool("HoldBox");



        if (item.agent.velocidades[0] > 0 && item.agent.velocidades[0] < 0.5)
        {
            anim.SetBool("isWalking", true);
            //Debug.Log("ENTRA AL PRIMER IF");


        }
        else if (item.agent.velocidades[0] == 0)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            //Debug.Log("ENTRA AL SEGUNDO IF");
        }
        else if (item.agent.velocidades[0] >= 0.5)
        {
            anim.SetBool("isRunning", true);
            //Debug.Log("ENTRA AL TERCER IF");
        }

    }
}