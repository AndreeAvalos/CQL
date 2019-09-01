using System;
using System.Collections.Generic;

namespace Servidor.Models
{
    public class TablaDeSimbolos : LinkedList<Simbolo>
    {
        TablaDeSimbolos padre = null;
        public TablaDeSimbolos() : base()
        {

        }

        public Object getValor(String id)
        {
            return getValor(id, this);
        }
        public Object getValor(string id, TablaDeSimbolos nodo)
        {
            foreach (Simbolo s in nodo)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    return s.Valor;
                }
            }
            if (nodo != null) return getValor(id, nodo.padre);
            return null;
        }

        public Tipo getType(string id)
        {
            return getType(id, this);
        }

        private Tipo getType(string id, TablaDeSimbolos nodo)
        {
            foreach (Simbolo s in nodo)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    return s.Tipo;
                }
            }
            if (nodo.padre != null) return getType(id, nodo.padre);
            else return Tipo.NO_ACEPTADO;
        }

        public string tipoAsignado(string id)
        {
            return tipoAsignado(id, this);
        }

        private string tipoAsignado(string id, TablaDeSimbolos nodo)
        {
            foreach (Simbolo s in nodo)
            {
                if (s.Id.ToLower().Equals(id.ToLower()))
                {
                    return s.Tipo_asignado;
                }
            }
            if (nodo.padre != null) return tipoAsignado(id, nodo);
            else
                return "NO EXISTE EL TIPO";
        }
        //Si existe el id en el ambito actual, solo se utiliza para declarar variables

        public bool existID(string id)
        {
            return existID(id, this);
        }
        private bool existID(string id, TablaDeSimbolos nodo)
        {
            foreach (Simbolo item in nodo)
            {
                if (item.Id.ToLower().Equals(id.ToLower())) return true;
            }
            if (nodo.padre != null) return existID(id, nodo.padre);
            else return false;
        }
        public bool existID_AA(string id)
        {
            foreach (Simbolo item in this)
            {
                if (item.Id.ToLower().Equals(id.ToLower())) return true;
            }
            return false;
        }

        public void addPadre(TablaDeSimbolos ts)
        {
            this.padre = ts;
        }

        public void setValor(String id, Object valor)
        {
            setValor(id, valor, this);
        }
        public void setValor(string id, object valor, TablaDeSimbolos nodo)
        {

            foreach (Simbolo s in nodo)
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
            if (nodo.padre != null) setValor(id, valor, nodo.padre);
        }
    }
}