using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Servidor.Analizador.CHISON;
using Servidor.Analizador.CQL;
using Servidor.Analizador.LUP;
using Servidor.Models;
using Servidor.NOSQL.Modelos;

namespace Servidor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class ValuesController : ControllerBase
    {
        string salida = "";
        // GET api/values
        [HttpGet]
        public string Get()
        {
            /*String text = System.IO.File.ReadAllText("entradaCliente.txt");
            Sintactico_LUP sintactico = new Sintactico_LUP();

            if (sintactico.Validar(text, new Gramatica_LUP()))
            {
                sintactico.Analizar(text, new Gramatica_LUP());*/
            foreach (string item in Program.log_errores)
            {
                salida += item + "\n";
            }


            return salida;
        }
        /* public IEnumerable<string> Get()
         {
             return new string[] { "data", "TODOS LOS DATOS" };
         }*/

        private string agregar(List<Permiso> permisos)
        {
            string salida = "";
            foreach (Permiso item in permisos)
            {
                salida += item.Name + ", ";
            }
            return salida;
        }
        // GET api/values/5
        [HttpGet("{id}")]

        public ActionResult<string> Get(int id)
        {
            return "value";
        }


        // POST api/values
        [HttpPost]
        public IEnumerable<string> Post(entrada value)
        {
            List<string> respuesta = new List<string>();
            if (Program.sistema != null)
            {
                string text = value.Data;
                Sintactico_LUP sintactico = new Sintactico_LUP();
                if (sintactico.Validar(text, new Gramatica_LUP()))
                {
                    sintactico.Analizar(text, new Gramatica_LUP());
                    foreach (string item in sintactico.salida)
                    {
                        respuesta.Add(item);
                    }
                }
            }
            else {
                respuesta.Add(Program.buildMessage("El sistma no existe."));
            }
            return respuesta ;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
