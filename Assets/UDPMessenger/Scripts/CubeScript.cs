using UnityEngine;
using System.Collections;
using JsonSplitter;
using System.Collections.Generic;

public class CubeScript : MonoBehaviour
{

    public bool Sending;
    public static string LastData;
    private void Update()
    {
        if (Sending)
            gameObject.GetComponent<UDPClient>().SendValue("ObjValue:" + JsonUtility.ToJson(transform.position));
    }


    void GetMessage(string mssg)
    {

        // if(mssg.Contains("ObjValue:")) transform.position = JsonUtility.FromJson<Vector3>(mssg.Replace("ObjValue:", ""));

        if (mssg.Contains("data") && mssg.Contains("[{"))
        {
            bool GenerateFiles = mssg.Contains("UnityGen");

          //  print("JsonRecibido");
            LastData = mssg;


            FindObjectOfType<Form1>().Generate(mssg);

            return;
            CheckNotLive();

        }

    }



    void CheckNotLive()
    {
        foreach (var item in UDPClient.instance.items)
        {

            char comma = '"';

            //Debug.LogError("DATA: " + LastData);

            if (!CubeScript.LastData.Contains(comma + "id" + comma + ":" + item.NetID))
            {

                GameObject oobj = item.Object;


                RemoveFromJsonID(FindObjectOfType<Form1>().readerAgente, item.NetID);
                RemoveFromJsonID(FindObjectOfType<Form1>().readerCajas, item.NetID);
                RemoveFromJsonID(FindObjectOfType<Form1>().readerTruck, item.NetID);
                RemoveFromJsonID(FindObjectOfType<Form1>().readerPallet, item.NetID);
                RemoveFromJsonID(FindObjectOfType<Form1>().readerForklift, item.NetID);
                RemoveFromJsonID(FindObjectOfType<Form1>().readerMaquinas, item.NetID);


                UDPClient.instance.items.Remove(item);
                DestroyImmediate(oobj);


            }
        }
    }

    void RemoveFromJsonID(JSONReader reader, int IDRemove)
    {

        List<JSONReader.Agente> agentestoremove = new List<JSONReader.Agente>();

        foreach (var ag in reader.agentes)
        {
            if (ag.id == IDRemove) agentestoremove.Add(ag);
        }

        foreach (var ag in agentestoremove)
        {
            if (ag.id == IDRemove) reader.agentes.Remove(ag);
        }


    }


}
