using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using _03075.Proyecto_1.SusanMurillo.Models;
using _03075.Proyecto_1.SusanMurillo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _03075.Proyecto_1.SusanMurillo.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IServicioCliente _iservicioCliente;
        private static List<Cliente> ListaClientes = new List<Cliente>();

        public ClienteController(IServicioCliente iservicioCliente)
        {
            _iservicioCliente = iservicioCliente;

        }


        // GET: ClienteController
        public async Task<ActionResult> Index()
        {

            var cliente = await _iservicioCliente.Get();
            return View(cliente);
        }

        // GET: ClienteController/Details/5
        public ActionResult Buscar(int id)
        {
            return View();
        }
        // POST: ClienteController/Buscar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Buscar(Cliente BuscarCliente)

        {
            if (BuscarCliente == null || BuscarCliente.Id <= 0)
            {
                ViewBag.Mensaje = "Debe ingresar una identificación válida.";
                return View();
            }

            var clienteEncontrado = await _iservicioCliente.Buscar(BuscarCliente.Id);

            if (clienteEncontrado != null && clienteEncontrado.Id > 0)
            {
                return View("ResultadoCliente", clienteEncontrado);
            }
            else
            {
                ViewBag.Mensaje = "Cliente no encontrado";
                return View();
            }
        }
        // GET: ClienteController/Create
        public ActionResult Create()
        {
            CargarPreferencias();
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente NuevoCliente)
        {
           
            try
            {
                if (!ModelState.IsValid)
                {
                    CargarPreferencias(); 
                    return View(NuevoCliente);
                }

                var resultado = await _iservicioCliente.Create(NuevoCliente);

                if (resultado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Mensaje = "Error al gauarda el cliente";
                        return View(NuevoCliente);
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al gauarda el cliente" + ex.Message;
                return View(NuevoCliente);
            }
        }

        // GET: ClienteController/Edit/5

        public async Task<ActionResult> Edit(int id)
        {
            CargarPreferencias();

            if (id <= 0)
                return NotFound("ID inválido.");

            var cliente = await _iservicioCliente.Buscar(id); 

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }


        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Cliente clienteEditado)
        {
            CargarPreferencias();

            if (!ModelState.IsValid)
            {
                ViewBag.Mensaje = "Por favor, corrige los errores de validación.";
                return View(clienteEditado);
            }
            var resultado = await _iservicioCliente.Editar(clienteEditado);

            if (resultado)
                return RedirectToAction(nameof(Index));

            ViewBag.Mensaje = "Error al editar el cliente.";
            return View(clienteEditado);
        }

        // GET: Cliente/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
                return NotFound("ID inválido.");

            var cliente = await _iservicioCliente.Buscar(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Cliente/DeleteConfirmed/56
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var resultado = await _iservicioCliente.Eliminar(id);

                if (resultado)
                    return RedirectToAction(nameof(Index));

                ViewBag.Mensaje = "No se pudo eliminar el cliente.";
                var cliente = await _iservicioCliente.Buscar(id);
                return View("Delete", cliente);
            }
            catch
            {
                ViewBag.Mensaje = "Ocurrió un error inesperado.";
                var cliente = await _iservicioCliente.Buscar(id);
                return View("Delete", cliente);
            }
        }

        private void CargarPreferencias()
        {
            ViewBag.PreferenciaDeLavado = new List<SelectListItem>
    {
        new SelectListItem { Text = "Semanal", Value = "Semanal" },
        new SelectListItem { Text = "Quincenal", Value = "Quincenal" },
        new SelectListItem { Text = "Mensual", Value = "Mensual" },
        new SelectListItem { Text = "Otro", Value = "Otro" }
    };
        }

    }
    }

