﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Tabla
    {
        string name = "";
        List<Columna> columnas;
        List<Fila> filas;

        string link;
        bool exportada = false;

        public string Name { get => name; set => name = value; }
        public List<Columna> Columnas { get => columnas; set => columnas = value; }
        public List<Fila> Filas { get => filas; set => filas = value; }
        public bool Exportada { get => exportada; set => exportada = value; }
        public string Link { get => link; set => link = value; }

        public Tabla()
        {
            this.columnas = new List<Columna>();
            this.filas = new List<Fila>();
        }

        public bool Exist_Column(string val) {
            foreach (Columna item in columnas)
            {
                if (item.Equals(val)) return true;
            }
            return false;

        }

        public string ArmarRespuesta()
        {
            string salida = "<table> \n";
            salida += " <tr>\n  ";
            foreach (Columna item in Columnas)
            {
                salida += item.ArmarRespuesta().ToString();
            }
            salida += "\n </tr>\n";
            
            foreach (Fila item in Filas)
            {
                salida += " <tr>\n      ";
                salida += item.ArmarRespuesta(columnas)+"\n";
                salida += " </tr>\n";
            }

            salida += "</table> \n";
            return salida;
        }

        public string ArmarHTML()
        {
            string salida = "<table> \n";
            salida += "\t<tr>\n\t\t<th>NAME</th><th>TYPE</th><th>PK</th>\n\t</tr>\n";
            foreach (Columna item in Columnas)
            {

                salida += "\t\t<tr>" + item.ArmarHTML() + "</tr>\n";
            }
            salida += "</table> \n";
            return salida;
        }
    }
}
