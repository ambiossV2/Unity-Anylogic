using System;
namespace JsonSplitter
{
    [Serializable]
    class Registro
    {
        public float z;
        public float tiempo;
        public float y;
        public float x;
        public int id;
        public float velocidad;
        public float width;
        public float heigth;

        public Registro(float z, float tiempo, float y, float x, int id, float velocidad, float width, float heigth)
        {
            this.z = z;
            this.tiempo = tiempo;
            this.y = y;
            this.x = x;
            this.id = id;
            this.velocidad = velocidad;
            this.width = width;
            this.heigth = heigth;
        }

        public override string ToString()
        {
            return "{\"Coordenada Z\":" + z.ToString(System.Globalization.CultureInfo.InvariantCulture) + 
                ",\"tiempo\":" + tiempo.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                ",\"Coordenada Y\":" + y.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                ",\"Coordenada X\":" + x.ToString(System.Globalization.CultureInfo.InvariantCulture) + 
                ",\"ID\":" + id +
                ",\"Velocidad\":" + velocidad.ToString(System.Globalization.CultureInfo.InvariantCulture)
                + ",\"Width\":" + width.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                ",\"Heigth\":" + heigth.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                 "}";
        }
    }
}
