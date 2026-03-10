using System.Numerics;
using Microsoft.AspNetCore.Mvc;


using Proyecto_2.SusanMurillo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_2.SusanMurillo.Controllers
{
    [Route("api/RegistroVehiculos")]
    [ApiController]
    public class RegistroVehiculosController : ControllerBase
    {
        private DBContexto ElContextoDeLaBaseDeDatos;
        public RegistroVehiculosController(DBContexto contexto)
        {
            ElContextoDeLaBaseDeDatos = contexto;
        }

        [HttpGet("ObtengaLaLista")]
        public List<RegistroVehiculos> ObtengaLaLista()
        {
            var resultado = from c in ElContextoDeLaBaseDeDatos.RegistroVehiculoss
                            select c;
            return resultado.ToList();
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] RegistroVehiculos vehiculos)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = ElContextoDeLaBaseDeDatos.Clientess.FirstOrDefault(c => c.Id == vehiculos.ClienteId);
            if (cliente == null)
                return BadRequest($"El cliente con ID {vehiculos.ClienteId} no existe.");

            ElContextoDeLaBaseDeDatos.RegistroVehiculoss.Add(vehiculos);
            ElContextoDeLaBaseDeDatos.SaveChanges();
            return Ok(vehiculos);
        }

        [HttpPut("Editar")]

        public IActionResult EditarVehiculo([FromBody] RegistroVehiculos vehiculos)
        {
            if (vehiculos == null || string.IsNullOrWhiteSpace(vehiculos.Placa))
                return BadRequest("Datos del vehículo inválidos.");

            
            var VehiculoAModificar = ElContextoDeLaBaseDeDatos.RegistroVehiculoss
                .FirstOrDefault(v => v.Placa == vehiculos.Placa);

            if (VehiculoAModificar == null)
                return NotFound($"No se encontró el vehículo con placa {vehiculos.Placa}.");

            var clienteExiste = ElContextoDeLaBaseDeDatos.Clientess
                .Any(c => c.Id == vehiculos.ClienteId);

            if (!clienteExiste)
                return BadRequest($"El cliente con ID {vehiculos.ClienteId} no existe.");


            try
            {
                VehiculoAModificar.Placa = vehiculos.Placa;
                VehiculoAModificar.Marca = vehiculos.Marca;
                VehiculoAModificar.Modelo = vehiculos.Modelo;
                VehiculoAModificar.Tracion = vehiculos.Tracion;
                VehiculoAModificar.Color = vehiculos.Color;
                VehiculoAModificar.UltimaFechaDeAtencion= vehiculos.UltimaFechaDeAtencion;
                VehiculoAModificar.TratamientoEspecial = vehiculos.TratamientoEspecial;
                VehiculoAModificar.ClienteId = vehiculos.ClienteId;





             
                ElContextoDeLaBaseDeDatos.SaveChanges();

                return Ok(vehiculos);
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


        [HttpGet("ObtenerRegsitro/{placa}")]
        public RegistroVehiculos ObtengaElRegistro(string placa)
        {
            List<RegistroVehiculos> lista;
            lista = ObtengaLaLista();
            foreach (var vehiculo in lista)
            {
                if (vehiculo.Placa == placa)
                    return vehiculo;
            }
            return null;
        }

        [HttpGet("PlacasConClientes")]
        public List<PlacaClienteDTO> ObtenerPlacasConClientes()
        {
            var lista = ObtengaLaLista();
            var resultado = lista.Select(r => new PlacaClienteDTO
            {
                Placa = r.Placa,
                IdCliente = r.ClienteId
            }).ToList();

            return resultado;
        }
        [HttpDelete("Eliminar/{placa}")]
        public IActionResult Eliminar(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return BadRequest("ID inválido.");

            var vehiculoExistente = ElContextoDeLaBaseDeDatos.RegistroVehiculoss.FirstOrDefault(c => c.Placa == placa);

            if (vehiculoExistente == null)
                return NotFound($"No se encontró el vehiculo con placa {placa}.");

            try
            {
                ElContextoDeLaBaseDeDatos.RegistroVehiculoss.Remove(vehiculoExistente);
                ElContextoDeLaBaseDeDatos.SaveChanges();

                return Ok($"vehiculo con ID {placa} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar vehiculo: {ex.Message}");
            }
        }
    }
}
    


