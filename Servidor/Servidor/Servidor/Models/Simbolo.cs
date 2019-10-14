namespace Servidor.Models
{
    public class Simbolo
    {
        private Tipo tipo;
        private string id;
        private string tipo_asignado;
        private object valor;
        private bool instanciado;
        private Tipo sub_tipo;

        public Simbolo(Tipo tipo, string id)
        {
            this.tipo = tipo;
            this.id = id;
        }

        public Tipo Tipo { get => tipo; set => tipo = value; }
        public string Id { get => id; set => id = value; }
        public object Valor { get => valor; set => valor = value; }
        public string Tipo_asignado { get => tipo_asignado; set => tipo_asignado = value; }
        public bool Instanciado { get => instanciado; set => instanciado = value; }
        public Tipo Sub_tipo { get => sub_tipo; set => sub_tipo = value; }
    }
}