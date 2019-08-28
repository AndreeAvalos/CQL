using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Error
    {
        string type;
        string descripcion;
        int fila, columna;
        string fecha, hora;

        public Error(string type, string descripcion, int fila, int columna, string fecha, string hora)
        {
            this.type = type;
            this.descripcion = descripcion;
            this.fila = fila;
            this.columna = columna;
            this.fecha = fecha;
            this.hora = hora;
        }

        public string Type { get => type; set => type = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int Fila { get => fila; set => fila = value; }
        public int Columna { get => columna; set => columna = value; }
        public string Fecha { get => fecha; set => fecha = value; }
        public string Hora { get => hora; set => hora = value; }
    }
}
