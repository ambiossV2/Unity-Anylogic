using UnityEngine;
using System.Collections;
     
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using static JSONReader;

[Serializable]
public class RealTimeItem
{
	public GameObject Object;
	public int NetID;
	public Vector3 Position;
	public Vector3 Target;
	public float TimeReach;
	public float TimeReachNext;
	public bool change;

	public Agente agent;
}
public class UDPClient : MonoBehaviour
{
	public List<RealTimeItem> items;


	public static UDPClient instance;
    public int portListen = 4444;
	public string ipSend = "";
	public int portSend = 5555;

	public GameObject[]  notifyObjects;
	public string messageToNotify;

	private string received = "";
	
	private UdpClient client;
	private Thread receiveThread;
	private IPEndPoint remoteEndPoint;
	private IPAddress ipAddressSend;

	public float TimeStamp = 1;

	public bool SetData(Agente ag)
	{
		if (getObjWithID(ag.id) == null) return false;
		RealTimeItem itemUse = items[GetID(ag.id)];
		int id = Convert.ToInt32(ag.id);

		GameObject obj = getObjWithID(id);

	    /*
		if (ag.coordenadas.Count > (ag.coordenadas.Count - 2))
		{ 
			itemUse.Position = ag.coordenadas[ag.coordenadas.Count - 2]; 
		}
		*/

		itemUse.Position = ag.coordenadas[ag.coordenadas.Count - 1];
		itemUse.Target = ag.coordenadas[ag.coordenadas.Count - 1];

        itemUse.TimeReach = ag.tiempos[ag.tiempos.Count - 1];
        itemUse.TimeReachNext = ag.tiempos[ag.tiempos.Count - 1];
        itemUse.change = true;


	//	Debug.LogError("Checking: " + itemUse.NetID + "|||AGID: " + ag.id);
        //  obj.transform.position = itemUse.Position;
        return true;
		
    }

	private void FixedUpdate()
	{
		foreach(var xItem in items)
		{
		//	xItem.Object.transform.position = Vector3.MoveTowards(xItem.Object.transform.position, xItem.Position, TimeStamp * Time.deltaTime);
		//	xItem.Object.transform.LookAt(xItem.Target);
		}
	}

	public int GetID(int SearchID)
	{
		foreach (var item in items)
		{
			if (item.NetID.Equals(SearchID)) return items.IndexOf(item);
		}
            return 0;
    }

    public GameObject getObjWithID(int SearchID)
	{
		foreach (var item in items)
		{
			if (item.NetID.Equals(SearchID)) return item.Object;
		}

		return null;
	}


	public void Awake ()
	{
		instance = this;

		//Check if the ip address entered is valid. If not, sendMessage will broadcast to all ip addresses 
		IPAddress ip;
		if (IPAddress.TryParse (ipSend, out ip)) {

			remoteEndPoint = new IPEndPoint (ip, portSend);

		} else {

			remoteEndPoint = new IPEndPoint (IPAddress.Broadcast, portSend);

		}

		//Initialize client and thread for receiving

		client = new UdpClient (portListen);

		receiveThread = new Thread (new ThreadStart (ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
		
	}

	void Update ()
	{


		//Check if a message has been recibed
		if (received != ""){

			//Debug.Log("UDPClient: message received \'" + received + "\'");

			//Notify each object defined in the array with the message received
			foreach (GameObject g in notifyObjects)
			{
			    g.SendMessage(messageToNotify, received, SendMessageOptions.DontRequireReceiver);

			}
			//Clear message
			received = "";
		}
	}

	//Call this method to send a message from this app to ipSend using portSend
	public void SendValue (string valueToSend)
	{
		try {
			if (valueToSend != "") {

				//Get bytes from string
				byte[] data = Encoding.UTF8.GetBytes (valueToSend);

				// Send bytes to remote client
				client.Send (data, data.Length, remoteEndPoint);
				Debug.Log ("UDPClient: send \'" + valueToSend + "\'");
				//Clear message
				valueToSend = "";
	
			}
		} catch (Exception err) {
			Debug.LogError ("Error udp send : " + err.Message);
		}
	}

	//This method checks if the app receives any message
	public void ReceiveData ()
	{
 
		while (true) {

			try {
				// Bytes received
				IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);
				byte[] data = client.Receive (ref anyIP);

				// Bytes into text
				string text = "";
				text = Encoding.UTF8.GetString (data);
	
                received = text;
				Debug.Log("UDPClient: message received \'" + received + "\'");

			} catch (Exception err) {
				Debug.Log ("Error:" + err.ToString ());
			}
		}
	}
		
	//Exit UDP client
	public void OnDisable ()
	{
		if (receiveThread != null) {
				receiveThread.Abort ();
				receiveThread = null;
		}
		client.Close ();
		Debug.Log ("UDPClient: exit");
	}
		
}