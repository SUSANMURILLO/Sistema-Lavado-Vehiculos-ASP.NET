using _03075.Proyecto_1.SusanMurillo.Models;

namespace _03075.Proyecto_1.SusanMurillo.Servicios
{
    public interface IServicioLavado
    {
        Task<List<Lavado>> Get();
        Task<(bool Exito, string Error)> Create(Lavado obj_lavado);
        Task<Lavado> Buscar(int id);                          
        Task<bool> Editar(Lavado obj_lavado, int id); 
        Task<bool> Eliminar(int id);
        Task<List<ClienteReporteDTO>> GetClientesPorContactar();
        Task<List<PlacaClienteDTO>> ObtenerPlacasConClientes();

    }
}
