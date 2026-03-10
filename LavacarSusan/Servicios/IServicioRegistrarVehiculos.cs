using _03075.Proyecto_1.SusanMurillo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _03075.Proyecto_1.SusanMurillo.Servicios
{
    public interface IServicioRegistrarVehiculos
    {

        Task<List<RegistroVehiculos>> Get(); 
        Task<RegistroVehiculos> Buscar(string placa);
        
        Task<bool> Create(RegistroVehiculos vehiculo); 
        Task<bool> Editar(RegistroVehiculos vehiculoEditado, string placa); 
        Task<bool> Eliminar(string placa);
        Task<List<PlacaClienteDTO>> ObtenerPlacasConClientes();



    }
}
