namespace _03075.Proyecto_1.SusanMurillo.Models
{
    public class RegistroVehiculos
    {
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Tracion { get; set; }
        public string Color { get; set; }
        public DateTime UltimaFechaDeAtencion { get; set; } 
        public bool TratamientoEspecial { get; set; }
        public int ClienteId { get; set; }

    }
}
