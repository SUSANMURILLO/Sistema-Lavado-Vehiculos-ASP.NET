using System.Numerics;
using System.Text;
using _03075.Proyecto_1.SusanMurillo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace _03075.Proyecto_1.SusanMurillo.Servicios
{
    public class ServicioRegistrarVehiculos : IServicioRegistrarVehiculos
    {
        private string _Baseurl;
        public ServicioRegistrarVehiculos()
        {
            _Baseurl = "http://localhost:5185/";
        }
        public async Task<RegistroVehiculos> Buscar(string placa)
        {
            RegistroVehiculos vehiculo = null;
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl); 

            var response = await httpClient.GetAsync($"api/RegistroVehiculos/ObtenerRegsitro/{placa}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                vehiculo = JsonConvert.DeserializeObject<RegistroVehiculos>(json);
            }

            return vehiculo;
        }
        public async Task<List<RegistroVehiculos>> BuscarPorCliente(string identificacion)
        {
            List<RegistroVehiculos> lista = new List<RegistroVehiculos>();
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);

            var response = await httpClient.GetAsync($"api/RegistroVehiculos/BuscarPorCliente/{identificacion}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<RegistroVehiculos>>(json);
            }

            return lista;
        }
        public async Task<bool> Create(RegistroVehiculos vehiculo)
        {
            if (vehiculo == null)
                return false;

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl); 

           
            var json = JsonConvert.SerializeObject(vehiculo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            
            var response = await httpClient.PostAsync("api/RegistroVehiculos/Agregar", content);
            
            var responseContent = await response.Content.ReadAsStringAsync();
           

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Editar(RegistroVehiculos vehiculoEditado, string placa)
        {
            if (vehiculoEditado == null || string.IsNullOrWhiteSpace(placa))
                return false;

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);

            var json = JsonConvert.SerializeObject(vehiculoEditado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("api/RegistroVehiculos/Editar", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return false;

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);

            var response = await httpClient.DeleteAsync($"api/RegistroVehiculos/Eliminar/{placa}");

            return response.IsSuccessStatusCode;
        }
        public async Task<List<RegistroVehiculos>> Get()
        {
            List<RegistroVehiculos> lista = new List<RegistroVehiculos>();
            var vehiculo = new HttpClient();
            vehiculo.BaseAddress = new Uri(_Baseurl);
            var response = await vehiculo.GetAsync("api/RegistroVehiculos/ObtengaLaLista");
            if (response.IsSuccessStatusCode)
            {
                var Json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<RegistroVehiculos>>(Json_respuesta);
                lista = resultado;
            }

            return lista;
        }

        public async Task<List<PlacaClienteDTO>> ObtenerPlacasConClientes()
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_Baseurl);
            var response = await httpClient.GetAsync("api/RegistroVehiculos/PlacasConClientes");

            if (!response.IsSuccessStatusCode)
                return new List<PlacaClienteDTO>();

            var json = await response.Content.ReadAsStringAsync();
            var lista = JsonConvert.DeserializeObject<List<PlacaClienteDTO>>(json);

            return lista;
        }


    }
}
