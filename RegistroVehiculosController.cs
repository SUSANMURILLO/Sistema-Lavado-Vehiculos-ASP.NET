using System.Numerics;
using _03075.Proyecto_1.SusanMurillo.Models;
using _03075.Proyecto_1.SusanMurillo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _03075.Proyecto_1.SusanMurillo.Controllers
{
    public class RegistroVehiculosController : Controller
    {
        private readonly IServicioRegistrarVehiculos _iservicioRegistrarVehiculos;
        private readonly IServicioCliente _servicioCliente;

        public RegistroVehiculosController(IServicioRegistrarVehiculos servicioRegistrarVehiculos, IServicioCliente servicioCliente)
        {
            _iservicioRegistrarVehiculos = servicioRegistrarVehiculos;
            _servicioCliente = servicioCliente;
        }


        // GET: RegistroVehiculosController
        public async Task<ActionResult> Index()
        {
            var lista = await _iservicioRegistrarVehiculos.Get();
            return View(lista);


        }

        // GET: RegistroVehiculosController/Buscar/5
        public ActionResult Buscar()
        {
            return View();
        }
        // POST: RegistroVehiculosController/Buscar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Buscar(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
            {
                ModelState.AddModelError("", "Debe ingresar una placa.");
                return View();
            }

            var vehiculoEncontrado = await _iservicioRegistrarVehiculos.Buscar(placa);

            if (vehiculoEncontrado == null)
            {
                ModelState.AddModelError("", "No se encontró un vehículo con esa placa.");
                return View();
            }

            return View("BuscarVehiculo", vehiculoEncontrado);
        }
        // GET: RegistroVehiculosController/Create
        public async Task<ActionResult> Create()
        {
           
            var clientes = await _servicioCliente.Get();

            ViewBag.ListaClientes = new SelectList(clientes, "Id", "Nombre");

            return View();
        }

        // POST: RegistroVehiculosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegistroVehiculos vehiculoNuevo)
        {
            if (!ModelState.IsValid)
                return View(vehiculoNuevo);

            var exito = await _iservicioRegistrarVehiculos.Create(vehiculoNuevo);

            if (!exito)
            {
                ModelState.AddModelError("", "No se pudo crear el vehículo.");
                return View(vehiculoNuevo);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: RegistroVehiculosController/Edit/5
        public async Task<ActionResult> Edit(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return BadRequest();

            var vehiculo = await _iservicioRegistrarVehiculos.Buscar(placa);
            if (vehiculo == null)
                return NotFound();

            return View(vehiculo);
        }

        // POST: RegistroVehiculosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string placaOriginal, RegistroVehiculos vehiculoEditado)
        {
            if (!ModelState.IsValid)
                return View(vehiculoEditado);

            var exito = await _iservicioRegistrarVehiculos.Editar(vehiculoEditado, placaOriginal);
            if (!exito)
            {
                ModelState.AddModelError("", "No se pudo editar el vehículo. Verifica que la nueva placa no exista.");
                return View(vehiculoEditado);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: RegistroVehiculosController/Delete/5
        public async Task<ActionResult> Delete(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return NotFound();

            var vehiculo = await _iservicioRegistrarVehiculos.Buscar(placa);
            if (vehiculo == null)
                return NotFound();

            return View(vehiculo);
        }

        // POST: RegistroVehiculosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return NotFound();

            var exito = await _iservicioRegistrarVehiculos.Eliminar(placa);
            if (!exito)
            {
                ModelState.AddModelError("", "No se pudo eliminar el vehículo.");
                var vehiculo = await _iservicioRegistrarVehiculos.Buscar(placa);
                return View(vehiculo);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
