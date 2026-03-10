using _03075.Proyecto_1.SusanMurillo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using _03075.Proyecto_1.SusanMurillo.Servicios;
namespace _03075.Proyecto_1.SusanMurillo.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IServicioEmpleados _iservicioEmpleados;
        private static IList<Empleados> listaDeEmpleados = new List<Empleados>();


        public EmpleadosController(IServicioEmpleados iservicioEmpleados)
        {
            _iservicioEmpleados = iservicioEmpleados;

        }
        // GET: EmpleadosController
        public async Task<ActionResult> Index()

        {
            var empleados = await _iservicioEmpleados.Get();
            return View(empleados);
        }

        // GET: EmpleadosController/Buscar/5
        public ActionResult Buscar(string cedula)
        {
            return View();
        }
        // POST: EmpleadosController/Buscar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Buscar(Empleados BuscarEmpleado)
        {
            if (string.IsNullOrEmpty(BuscarEmpleado?.Cedula))
            {
                ViewBag.Mensaje = "Debe ingresar una cédula.";
                return View();
            }

            var empleadoEncontrado = await _iservicioEmpleados.Buscar(BuscarEmpleado.Cedula);

            if (empleadoEncontrado != null && !string.IsNullOrEmpty(empleadoEncontrado.Cedula))
            {
                return View("ResultadoVista", empleadoEncontrado);
            }
            else
            {
                ViewBag.Mensaje = "Empleado no encontrado";
                return View();
            }

        }
        // GET: EmpleadosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpleadosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Empleados empleadonuevo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(empleadonuevo);
                }

                var resultado = await _iservicioEmpleados.Create(empleadonuevo);

                if (resultado)
                {
                    return RedirectToAction(nameof(Index)); 
                }
                else
                {
                    ViewBag.Mensaje = "Error al guardar el empleado.";
                    return View(empleadonuevo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Ocurrió un error inesperado: " + ex.Message;
                return View(empleadonuevo);
            }
        }


        // GET: EmpleadosController/Edit/5
        public async Task<ActionResult> Edit(string cedula)
        {
            if (string.IsNullOrEmpty(cedula))
                return NotFound();

            var empleado = await _iservicioEmpleados.Buscar(cedula);

            if (empleado == null || string.IsNullOrEmpty(empleado.Cedula))
                return NotFound();

            return View(empleado);
        }

        // POST para recibir la edición
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Empleados empleadoEditado, string cedulaOriginal)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(empleadoEditado);

                var resultado = await _iservicioEmpleados.Editar(empleadoEditado, cedulaOriginal);

                if (resultado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Mensaje = "Error al editar el empleado.";
                    return View(empleadoEditado);
                }
            }
            catch
            {
                ViewBag.Mensaje = "Ocurrió un error inesperado.";
                return View(empleadoEditado);
            }
        }


        public async Task<ActionResult> Delete(string cedula)
        {
            if (string.IsNullOrEmpty(cedula))
                return NotFound();

            var empleado = await _iservicioEmpleados.Buscar(cedula);

            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string cedula)
        {
            try
            {
                var resultado = await _iservicioEmpleados.Eliminar(cedula);

                if (resultado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo eliminar el empleado.";
                    var empleado = await _iservicioEmpleados.Buscar(cedula);
                    return View("Delete", empleado);
                }
            }
            catch
            {
                ViewBag.Mensaje = "Ocurrió un error inesperado.";
                var empleado = await _iservicioEmpleados.Buscar(cedula);
                return View("Delete", empleado);
            }
        }
    }
}

