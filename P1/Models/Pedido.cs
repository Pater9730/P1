namespace P1.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }

        public Persona? Persona { get; set; }
    }
}
