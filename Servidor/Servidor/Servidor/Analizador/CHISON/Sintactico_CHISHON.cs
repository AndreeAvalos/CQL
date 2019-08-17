using Irony.Parsing;
using Servidor.NOSQL.Estructuras;
using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Analizador.CHISON
{
    public class Sintactico_CHISHON
    {
        public List<string> salida = new List<string>();
        public Manejo db_nosql;

        #region Auxiliares

        #endregion

        public bool Validar(String entrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);
            if (arbol.Root != null) return true;
            else return false;
        }

        public ParseTreeNode Analizar(String entrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;

            //instanciamos un objeto para la base de datos no-sql
            db_nosql = new Manejo();

            Instrucciones(raiz.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));


            return arbol.Root.ChildNodes.ElementAt(0);
        }

        private void Instrucciones(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                Instrucciones(nodo.ChildNodes.ElementAt(0));
                Instruccion(nodo.ChildNodes.ElementAt(2));
            }
            else Instruccion(nodo.ChildNodes.ElementAt(0));

        }

        private void Instruccion(ParseTreeNode nodo)
        {

            string produccion = nodo.ChildNodes.ElementAt(0).ToString();

            switch (produccion)
            {
                case "USERS":
                    db_nosql.Usuarios = (List<Usuario>)Ejecutar(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3));
                    salida.Add("Se agregaron usuarios al sistema ");
                    break;
                case "DATABASES":
                    Ejecutar(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3));
                    salida.Add("Se agrego la base de datos al sistema ");
                    break;
            }


        }

        private object Ejecutar(ParseTreeNode nodo)
        {

            string produccion = nodo.Term.Name.ToString();

            switch (produccion)
            {
                case "DATABASES2":

                    break;

                case "USERS2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Usuario> usuarios = (List<Usuario>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        usuarios.Add((Usuario)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return usuarios;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Usuario> users = new List<Usuario>();
                        users.Add((Usuario)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return users;
                    }
                    else
                    {
                        return new List<Usuario>();
                    }
                case "USERS3":
                    List<Tipo_Objeto> lst_objetos = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(1));
                    Usuario new_user = new Usuario();

                    foreach (Tipo_Objeto item in lst_objetos)
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) new_user.Name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"password\"")) new_user.Password = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"permissions\"")) new_user.Permisos = (List<Permiso>) item.Valor;
                    }
                    return new_user;
                case "USERS4":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> tipo_Objetos = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        tipo_Objetos.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return tipo_Objetos;

                    }
                    else
                    {
                        List<Tipo_Objeto> objetos = new List<Tipo_Objeto>();
                        objetos.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return objetos;
                    }
                case "USER":
                    return Ejecutar(nodo.ChildNodes.ElementAt(0));
                case "NAME":
                    Tipo_Objeto objeto = new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());
                    return objeto;
                case "PASSWORD":
                    Tipo_Objeto objeto2 = new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());
                    return objeto2;
                case "PERMISSIONS":
                    Tipo_Objeto objeto3 = new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));
                    return objeto3;
                case "VALOR":
                    String evaluar = nodo.ChildNodes[0].Term.Name;
                    switch (evaluar)
                    {
                        case "Cadena":
                            return nodo.ChildNodes[0].ToString().Replace(" (Cadena)", "");
                        case "Identificador":
                            return nodo.ChildNodes[0].ToString().Replace(" (Identificador)", "");
                        case "Numero":
                            return nodo.ChildNodes[0].ToString().Replace(" (Numero)", "");
                        default:
                            return "";
                    }
                case "PERMISSIONS2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Permiso> permisos = (List<Permiso>) Ejecutar(nodo.ChildNodes.ElementAt(0));
                        permisos.Add((Permiso) Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return permisos;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Permiso> permisos = new List<Permiso>();
                        permisos.Add((Permiso) Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return permisos;
                    }
                    else return new List<Permiso>();
                case "PERMISSION":
                    Tipo_Objeto aux =(Tipo_Objeto) Ejecutar(nodo.ChildNodes.ElementAt(1));
                    Permiso permiso = new Permiso(aux.Valor.ToString());
                    return permiso;

            }
            return null;
        }


    }
}
