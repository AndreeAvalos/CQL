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


        public Tipo getType(string id) {
            foreach (Simbolo s in this)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    return s.Tipo;
                }
            }
            return Tipo.NO_ACEPTADO;
        }
        public string tipoAsignado(string id) {
            foreach (Simbolo s in this)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    return s.Tipo_asignado;
                }
            }
            return "NO EXISTE EL TIPO";
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