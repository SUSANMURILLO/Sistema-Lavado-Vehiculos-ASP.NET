using Microsoft.AspNetCore.Mvc;


using Proyecto_2.SusanMurillo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_2.SusanMurillo.Controllers
{
    [Route("api/Cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private DBContexto ElContextoDeLaBaseDeDatos;
        public ClienteController(DBContexto contexto)
        {
            ElContextoDeLaBaseDeDatos = contexto;
        }

        [HttpGet("ObtengaLaLista")]
        public List<Cliente> ObtengaLaLista()
        {
            var resultado = from c in ElContextoDeLaBaseDeDatos.Clientess
                            select c;
            return resultado.ToList();
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ElContextoDeLaBaseDeDatos.Clientess.Add(cliente);
            ElContextoDeLaBaseDeDatos.SaveChanges();
            return Ok(cliente);
        }
        [HttpPut("Editar")]

        public IActionResult EditarElCliente([FromBody] Cliente cliente)
        {
            if (cliente == null || cliente.Id <= 0)
                return BadRequest("Datos del cliente inválidos.");

            var clienteExistente = ElContextoDeLaBaseDeDatos.Clientess.FirstOrDefault(c => c.Id == cliente.Id);

            if (clienteExistente == null)
                return NotFound($"No se encontró el cliente con ID {cliente.Id}.");

            try
            {
             
                clienteExistente.Identificacion = cliente.Identificacion;
                clienteExistente.Nombre = cliente.Nombre;
                clienteExistente.Provincia = cliente.Provincia;
                clienteExistente.Canton = cliente.Canton;
                clienteExistente.Distrito = cliente.Distrito;
                clienteExistente.Direccion = cliente.Direccion;
                clienteExistente.Telefono = cliente.Telefono;
                clienteExistente.PreferenciaDeLavado = cliente.PreferenciaDeLavado;
                
                ElContextoDeLaBaseDeDatos.SaveChanges();

                return Ok(clienteExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar los cambios: {ex.Message}");
            }
        }


        [HttpGet("ObtenerCliente/{id}")]
        public Cliente ObtengaElCliente(int id)
        {
            List<Cliente> lista;
            lista = ObtengaLaLista();
            foreach (var cliente in lista)
            {
                if (cliente.Id == id)
                    return cliente;
            }
            return null;
        }


        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            var clienteExistente = ElContextoDeLaBaseDeDatos.Clientess.FirstOrDefault(c => c.Id == id);

            if (clienteExistente == null)
                return NotFound($"No se encontró el cliente con ID {id}.");

            try
            {
                ElContextoDeLaBaseDeDatos.Clientess.Remove(clienteExistente);
                ElContextoDeLaBaseDeDatos.SaveChanges();

                return Ok($"Cliente con ID {id} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar cliente: {ex.Message}");
            }
        }
    }
}
