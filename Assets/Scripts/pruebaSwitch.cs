using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebaSwitch : MonoBehaviour
{
    public int Vida;
    public int Escudo;
    public enum prueba { v1,v2,v3,v4,v5,v6,v7,v8,v9,v10,v11,v12}

    public int switchvariable;

    public bool Call;
    // Update is called once per frame
    void Update()
    {
        if (Call)
        {
            Call = false;
            QuitarVida();
        }
    }

    public void QuitarVida()
    {

        if (switchvariable.Equals(0))
        {
        
        }

    }
}
