using System.Text;
using _03075.Proyecto_1.SusanMurillo.Models;
using Newtonsoft.Json;

namespace _03075.Proyecto_1.SusanMurillo.Servicios
{
    public class ServicioCliente : IServicioCliente
    {
        private string _Baseurl;
        public ServicioCliente()
        {
            _Baseurl = "http://localhost:5185/";
        }
        public async Task<Cliente> Buscar(int id)
        {
            Cliente cliente= new Cliente();
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);

            var response = await httpClient.GetAsync($"api/Cliente/ObtenerCliente/{ id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                cliente = JsonConvert.DeserializeObject<Cliente>(json);
            }

            return cliente;
        }

        public async Task<bool> Create(Cliente obj_cliente)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri(_Baseurl);

                var json = JsonConvert.SerializeObject(obj_cliente);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await cliente.PostAsync("api/Cliente/Agregar", contenido);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> Editar(Cliente clienteEditado)
        {
            var clienteHttp = new HttpClient();
            clienteHttp.BaseAddress = new Uri(_Baseurl);

            var contenido = new StringContent(
                JsonConvert.SerializeObject(clienteEditado),
                Encoding.UTF8,
                "application/json"
            );

            var response = await clienteHttp.PutAsync($"api/Cliente/Editar", contenido);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> Eliminar(int id)
        {
            var clienteHttp = new HttpClient();
            clienteHttp.BaseAddress = new Uri(_Baseurl);

            var response = await clienteHttp.DeleteAsync($"api/Cliente/Eliminar/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Cliente>> Get()
        {
            List<Cliente> lista = new List<Cliente>();
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_Baseurl);
            var response = await cliente.GetAsync("api/Cliente/ObtengaLaLista");
            if (response.IsSuccessStatusCode)
            {
                var Json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Cliente>>(Json_respuesta);
                lista = resultado;
            }

            return lista;
        }
    }
}
