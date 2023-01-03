using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float t;
    public Vector3 startPosition;
    public Vector3 target;
    
    float timeToReachTarget;
    //Animator anim;
    //JSONReader jsonreader;
    public bool iniciar = false;
    int indexSiguiente = 1; //el tercer elemento de la lista posiciones va a ser el segundo punto a recorrer
    //collider
    //public GameObject signObject;
    public GameObject enemy;
    //datos del agente
    List<Vector3> posiciones = new List<Vector3>();
    List<float> tiempos = new List<float>();
    List<float> velocidades = new List<float>();
    List<float> anchuras = new List<float>();
    List<float> alturas = new List<float>();



    void Start()
    {
        //startPosition = target = transform.position;
        //jsonreader = GetComponent<JSONReader>();
        //anim = GetComponent<Animator>();
    }
    void Update()
    {


        if (iniciar)
        {
            
            //Debug.Log("SE ESTÁ LEYENDO EL UPDATE DE MOVIMIENTO INICIAR");

            t += Time.deltaTime / timeToReachTarget; //valor entre 0 y 1, 0 equivale a 0% del recorrido entre origen y destino, y 1 el 100% del camino recorrido.
            transform.position = Vector3.Lerp(startPosition, target, t);
            transform.LookAt(new Vector3(target.x,transform.position.y,target.z));
            //transform.localScale = new Vector3(anchuras[indexSiguiente-1], 1, alturas[indexSiguiente-1]);


            if (t >= 1)
            {
                //Debug.Log("SE ESTÁ LEYENDO T>=1");
                iniciar = false; //permite que no se actualize la funcion LERP
                indexSiguiente++;
                


                if (indexSiguiente < posiciones.Count) //solo si existe un proximo destino podemos animar el objeto
                {
                    SetDestination(posiciones[indexSiguiente], tiempos[indexSiguiente] - tiempos[indexSiguiente - 1]);
                    iniciar = true;
                    
                }

            }

            //Debug.Log("SE ESTÁ TERMIANNDO DE LEER EL UPDATE DE MOVIMIENTO INICIAR");
        }
        
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Object")
        {
            anim.SetBool("HoldBox", true);
            enemy.SetActive(true);
        }
    }
    private void onTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            anim.SetBool("HoldBox", false);
        }
    }
    */

    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        target = destination;
    }

    public void RecibirDatos(JSONReader.Agente ag)
    {
        //todas las listas deberian tener la misma cantidad de componentes
        for (int i=0; i < ag.coordenadas.Count; i++)
        {
            //Debug.Log("///RECIBIENDO DATOS///");
            posiciones.Add(ag.coordenadas[i]);
            tiempos.Add(ag.tiempos[i]);
            velocidades.Add(ag.velocidades[i]);
            anchuras.Add(ag.anchuras[i]);
            alturas.Add(ag.alturas[i]);
        }

        transform.position = posiciones[0];
        if (posiciones.Count > 1)
        {
            SetDestination(posiciones[1], tiempos[0]);
            iniciar = true;
        }
        else
        {
            SetDestination(posiciones[0], tiempos[0]);
        }

        
    }
}
