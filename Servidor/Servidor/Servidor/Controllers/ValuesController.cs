using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Servidor.Analizador.CHISON;
using Servidor.Analizador.CQL;
using Servidor.Analizador.LUP;
using Servidor.NOSQL.Modelos;

namespace Servidor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            String text = System.IO.File.ReadAllText("entrada2.txt");
            /*Sintactico_LUP sintactico = new Sintactico_LUP();
            sintactico.Analizar(text, new Gramatica_LUP());
            string salida = "";
            foreach (string item in sintactico.salida)
            {
                salida += item.ToString() +"\n";
            }*/

            Sintactico_CHISHON sintactico = new Sintactico_CHISHON();
            sintactico.Analizar(text, new Gramatica_CHISON());
            string salida = "Analisis: " + sintactico.Validar(text, new Gramatica_CHISON()).ToString() + "\n";

            foreach (string item in sintactico.salida)
            {
                salida += item.ToString() + "\n";
            }

            foreach (Usuario item in sintactico.db_nosql.Usuarios)
            {
                salida += "USUARIO: " + item.Name + " PASSWORD: " + item.Password + " PERMISOS: " + agregar(item.Permisos) + "\n";
            }
            salida += "\n\n\n";

            salida += sintactico.db_nosql.Databases[0].ArmarHMTL(0);

            return salida;
        }
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
        public void Post([FromBody] string value)
        {
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
