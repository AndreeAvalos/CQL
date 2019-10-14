using Servidor.Models.FCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Tipo_IF
    {
        Operacion condicion;
        List<Tipo_IF> else_if = new List<Tipo_IF>();
        LinkedList<Instruccion> if_instrucciones = new LinkedList<Instruccion>();
        LinkedList<Instruccion> else_instrucciones = new LinkedList<Instruccion>();

        public Operacion Condicion { get => condicion; set => condicion = value; }
        public List<Tipo_IF> Else_if { get => else_if; set => else_if = value; }
        public LinkedList<Instruccion> If_instrucciones { get => if_instrucciones; set => if_instrucciones = value; }
        public LinkedList<Instruccion> Else_instrucciones { get => else_instrucciones; set => else_instrucciones = value; }
    }
}


