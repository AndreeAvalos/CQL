using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Login
    {
        private string usuario;
        private string password;

        public Login(string usuario, string password)
        {
            this.Usuario = usuario;
            this.Password = password;
        }

        public string Usuario { get => usuario; set => usuario = value; }
        public string Password { get => password; set => password = value; }
    }
}
