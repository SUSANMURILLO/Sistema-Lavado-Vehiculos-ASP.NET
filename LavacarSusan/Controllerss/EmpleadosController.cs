using Microsoft.AspNetCore.Mvc;
using Proyecto_2.SusanMurillo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_2.SusanMurillo.Controllers
{
    [Route("api/Empleado")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private DBContexto ElContextoDeLaBaseDeDatos;
        public EmpleadosController(DBContexto contexto)
        {
            ElContextoDeLaBaseDeDatos = contexto;
        }

        [HttpGet("ObtengaLaLista")]
        public List<Empleados> ObtengaLaLista()
        {
            var resultado = from c in ElContextoDeLaBaseDeDatos.Empleadoss
                            select c;
            return resultado.ToList();
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Empleados empleado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ElContextoDeLaBaseDeDatos.Empleadoss.Add(empleado);
            ElContextoDeLaBaseDeDatos.SaveChanges();
            return Ok(empleado);
        }

        [HttpPut("Editar")]

        public IActionResult EditarElCliente([FromBody] Empleados empleado)
        {
            if (empleado == null || string.IsNullOrWhiteSpace(empleado.Cedula))
                return BadRequest("Datos del empleado inválidos.");

            var EmpleadoAModificar = ObtengaElEmpleado(empleado.Cedula);

            if (EmpleadoAModificar == null)
                return NotFound($"No se encontró el empleado con identificación {empleado.Cedula}.");

            try
            {

                EmpleadoAModificar.Cedula = empleado.Cedula;
                EmpleadoAModificar.FechaNacimiento = empleado.FechaNacimiento;
                EmpleadoAModificar.FechaIngreso = empleado.FechaIngreso;
                EmpleadoAModificar.SalarioXDia = empleado.SalarioXDia;
                EmpleadoAModificar.VacacionesAcumuladas = empleado.VacacionesAcumuladas;
                EmpleadoAModificar.FechadeRetiro = empleado.FechadeRetiro;


                ElContextoDeLaBaseDeDatos.SaveChanges();

                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar los cambios: {ex.Message}");
            }
        }
      

        [HttpGet("ObtenerEmpleado/{cedula}")]
        public Empleados ObtengaElEmpleado(string cedula)
        {
            List<Empleados> lista;
            lista = ObtengaLaLista();
            foreach (var empleados in lista)
            {
                if (empleados.Cedula == cedula)
                    return empleados;
            }
            return null;
        }


        [HttpGet("EmpleadosDisponibles")]
        public IActionResult ObtenerEmpleados()
        {
            var empleados = ElContextoDeLaBaseDeDatos.Empleadoss
                .Select(e => new EmpleadoDTO
                {
                    Cedula = e.Cedula
                   
                })
                .ToList();

            return Ok(empleados);
        }

        [HttpDelete("Eliminar/{cedula}")]
        public IActionResult Eliminar(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                return BadRequest("ID inválido.");

            var EmpleadoExistente = ElContextoDeLaBaseDeDatos.Empleadoss.FirstOrDefault(c => c.Cedula == cedula);

            if (EmpleadoExistente == null)
                return NotFound($"No se encontró el cliente con ID {cedula}.");

            try
            {
                ElContextoDeLaBaseDeDatos.Empleadoss.Remove(EmpleadoExistente);
                ElContextoDeLaBaseDeDatos.SaveChanges();

                return Ok($"Empleado con ID {cedula} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar empleado: {ex.Message}");
            }
        }
    }
}