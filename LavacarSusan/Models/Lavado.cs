using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _03075.Proyecto_1.SusanMurillo.Models
{
    public class Lavado
    {
        public int IdLavado { get; set; }
        public string PlacaVehiculo { get; set; }
        public int IdCliente { get; set; }
        public string CedulaEmpleado { get; set; }
        public string TiposDeLavado { get; set; }
        public decimal pago { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }


        
    }
}
