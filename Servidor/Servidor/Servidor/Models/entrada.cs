using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class entrada
    {
        string data;

        public entrada(string data)
        {
            this.Data = data;
        }

        public string Data { get => data; set => data = value; }
    }
}
