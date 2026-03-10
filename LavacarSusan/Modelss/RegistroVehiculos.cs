using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_2.SusanMurillo.Models
{
    public class RegistroVehiculos
    {
        [Key]
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Tracion { get; set; }
        public string Color { get; set; }
        public DateTime UltimaFechaDeAtencion { get; set; }
        public bool TratamientoEspecial { get; set; }
        [ForeignKey("ClienteId")]
        public int ClienteId { get; set; }
      
      


    }
}
