using _03075.Proyecto_1.SusanMurillo.Models;

namespace _03075.Proyecto_1.SusanMurillo.Servicios
{
    public interface IServicioCliente
    {
        public Task<List<Cliente>> Get();
        public Task<bool> Create(Cliente obj_cliente);
        public Task<Cliente> Buscar(int id);
        public Task<bool> Editar(Cliente obj_cliente);
        Task<bool> Eliminar(int id);
    }
}
