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
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    return s.Valor;
                }
            }
            return null;
        }

        string hola = "";
        public bool instanciada(string id) {
            foreach (Simbolo s in this)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    if (s.Instanciado) return true;
                    else return false;
                }
            }
            return false;
        }
        public bool existID(string id) {

            foreach (Simbolo s in this)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
        public void setValor(String id, Object valor)
        {
            foreach (Simbolo s in this)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    s.Valor = valor;
                    return;
                }
            }
        }
    }
}