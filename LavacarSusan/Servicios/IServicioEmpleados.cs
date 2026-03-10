using _03075.Proyecto_1.SusanMurillo.Models;

namespace _03075.Proyecto_1.SusanMurillo.Servicios
{
    public interface IServicioEmpleados
    {
        public Task <List<Empleados>> Get();
        public Task<bool> Create(Empleados obj_empleados);
        public Task<Empleados> Buscar(string cedula);
        public Task<bool> Editar(Empleados obj_empleados, string cedulaOriginal);
        Task<bool> Eliminar(string cedula);
        Task<List<EmpleadoDTO>> ObtenerEmpleados();
    }
}
