using System.Net.Http;
using System.Text;
using _03075.Proyecto_1.SusanMurillo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace _03075.Proyecto_1.SusanMurillo.Servicios
{
    public class ServicioLavado : IServicioLavado
    {

        private string _Baseurl;
        

        public ServicioLavado()
        {
            _Baseurl = "http://localhost:5185/";
        }


        public async Task<Lavado> Buscar(int id)
        {
            Lavado lavado = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_Baseurl);
                var response = await httpClient.GetAsync($"api/Lavado/ObtenerRegsitro/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    lavado = JsonConvert.DeserializeObject<Lavado>(json);
                }
               
            }
            return lavado;
        }

        public async Task<(bool Exito, string Error)> Create(Lavado obj_lavado)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(obj_lavado), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/Lavado/Agregar", contenido);

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Respuesta API: " + responseBody);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error al crear lavado: " + responseBody);
                return (false, responseBody);
            }

            return (true, null);
        }

        public async Task<bool> Editar(Lavado obj_lavado, int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_Baseurl);
                var contenido = new StringContent(JsonConvert.SerializeObject(obj_lavado), Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"api/Lavado/Editar", contenido);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Error al editar lavado: " + error);
                    return false;
                }
               
                return true;
            }
        }
        public async Task<List<PlacaClienteDTO>> ObtenerPlacasConClientes()
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);
            var response = await httpClient.GetAsync("api/Lavado/ClientesDisponibles");

            if (!response.IsSuccessStatusCode)
                return new List<PlacaClienteDTO>();

            var json = await response.Content.ReadAsStringAsync();
            var lista = JsonConvert.DeserializeObject<List<PlacaClienteDTO>>(json);

            return lista;
        }
        public async Task<bool> Eliminar(int id)
        {
            var clienteHttp = new HttpClient();
            clienteHttp.BaseAddress = new Uri(_Baseurl);

            var response = await clienteHttp.DeleteAsync($"api/Lavado/Eliminar/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Lavado>> Get()
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);

            var response = await httpClient.GetAsync("api/Lavado/ObtengaLaLista");
            if (!response.IsSuccessStatusCode)
                return new List<Lavado>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Lavado>>(json);
        }
        public async Task<List<ClienteReporteDTO>> GetClientesPorContactar()
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);

            var response = await httpClient.GetAsync("api/Lavado/ClientesPorContactar");
            if (!response.IsSuccessStatusCode)
                return new List<ClienteReporteDTO>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ClienteReporteDTO>>(json);
        }
    }
}
