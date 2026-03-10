using System.Text;
using _03075.Proyecto_1.SusanMurillo.Models;
using Newtonsoft.Json;

namespace _03075.Proyecto_1.SusanMurillo.Servicios
{
    public class ServicioEmpleados : IServicioEmpleados
    {
        private string _Baseurl;

        public ServicioEmpleados()
        {
            _Baseurl = "http://localhost:5185/";
        }

        public async Task<Empleados> Buscar(string cedula)
        {
            Empleados empleado = new Empleados();
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);

            var response = await httpClient.GetAsync($"api/Empleado/ObtenerEmpleado/{cedula}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                empleado = JsonConvert.DeserializeObject<Empleados>(json);
            }

            return empleado;
        }

        public async Task<bool> Editar(Empleados empleadoEditado, string cedulaOriginal)
        {
            var clienteHttp = new HttpClient();
            clienteHttp.BaseAddress = new Uri(_Baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(empleadoEditado), Encoding.UTF8, "application/json");

            var response = await clienteHttp.PutAsync($"api/Empleado/Editar", contenido);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(string cedula)
        {
            var clienteHttp = new HttpClient();
            clienteHttp.BaseAddress = new Uri(_Baseurl);

            var response = await clienteHttp.DeleteAsync($"api/Empleado/Eliminar/{cedula}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Empleados>> Get()
        {
           List<Empleados> lista = new List<Empleados>();
            var empleado = new HttpClient();
            empleado.BaseAddress=new Uri(_Baseurl);
            var response = await empleado.GetAsync("api/Empleado/ObtengaLaLista");
            if (response.IsSuccessStatusCode) 
                {  var Json_respuesta=await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Empleados>>(Json_respuesta);
                lista = resultado;
            }

            return lista;
        }
        public async Task<bool> Create(Empleados empleadonuevo)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri(_Baseurl);

                var json = JsonConvert.SerializeObject(empleadonuevo);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await cliente.PostAsync("api/Empleado/Agregar", contenido);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<List<EmpleadoDTO>> ObtenerEmpleados()
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);
            var response = await httpClient.GetAsync("api/Empleado/EmpleadosDisponibles");

            if (!response.IsSuccessStatusCode)
                return new List<EmpleadoDTO>();

            var json = await response.Content.ReadAsStringAsync();
            var lista = JsonConvert.DeserializeObject<List<EmpleadoDTO>>(json);

            return lista;
        }

    }
}
