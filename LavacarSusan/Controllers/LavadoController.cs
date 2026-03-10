using _03075.Proyecto_1.SusanMurillo.Models;
using _03075.Proyecto_1.SusanMurillo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace _03075.Proyecto_1.SusanMurillo.Controllers
{
    public class LavadoController : Controller
    {
        private readonly IServicioLavado _iservicioLavado;
        private readonly IServicioRegistrarVehiculos _iservicioRegistrarVehiculos;
        private readonly IServicioEmpleados _iservicioEmpleados;
        private string placa;

        public LavadoController(IServicioLavado iservicioLavado, IServicioRegistrarVehiculos servicioRegistrarVehiculos, IServicioEmpleados servicioEmpleados )
        {
            _iservicioLavado = iservicioLavado;
            _iservicioRegistrarVehiculos = servicioRegistrarVehiculos;
            _iservicioEmpleados = servicioEmpleados;
        }


        // GET: LavdoController
        public async Task<ActionResult> Index()
        {
            var lavado = await _iservicioLavado.Get();

            if (lavado == null || !lavado.Any())
            {
                ViewBag.Mensaje = "No se encontraron lavados";
            }

            return View(lavado);
        }

        // GET: LavadoController/Details/5
        public ActionResult Buscar(int id)
        {
            return View();
        }
        // POST: LavadoController/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Buscar(int? IdLavado)
        {
            if (!IdLavado.HasValue)
            {
                ModelState.AddModelError("", "Debe ingresar una IdLavado.");
                return View();
            }

            try
            {
                var lavadoEncontrado = await _iservicioLavado.Buscar(IdLavado.Value);

                if (lavadoEncontrado == null)
                {
                    ModelState.AddModelError("", "No se encontró un lavado con esa id.");
                    return View();
                }

                return View("BuscarLavado", lavadoEncontrado);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al consultar el servicio: " + ex.Message);
                return View();
            }
        }

        // GET: LavdoController/Create
        public async Task<ActionResult> Create()
        {
            CargarListasLavado();
        
            var vehiculos = await _iservicioRegistrarVehiculos.Get();

            ViewBag.PlacasVehiculos = vehiculos.Select(v => new SelectListItem
            {
                Text = v.Placa,
                Value = v.Placa
            }).ToList();

            var placasYClientes = await _iservicioRegistrarVehiculos.Get();
            ViewBag.PlacasYClientesJson = JsonConvert.SerializeObject(placasYClientes);

            var empleados = await _iservicioEmpleados.ObtenerEmpleados(); 
            ViewBag.Empleados = empleados.Select(e => new SelectListItem
            {
                Text = e.Cedula, 
                Value = e.Cedula
            }).ToList();


            return View();
        }

        // POST: LavdoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Lavado LavadoNuevo)
        {
            if (LavadoNuevo == null)
            {
                ModelState.AddModelError("", "Debe ingresar los datos del lavado.");
                CargarListasLavado();
                return View(LavadoNuevo);
            }

            try
            {
                var (creado, error) = await _iservicioLavado.Create(LavadoNuevo);
                if (creado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear: " + error);
                    CargarListasLavado();
                    return View(LavadoNuevo);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear lavado: {ex.Message}");
                CargarListasLavado();
                return View(LavadoNuevo);
            }
        }

        // GET: LavdoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var lavado = await _iservicioLavado.Buscar(id);

            if (lavado == null)
            {
                return NotFound();
            }

            CargarListasLavado();

            return View(lavado);
        }

        // POST: LavdoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Lavado lavadoEditado)
        {
            if (id != lavadoEditado.IdLavado)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                CargarListasLavado();
                return View(lavadoEditado);
            }

            try
            {
                bool actualizado = await _iservicioLavado.Editar(lavadoEditado, id);
                if (actualizado)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "No se pudo actualizar el lavado.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }

            CargarListasLavado();
            return View(lavadoEditado);
        }
        private void CargarListasLavado()
        {
            ViewBag.TiposLavado = new List<SelectListItem>
    {
        new SelectListItem { Text = "Básico", Value = "Básico" },
        new SelectListItem { Text = "Premium", Value = "Premium" },
        new SelectListItem { Text = "Deluxe", Value = "Deluxe" },
        new SelectListItem { Text = "La joya", Value = "La joya" }
    };

            ViewBag.EstadosLavado = new List<SelectListItem>
    {
        new SelectListItem { Text = "En proceso", Value = "En proceso" },
        new SelectListItem { Text = "Facturado", Value = "Facturado" },
        new SelectListItem { Text = "Agendado", Value = "Agendado" }


    };



        }
        // GET: LavdoController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var lavado = await _iservicioLavado.Buscar(id);

            if (lavado == null)
            {
                return NotFound();
            }

            return View(lavado);
        }

        // POST: LavdoController/Delete/5
        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var resultado = await _iservicioLavado.Eliminar(id);

            if (!resultado)
            {
                ModelState.AddModelError("", "No se pudo eliminar el lavado.");
                var lavado = await _iservicioLavado.Buscar(id);
                return View(lavado);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Mostrar reporte
        public async Task<IActionResult> ClientesPorContactar()
        {
            var clientes = await _iservicioLavado.GetClientesPorContactar();
            return View(clientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientesPorContactar(string filtro)
        {
            var clientes = await _iservicioLavado.GetClientesPorContactar();

            if (!string.IsNullOrEmpty(filtro))
            {
                clientes = clientes
                    .Where(c => c.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(clientes);
        }

    }
}
