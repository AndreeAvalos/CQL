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


        public Tipo getType(string id)
        {
            foreach (Simbolo s in this)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    return s.Tipo;
                }
            }
            return Tipo.NO_ACEPTADO;
        }
        public string tipoAsignado(string id)
        {
            foreach (Simbolo s in this)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    return s.Tipo_asignado;
                }
            }
            return "NO EXISTE EL TIPO";
        }
        public bool existID(string id)
        {

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
                    if (s.Tipo == Tipo.ENTERO)
                    {
                        s.Valor = Convert.ToInt32(valor);
                    }
                    else if (s.Tipo == Tipo.DECIMAL)
                    {
                        s.Valor = Convert.ToDouble(valor);
                    }
                    else if (s.Tipo == Tipo.CADENA)
                    {
                        s.Valor = valor.ToString();
                    }
                    return;
                }
            }
        }
    }
}