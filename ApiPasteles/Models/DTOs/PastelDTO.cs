namespace ApiPasteles.Models.DTOs
{
    public class PastelDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Origen { get; set; }
        public decimal Precio { get; set; }
        public double PromedioSabor { get; set; }
        public double PromedioPresentacion { get; set; }
        public double PromedioFinal { get; set; }

    }
}
