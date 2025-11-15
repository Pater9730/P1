namespace P1.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
