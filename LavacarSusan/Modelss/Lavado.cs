using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Proyecto_2.SusanMurillo.Models
{
    public class Lavado
    {
        [Key]
        public int IdLavado { get; set; }
        public string PlacaVehiculo { get; set; }
        public int IdCliente { get; set; }
        public  string CedulaEmpleado { get; set; }
        public string TiposDeLavado { get; set; }
        [Required]
        public decimal pago { get; set; }
        [Required]
        public decimal Impuesto { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Total { get; set; }
        
        public string Estado { get; set; }

       
    }
}
