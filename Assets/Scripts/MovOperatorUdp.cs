using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class MovOperatorUdp : MonoBehaviour
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

        if (item.agent.coordenadas.Count < 7) transform.position = item.Position;
        else transform.position = Vector3.Lerp(transform.position, item.Position, t * Time.deltaTime);

        transform.LookAt(new Vector3(item.Target.x, transform.position.y, item.Target.z));

        //   transform.LookAt(new Vector3(item.Position.x,transform.position.y, item.Position.z));
        //transform.localScale = new Vector3(anchuras[indexSiguiente-1], 1, alturas[indexSiguiente-1]);
        //Debug.Log("SE ESTA POR LEER LOS BOOLEANOS DE MOVIMIENTO");


        bool isWalking = anim.GetBool("isWalking");
        bool isRunning = anim.GetBool("isRunning");
        bool HoldBox = anim.GetBool("HoldBox");



        if (item.agent.velocidades[0] > 0 && item.agent.velocidades[0] < 0.5)
        {
            anim.SetBool("isWalking", true);
            Debug.Log("ENTRA AL PRIMER IF");


        }
        else if (item.agent.velocidades[0] == 0)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            Debug.Log("ENTRA AL SEGUNDO IF");
        }
        else if (item.agent.velocidades[0] >= 0.5)
        {
            anim.SetBool("isRunning", true);
            Debug.Log("ENTRA AL TERCER IF");
        }

    }
    

    /*
    public void RecibirDatos(JSONReader.Agente ag)
    {
        //todas las listas deberian tener la misma cantidad de componentes
        for (int i = 0; i < ag.coordenadas.Count; i++)
        {
            velocidades.Add(ag.velocidades[i]);
            Debug.Log(ag.velocidades[i]);

        }
    }
    */
}