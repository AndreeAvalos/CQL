namespace Servidor.Models
{
    public class Simbolo
    {
        private Tipo tipo;
        private string id;
        private object valor;

        public Simbolo(Tipo tipo, string id)
        {
            this.tipo = tipo;
            this.id = id;
        }

        public Tipo Tipo { get => tipo; set => tipo = value; }
        public string Id { get => id; set => id = value; }
        public object Valor { get => valor; set => valor = value; }
    }
}