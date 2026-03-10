using Microsoft.AspNetCore.Mvc;

using Proyecto_2.SusanMurillo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_2.SusanMurillo.Controllers
{
    [Route("api/Lavado")]
    [ApiController]
    public class LavadoController : ControllerBase
    {
        private DBContexto ElContextoDeLaBaseDeDatos;
        public LavadoController(DBContexto contexto)
        {
            ElContextoDeLaBaseDeDatos = contexto;
        }

        [HttpGet("ObtengaLaLista")]
        public List<Lavado> ObtengaLaLista()
        {
            var resultado = from c in ElContextoDeLaBaseDeDatos.Lavadoss
                            select c;
            return resultado.ToList();
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Lavado lavados)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            lavados.Impuesto = lavados.pago * 0.13m;

            try
            {
                ElContextoDeLaBaseDeDatos.Lavadoss.Add(lavados);
                ElContextoDeLaBaseDeDatos.SaveChanges();

                var lavadoGuardado = ElContextoDeLaBaseDeDatos.Lavadoss.Find(lavados.IdLavado);

                return Ok(lavadoGuardado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar lavado: {ex.Message}");
            }
        }

        [HttpPut("Editar")]

        public IActionResult EditarLavado([FromBody] Lavado lavado)
        {
            if (lavado == null || lavado.IdLavado <= 0)
                return BadRequest("Datos del Lavado inválidos."); ;

            var LavadoAModificar = ElContextoDeLaBaseDeDatos.Lavadoss
                .FirstOrDefault(v => v.IdLavado == lavado.IdLavado);

            if (LavadoAModificar == null)
                return NotFound($"No se encontró el vehículo con placa {lavado.IdLavado}.");

            var clienteExiste = ElContextoDeLaBaseDeDatos.Clientess
                .Any(c => c.Id == lavado.IdCliente);

            if (!clienteExiste)
                return BadRequest($"El cliente con ID {lavado.IdCliente} no existe.");


            try
            {
               LavadoAModificar.IdLavado = lavado.IdLavado;
               LavadoAModificar.PlacaVehiculo=lavado.PlacaVehiculo;
               LavadoAModificar.IdCliente = lavado.IdCliente;
                LavadoAModificar.CedulaEmpleado=lavado.CedulaEmpleado;
                LavadoAModificar.TiposDeLavado=lavado.TiposDeLavado;
                LavadoAModificar.pago = lavado.pago;
                LavadoAModificar.Impuesto = lavado.Impuesto;
                LavadoAModificar.Total = lavado.Total;
                LavadoAModificar.Estado = lavado.Estado;


                ElContextoDeLaBaseDeDatos.SaveChanges();

                return Ok(lavado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar los cambios: {ex.Message}");
            }
        }
        [HttpGet("ClientesDisponibles")]
        public IActionResult ObtenerClientes()
        {
            var clientes = ElContextoDeLaBaseDeDatos.Clientess.Select(c => new { c.Id, c.Nombre }).ToList();
            return Ok(clientes);
        }
        [HttpGet("PlacasDisponibles")]
        public IActionResult ObtenerPlacas()
        {
            var placas = ElContextoDeLaBaseDeDatos.RegistroVehiculoss
                .Select(v => new { Placa = v.Placa })
                .ToList();
            return Ok(placas);
        }
        
        [HttpGet("ObtenerRegsitro/{id}")]
        public Lavado ObtengaElRegistro(int id)
        {
            List<Lavado> lista;
            lista = ObtengaLaLista();
            foreach (var lavado in lista)
            {
                if (lavado.IdLavado == id)
                    return lavado;
            }
            return null;
        }


        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            var lavadoExistente = ElContextoDeLaBaseDeDatos.Lavadoss.FirstOrDefault(c => c.IdLavado == id);

            if (lavadoExistente == null)
                return NotFound($"No se encontró el lavados con placa {id}.");

            try
            {
                ElContextoDeLaBaseDeDatos.Lavadoss.Remove(lavadoExistente);
                ElContextoDeLaBaseDeDatos.SaveChanges();

                return Ok($"lavado con ID {id} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar lavado: {ex.Message}");
            }
        }
        [HttpGet("ClientesPorContactar")]
        public List<ClienteReporteDTO> ObtenerClientesParaContacto()
        {
            DateTime fechaLimite = DateTime.Now.AddMonths(-1);
            var consulta = from cliente in ElContextoDeLaBaseDeDatos.Clientess
            join registro in ElContextoDeLaBaseDeDatos.RegistroVehiculoss
                               on cliente.Id equals registro.ClienteId into registrosCliente
                           from ultimoRegistro in registrosCliente.OrderByDescending(r => r.UltimaFechaDeAtencion).Take(1).DefaultIfEmpty()
                           where ultimoRegistro == null || ultimoRegistro.UltimaFechaDeAtencion <= fechaLimite
                           select new ClienteReporteDTO
                           {
                               ClienteId = cliente.Id,
                               Nombre = cliente.Nombre,
                               Telefono = cliente.Telefono,
                               PlacaVehiculo = ultimoRegistro != null ? ultimoRegistro.Placa : "Sin registros",
                               UltimaFechaDeAtencion = ultimoRegistro != null ? ultimoRegistro.UltimaFechaDeAtencion : DateTime.MinValue
                           };

            return consulta.ToList();
        }












    }

}  
