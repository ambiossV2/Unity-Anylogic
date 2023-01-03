using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JsonSplitter
{
    public class Form1 : MonoBehaviour
    {
        private string filePath;

        public string AgentText;
        public string BoxText;
        public string MaquinasText;
        public string TruckText;
        public string ForkliftText;
        public string PalletText;
        public JSONReader readerAgente;
        public JSONReader readerCajas;
        public JSONReader readerMaquinas;
        public JSONReader readerTruck;
        public JSONReader readerForklift;
        public JSONReader readerPallet;

        //public JSONReader readerPersonas;
        //public JSONReader readerAutos;

        public void Generate(string text)
        {
            var objetosJSON = text;
            List<MainObject> lista = DeserializarJSON(objetosJSON);
            GenerarArchivos(lista);

            readerAgente.GetDataAndPlay();
            readerCajas.GetDataAndPlay();
            readerMaquinas.GetDataAndPlay();
            readerForklift.GetDataAndPlay();
            readerPallet.GetDataAndPlay();
            readerTruck.GetDataAndPlay();
        }

     

        private List<MainObject> DeserializarJSON(string objetosJSON)
        {
            return JsonConvert.DeserializeObject<List<MainObject>>(objetosJSON);
        }

        private void GenerarArchivos(List<MainObject> lista)
        {
         //   print("Obteniendo tipos de Agentes...");
            //obtengo los nombres de los agentes y por cada agente hacemos un archivo
            Dictionary<string, int> diccionarioNombresDeArchivos = new Dictionary<string, int>();
            int index = 0;

            foreach (MainObject objeto in lista)
            {
                foreach (Data dato in objeto.data)
                {
                    if (!diccionarioNombresDeArchivos.ContainsKey(dato.ot)) {

                        diccionarioNombresDeArchivos.Add(dato.ot, index);
                        index++;
                    }
                }
            }
          //  print(" Finalizado");
            //fin nombres de agentes
//
         //   print("Obteniendo registros para cada agente...");
            int cantArchivos = diccionarioNombresDeArchivos.Count();

            List<List<Registro>> listaDeRegistrosEnArchivos = new List<List<Registro>>(); //es una lista de listas de registros, donde cada registro representa una fila en el archivo de salida

            //recorro el diccionario que contiene los valores de los nombres de los agentes sin repetir
            foreach (KeyValuePair<string, int> item in diccionarioNombresDeArchivos)
            {
                //por cada agente creo una lista de registros
                Console.WriteLine(item.Value+" " +item.Key);
                listaDeRegistrosEnArchivos.Add(new List<Registro>());
            }

            //recorro la lista principal y creo las listas de Registros
            foreach (MainObject objeto in lista)
            {
                foreach (Data dato in objeto.data)
                {
                    foreach (Value value in dato.value)
                    {
                        Registro r = new Registro(value.z, objeto.time, value.y, value.x, value.id, value.Speed, value.width, value.heigth); //creo el registo
                        int indexLista = diccionarioNombresDeArchivos[dato.ot]; //el indice de la lista va a ser igual al value del diccionario de esa key
                        listaDeRegistrosEnArchivos[indexLista].Add(r);
                    }
                }
            }

         //   print(" Finalizado");

            //Generar y guardar archivos .json
            int indexNombreNuevoArchivo = 0;
            foreach (var listaRegistrosPorAgentes in listaDeRegistrosEnArchivos)
            {
                string nombreNuevoArchivo = diccionarioNombresDeArchivos.FirstOrDefault(x => x.Value == indexNombreNuevoArchivo).Key;


                string nuevoArchivo = @"" + filePath + "\\" + nombreNuevoArchivo+".txt";
                if (!File.Exists(nuevoArchivo))
                {
                   
                    print("Generando archivo " +  nuevoArchivo + "...");
                    string texto = "[" + Environment.NewLine; 

                    foreach (var registro in listaRegistrosPorAgentes)
                    {
                        texto += registro.ToString() + Environment.NewLine;
                    }
                    texto += "]";



                    if (nuevoArchivo.ToLower().Contains("operator") || nuevoArchivo.ToLower().Contains("agent"))
                    {
                        AgentText = texto;
                    }
                    if (nuevoArchivo.ToLower().Contains("box")) BoxText = texto;
                    if (nuevoArchivo.ToLower().Contains("maquinas")) MaquinasText = texto;
                    if (nuevoArchivo.ToLower().Contains("pallet")) PalletText = texto;
                    if (nuevoArchivo.ToLower().Contains("truck")) TruckText = texto;
                    if (nuevoArchivo.ToLower().Contains("truck")) TruckText = texto;
                    if (nuevoArchivo.ToLower().Contains("forklift")) ForkliftText = texto;


                    // Guardar Texto
                    File.WriteAllText(Application.persistentDataPath+ "/" + nuevoArchivo, texto);
                   // print(" Finalizado");
                }

                indexNombreNuevoArchivo++;

            }



           // print("Cantidad de archivos generados: " + cantArchivos);
        }
    }
}
