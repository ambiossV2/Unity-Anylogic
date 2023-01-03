using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JSONReader : MonoBehaviour
{
    public string archivoAgente;
    public GameObject agentePrefab;
 
    public bool ignore;

    [Header("Ascensor")]
    public float PosicionMaximaAlta;
    public bool CheckAscensor;
    public GameObject AscensorPrefab;
    
    //string filename = "";
    
    //PlayerInput playerInput;
    //CharacterController characterController;

    [System.Serializable]
    public class Agente
    {
        public int id;
        public List<Vector3> coordenadas = new List<Vector3>();
        public List<float> tiempos = new List<float>();
        public List<float> velocidades = new List<float>();
        public List<float> anchuras = new List<float>();
        public List<float> alturas = new List<float>();
    }

    public List<Agente> agentes = new List<Agente>();


    private void Start()
    {
        

        if (ignore) return;
        LeerArchivoAgentes(archivoAgente);
        InstanciarAgentes();
        //filename = Application.persistentDataPath + "/test.csv";

        //playerInput = new PlayerInput();
        //characterController = GetComponent<CharacterController>();

    }


    public void GetDataAndPlay()
    {
        LeerArchivoAgentes(archivoAgente);


        foreach (Agente ag in agentes)
        {
            if (UDPClient.instance.getObjWithID(ag.id) != null) UDPClient.instance.SetData(ag);

        }

        InstanciarAgentes();

    }

 

    private void LeerArchivoAgentes(string archivo)
    {
        //leemos el archivo JSON linea por linea

        if (!File.Exists(Application.persistentDataPath + "/" + archivo)) return;
        foreach (string line in File.ReadLines(Application.persistentDataPath + "/" + archivo))
        {
            if (line.CompareTo("[") != 0 && line.CompareTo("]") != 0) //filtro caracteres finales e iniciales
            {
                string aux = line.Substring(1, line.Length - 2);//retiramos las llaves { } inicial y final de cada linea

                string[] subs = aux.Split(','); //separamos en valores separados por la coma

                //por cada valor dejamos solo los datos
                for (int i = 0; i < subs.Length; i++)
                {
                    int posicion = subs[i].IndexOf(":");
                    subs[i] = subs[i].Substring(posicion + 1); //para obtener la cadena sin los :
                }

                //muestro los datos filtrados
                //tener en cuenta que Z de Anylogic es Y en Unity
               /* Debug.Log(
                    "z: " + subs[2] +
                    "tiempo: " + subs[1] +
                    "y: " + subs[0] +
                    "x: " + subs[3] +
                    "id: " + subs[4] +
                    "velocidad: " + subs[5] +
                    "ancho: " + subs[6] +
                    "alto: " + subs[7]
                    );
               */

                AgregarAgente(subs);
            

            }

        }

    }

     
    private void AgregarAgente(string[] subs)
    {
        int id = Convert.ToInt32(subs[4]);
        //coordenadas
        float x = float.Parse(subs[3], CultureInfo.InvariantCulture.NumberFormat);
        float y = float.Parse(subs[0], CultureInfo.InvariantCulture.NumberFormat);
        float z = float.Parse(subs[2], CultureInfo.InvariantCulture.NumberFormat);
        //valores para el objeto en esa coordenada
        float tiempo = float.Parse(subs[1], CultureInfo.InvariantCulture.NumberFormat);
        float velocidad = float.Parse(subs[5], CultureInfo.InvariantCulture.NumberFormat);
        float ancho = float.Parse(subs[6], CultureInfo.InvariantCulture.NumberFormat);
        float alto = float.Parse(subs[7], CultureInfo.InvariantCulture.NumberFormat);


        //Verificamos si el agente existe en la lista de agentes
        int indexAgente = -1; //agente no existe

        for (int i = 0; i < agentes.Count; i++)
        {
            if (agentes[i].id == id)
            {
                indexAgente = i; //agente existe
                break;
            }
        }

        //si el agente no existe en la lista lo agregamos
        if (indexAgente == -1)
        {
            Agente a = new Agente();
            a.id = id;
            a.coordenadas.Add(new Vector3(x, y, z)); //agrego la primer coordenada de ese objeto
            a.tiempos.Add(tiempo);
            a.velocidades.Add(velocidad);
            a.anchuras.Add(ancho);
            a.alturas.Add(alto);

            agentes.Add(a); //agrego la persona a la lista
            Debug.Log("el numero de velocidades dentro de la lista son: " + a.velocidades.Count);
         
        }
        //si el agente ya existe agrego a esa agente otra registro de coordenadas, velocidad y tiempo
        else
        {
            agentes[indexAgente].coordenadas.Add(new Vector3(x, y, z)); //agrego las demas coordenadas de ese objeto
            agentes[indexAgente].tiempos.Add(tiempo);
            agentes[indexAgente].velocidades.Add(velocidad);
            agentes[indexAgente].anchuras.Add(ancho);
            agentes[indexAgente].alturas.Add(alto);
        }
    }


    private void InstanciarAgentes()
    {
        foreach (Agente ag in agentes)
        {
            // no existe el objeto
            if(UDPClient.instance.getObjWithID(ag.id) == null)
            {
            //var obj3 =  Instantiate(agentePrefab, new Vector3(1, 0, 0), Quaternion.identity);
            var obj =  Instantiate(agentePrefab, new Vector3(1, 0, 0), Quaternion.identity); // instanciar objeto
               obj.transform.position = ag.coordenadas[0]; // aplicar posicion
               //obj3.transform.position = ag.coordenadas[0];
              //  obj.transform.localScale = new Vector3(ag.anchuras[0], ag.alturas[0], ag.anchuras[0]); // aplicar escala

                UDPClient.instance.SetData(ag);
                UDPClient.instance.items.Add(new RealTimeItem { Object = obj, NetID = ag.id, agent = ag, Position = ag.coordenadas[0], Target = ag.coordenadas[0] }); 
                obj.GetComponent<MovimientoUdp>().item = UDPClient.instance.items[UDPClient.instance.GetID(ag.id)];
                //obj3.GetComponent<MovOperatorUdp>().item = UDPClient.instance.items[UDPClient.instance.GetID(ag.id)];

                if (obj.transform.position.y < PosicionMaximaAlta)
                {
                    var obj2 = Instantiate(AscensorPrefab, obj.transform.position, Quaternion.identity); // instanciar objeto
                    obj2.GetComponent<Ascensor>().Setup(obj.gameObject);
                }
                obj.GetComponent<Movimiento>().RecibirDatos(ag);
                //obj3.GetComponent<Movimiento>().RecibirDatos(ag);
            }
        }
    }
    /*
    public void WriteCSV()
    {
        if (agentes.Count > 0)
        {
            TextWriter tw = new StreamWriter(filename, false); //usamos false porque la primera vez queremos asegurarnos de que no haya nada y sobreescribir
            tw.WriteLine("id, coordenadas, tiempos, velocidades, anchuras, alturas");
            tw.Close();

            tw = new StreamWriter(filename, true);
            for (int i = 0; i < agentes.Count; i++)
            {
                tw.WriteLine(agentes[i].id + "," + agentes[i].coordenadas + "," + agentes[i].velocidades + "," + agentes[i].anchuras + "," + agentes[i].alturas);
            }
            tw.Close();
        }
    }
    void Update()
    {
            WriteCSV();
    }
    */
}

