using System.ComponentModel.DataAnnotations;

namespace Proyecto_2.SusanMurillo.Models
{
    public class Empleados
    {

        [Key] // Indica que esta propiedad es la clave primaria
        [StringLength(12)]
        public string Cedula { get; set; }
      
        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; }

        [Required]
        public double SalarioXDia { get; set; }

        [Required]
        public int VacacionesAcumuladas { get; set; }

        [Required]
        public DateTime FechadeRetiro { get; set; }

        [Required]
        public decimal MontoLiquidacion { get; set; }
      
    }
}
