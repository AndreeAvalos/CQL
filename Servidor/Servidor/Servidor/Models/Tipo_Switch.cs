using Servidor.Models.FCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Tipo_Switch
    {
        Operacion expresion;
        List<Tipo_Case> casos;
        LinkedList<Instruccion> instrucciones_default;
        public Tipo_Switch(Operacion expresion, List<Tipo_Case> casos, LinkedList<Instruccion> instrucciones_default)
        {
            this.Expresion = expresion;
            this.Casos = casos;
            this.Instrucciones_default = instrucciones_default;
        }

        public Operacion Expresion { get => expresion; set => expresion = value; }
        public List<Tipo_Case> Casos { get => casos; set => casos = value; }
        public LinkedList<Instruccion> Instrucciones_default { get => instrucciones_default; set => instrucciones_default = value; }
    }
}
