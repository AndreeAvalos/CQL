using Servidor.Analizador.CHISON;
using Servidor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Valor
    {
        string id;
        object val;
        int tipo_real;
        Tipo sub_tipo;

        public Valor(object val, Tipo sub_tipo)
        {
            this.val = val;
            this.sub_tipo = sub_tipo;
        }

        public Valor(string id, object val, int tipo_real)
        {
            this.id = id;
            this.val = val;
            this.tipo_real = tipo_real;
        }

        public string Id { get => id; set => id = value; }
        public object Val { get => val; set => val = value; }
        public Tipo Sub_tipo { get => sub_tipo; set => sub_tipo = value; }

        //1 = tipo objeto
        //2 = lista
        //4 = double
        //3 = int 
        //5 = bool
        //6 = time
        //7 = string 
        //8 = data
        //9 = null

        public string execCommit(int num_tabs)
        {
            string tabs = Program.getTabulaciones(num_tabs);
            string salida = tabs + "\"" + id + "\"= ";
            if (tipo_real == 1)
            {
                List<Tipo_Objeto> lst_aux = (List<Tipo_Objeto>)val;
                salida += "<\n";
                salida += writeMap(lst_aux, 3);
                salida += "\t\t>\n";
            }
            else if (tipo_real == 2)
            {
                List<Item_List> lst_aux = (List<Item_List>)val;
                salida += writeList(lst_aux, 2);

            }
            else if (tipo_real == 9) {
                salida += "null";
            }
            else if (tipo_real == 7)
            {
                salida += "\"" + val.ToString() + "\"";
            }
            else
            {
                salida += val.ToString();
            }
            return salida;

        }
        private string writeList(List<Item_List> lst, int num_tabs)
        {
            string salida = "";
            string tabs = Program.getTabulaciones(num_tabs);
            salida += "[";
            List<Item_List> lst_aux = lst;
            for (int i = 0; i < lst_aux.Count; i++)
            {
                if (lst_aux.ElementAt(i).Type == 7)
                {
                    if (i == lst_aux.Count - 1)
                    {
                        salida += "\"" + lst_aux.ElementAt(i).Valor + "\"";

                    }
                    else
                    {
                        salida += "\"" + lst_aux.ElementAt(i).Valor + "\",";
                    }

                }
                else if (tipo_real == 9)
                {
                    salida += "null";
                }
                else if (lst_aux.ElementAt(i).Type == 2)
                {
                    List<Item_List> lst_aux2 = (List<Item_List>)lst_aux.ElementAt(i).Valor;
                    if (i == lst_aux.Count - 1)
                    {

                        salida += writeList(lst_aux2, 0);
                    }
                    else
                    {
                        salida += writeList(lst_aux2, 0) + ",";
                    }
                }
                else if (lst_aux.ElementAt(i).Type == 1)
                {


                    List<Tipo_Objeto> lst_aux2 = (List<Tipo_Objeto>)lst_aux.ElementAt(i).Valor;
                    if (i == lst_aux.Count - 1)
                    {
                        salida += "\n" + Program.getTabulaciones(num_tabs + 1) + "<\n";
                        salida += writeMap(lst_aux2, num_tabs + 1);
                        salida += Program.getTabulaciones(num_tabs + 1) + ">\n";
                        salida += tabs;
                    }
                    else
                    {
                        salida += "\n" + Program.getTabulaciones(num_tabs + 1) + "<\n";
                        salida += writeMap(lst_aux2, num_tabs + 1);
                        salida += Program.getTabulaciones(num_tabs + 1) + ">,\n";
                        salida += tabs;
                    }
                }
                else
                {
                    if (i == lst_aux.Count - 1)
                    {
                        if (lst_aux.ElementAt(i).Valor != null)
                            salida += lst_aux.ElementAt(i).Valor.ToString();
                        else
                            salida += "null";
                    }
                    else
                    {
                        if(lst_aux.ElementAt(i).Valor!=null)
                        salida += lst_aux.ElementAt(i).Valor.ToString() + ",";
                        else
                            salida += "null,";
                    }


                }
            }
            return salida += "]";
        }

        private string writeMap(List<Tipo_Objeto> map, int num_tabs)
        {
            string salida = "";
            string tabs = Program.getTabulaciones(num_tabs);

            for (int i = 0; i < map.Count; i++)
            {
                salida += tabs + "\"" + map.ElementAt(i).Name + "\"= ";

                if (map.ElementAt(i).Type == 1)
                {
                    if (i == map.Count - 1)
                    {
                        salida += "<\n";
                        List<Tipo_Objeto> lst_aux = (List<Tipo_Objeto>)map.ElementAt(i).Valor;
                        salida += writeMap(lst_aux, num_tabs + 1);
                        salida += tabs + ">\n";
                    }
                    else
                    {

                        salida += "<\n";
                        List<Tipo_Objeto> lst_aux = (List<Tipo_Objeto>)map.ElementAt(i).Valor;
                        salida += writeMap(lst_aux, num_tabs + 1);
                        salida += tabs + ">,\n";
                    }
                }
                else if (map.ElementAt(i).Type == 2)
                {

                    List<Item_List> lst_aux = (List<Item_List>)map.ElementAt(i).Valor;
                    salida += writeList(lst_aux, 0) + "\n";

                }
                else if (tipo_real == 9)
                {
                    salida += "null";
                }
                else if (map.ElementAt(i).Type == 7)
                {
                    if (i == map.Count - 1)
                        salida += "\"" + map.ElementAt(i).Valor.ToString() + "\"\n";
                    else
                        salida += "\"" + map.ElementAt(i).Valor.ToString() + "\"\n";
                }
                else
                {
                    if (i == map.Count - 1)
                        salida += map.ElementAt(i).Valor.ToString() + "\n";
                    else
                        salida += map.ElementAt(i).Valor.ToString() + ",\n";

                }


            }


            return salida;

        }



        public string ArmarRespuesta()
        {

            try
            {
                List<Tipo_Objeto> lst = (List<Tipo_Objeto>)val;
                return "<td>DATOS_OBJETO</td>";
            }
            catch (Exception)
            {

                try
                {
                    List<string> lst = (List<string>)val;
                    return "<td>DATOS_LISTA</td>";
                }
                catch (Exception)
                {

                    return "<td>" + this.val + "</td>";
                }
            }

        }
    }
}
