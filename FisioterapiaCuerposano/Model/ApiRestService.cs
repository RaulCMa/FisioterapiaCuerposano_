using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FisioterapiaCuerposano.Model
{
    public class ApiRestService
    {
        internal ObservableCollection<Cita> GetCitas()
        {
            DateTime now = DateTime.Now;
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            //var request = new RestRequest("Cita/Fecha/?desde="+now.Year + "-" + now.Month + "-" + now.Day + 7, Method.GET);
            var request = new RestRequest("Cita/realizada/0", Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ObservableCollection<Cita>>(response.Content);
        }

        internal ObservableCollection<Cliente> GetClientes()
        {
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            var request = new RestRequest("Cliente", Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ObservableCollection<Cliente>>(response.Content);
        }

        internal IRestResponse PutCitaAceptada(Cita citaAAceptar)
        {
            Cita cita = new Cita(citaAAceptar.IdCita, citaAAceptar.IdCliente, citaAAceptar.Fecha, 1, citaAAceptar.Realizada);
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            var request = new RestRequest($"Cita/idcita/{cita.IdCita}", Method.PUT);
            string data = JsonConvert.SerializeObject(cita);
            MessageBox.Show(citaAAceptar.toString());
            
            MessageBox.Show(JsonConvert.SerializeObject(cita));
            string dataa = "{\"IdCita\":" + citaAAceptar.IdCita + ",\"IdCliente\":" + citaAAceptar.IdCliente + ",\"Fecha\":" + citaAAceptar.Fecha + "\",\"Aceptada\":1,\"Realizada\":0}";
            MessageBox.Show(data);
            request.AddParameter("application/json", dataa, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        internal IRestResponse PutCitaRealizada(Cita citaARealizar)
        {
            citaARealizar.Realizada = 1;
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            var request = new RestRequest($"Cita/idCita/{citaARealizar.IdCita}", Method.PUT);
            
            string data = JsonConvert.SerializeObject(citaARealizar);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        internal IRestResponse DeleteCita(int idCitaABorrar)
        {
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            var request = new RestRequest($"Cita/{idCitaABorrar}", Method.DELETE);
            var response = client.Execute(request);
            return response;
        }

        internal Cita GetCita(int idCita)
        {
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            var request = new RestRequest($"Cita/idCita/{idCita}", Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ObservableCollection<Cita>>(response.Content).First();
        }

        internal Cliente getCliente(int idCliente)
        {
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            var request = new RestRequest($"/Cliente/idCliente/{idCliente}", Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ObservableCollection<Cliente>>(response.Content).First();
        }

        internal int getClienteCount(string dni)
        {
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            var request = new RestRequest($"/Cliente/dni/{dni}", Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ObservableCollection<Cliente>>(response.Content).Count();
        }

        internal IRestResponse PostCliente(Cliente nuevoCliente)
        {
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            var request = new RestRequest("Cliente", Method.POST);
            string data = JsonConvert.SerializeObject(nuevoCliente);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }
        internal IRestResponse PostCita(Cita nuevaCita)
        {
            var client = new RestClient(Properties.Settings.Default.apiEndPoint);
            var request = new RestRequest("Cita", Method.POST);
            string data = JsonConvert.SerializeObject(nuevaCita);
            MessageBox.Show(data);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        internal IRestResponse PostClienteFireBase(Cliente nuevoCliente)
        {
            var client = new RestClient(Properties.Settings.Default.apiKeyFireBaseEndPoint);
            var request = new RestRequest("", Method.POST);
            string data = "{\"Email\":" + nuevoCliente.Correo + "\",\"password\":" + nuevoCliente.Contraseña + "\",\"returnSecureToken\":true}";
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }
    }
}
