using System;
using System.Collections.Generic;

namespace Servidor.Models
{
    public class TablaDeSimbolos : LinkedList<Simbolo>
    {
        public TablaDeSimbolos() : base()
        {

        }

        public Object getValor(String id)
        {
            foreach (Simbolo s in this)
            {
                if (s.Id.Equals(id))
                {
                    return s.Valor;
                }
            }
            Console.WriteLine("La variable " + id + " no existe en este ámbito.");
            return "Desconocido";
        }

        public void setValor(String id, Object valor)
        {
            foreach (Simbolo s in this)
            {
                if (s.Id.Equals(id))
                {
                    s.Valor = valor;
                    return;
                }
            }
            Console.WriteLine("La variable " + id + " no existe en este ámbito, por lo "
                    + "que no puede asignársele un valor.");
        }
    }
}