using FisioterapiaCuerposano.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FisioterapiaCuerposano.ViewModel
{
    public class AdministrarVM
    {
        public ObservableCollection<Cita> citas;
        public ObservableCollection<Cliente> clientes;
        public static Cita cita = new Cita();
        public static Cliente cliente = new Cliente();
        public static Model.ApiRestService _apiRest = new ApiRestService();

        public AdministrarVM()
        {
            citas = _apiRest.GetCitas();
        }

        public static void cambiarAceptadaCita(Cita c)
        {
            c.Aceptada=1;
            _apiRest.PutCitaAceptada(c);
            MessageBox.Show("Se ha cambiado la cita " + c.IdCita, "Cambio cita", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void cambiarRealizadaCita(Cita c)
        {
            _apiRest.PutCitaRealizada(c);
            MessageBox.Show("Se ha cambiado la cita " + c.IdCita, "Cambio cita", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void borrarCita(int idCita)
        {
            _apiRest.DeleteCita(idCita);
            MessageBox.Show("Se ha borrado la cita " + idCita, "Cita borrada", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static Cita getCita(int idCita)
        {
            return cita = _apiRest.GetCita(idCita);
        }

        public static int getClientesCount(string dni)
        {
            return _apiRest.getClienteCount(dni);
        }

        public static void PostCliente(Cliente cliente)
        {
            _apiRest.PostCliente(cliente);
            MessageBox.Show("Se ha añadido correctamente el nuevo cliente.", "Cliente nuevo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static void PostCita(Cita cita)
        {
            _apiRest.PostCita(cita);
            MessageBox.Show("Se ha cambiado correctamente la cita.", "Cita cambiada", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        internal static Cliente getCliente(int idCliente)
        {
            return _apiRest.getCliente(idCliente);
        }

        internal static Cliente getCliente(string dni)
        {
            return _apiRest.getClienteDni(dni);
        }

        internal static int getCantidadClientesConDni(string dni)
        {
            int contador = 0;
            ObservableCollection<Cliente> clientes = _apiRest.GetClientes();
            for (int i = 0; i < clientes.Count; i++)
            {
                cliente = clientes[i];
                if (cliente.Dni.Equals(dni)) contador++;
            }
            return contador;
        }

        public static void Notificacion(string uidToken)
        {
            string BrowserAPIKey = "AAAAG_kc2KE:APA91bE4ooM-rpQS9z8uXdoERsfCGEXLrYmIqPYuDOoK9_-JScz3uYnKsybQlgtdzZuOkTcpL4m_eEtcxWrXLAJqpXKjJ3wBmZut0XaIjdLjr-QEtvJycG4wRpcYiNhmNoes1kSiakza";

            string message = "Tienes una cita hoy.";
            string contentTitle = "Entra para ver a que hora.";
            string client = uidToken;
            //string client = "fpz9X-lCRt2EhTDvkzUDsj:APA91bEl4vUNjD1vYGKzAWab0as5bZRZgqmBqrAmQ0nmDo1YfhCy4fqvSRfQPi67kR1DMBBGgbqg1omGtoWIUup3Ip8gJ0MjX42FrxrJjj7Rcj23JtdOUQUBEnloS3ujnWVB2Y1iyA4n";
            //string postData = "{ \"registration_id\": [ \"" + "MS4AsSB9qPQcdostoyM2hf25KVY2" + "\" ], \"data\": {\"tickerText\":\"" + tickerText + "\", \"contentTitle\":\"" + contentTitle + "\", \"message\": \"" + message + "\"}}";

            var payload = new
            {
                to = client,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = contentTitle,
                    title = message
                }
                //date = new
                //{
                //    key1 = "value1",
                //    key2 = "value2"
                //}
            };
            string postData = JsonConvert.SerializeObject(payload).ToString();

            string response = SendGCMNotification(BrowserAPIKey, postData);
        }

        private static string SendGCMNotification(string apiKey, string postData, string postDataContentType = "application/json")
        {
            // from here:
            // https://stackoverflow.com/questions/11431261/unauthorized-when-calling-google-gcm
            //
            // original:
            // http://www.codeproject.com/Articles/339162/Android-push-notification-implementation-using-ASP

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //
            //  MESSAGE CONTENT
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //
            //  CREATE REQUEST
            WebRequest Request = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

            Request.Method = "POST";
            Request.ContentType = postDataContentType;
            //Request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
            Request.Headers.Add("Authorization", String.Format("key={0}", apiKey));
            Request.ContentLength = byteArray.Length;

            //Stream dataStream;
            try
            {
                Stream dataStream = Request.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            //
            //  SEND MESSAGE
            try
            {
                WebResponse Response = Request.GetResponse();

                HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;

                if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                {
                    MessageBox.Show("Unauthorized - need new token");

                }
                else if (!ResponseCode.Equals(HttpStatusCode.OK))
                {
                    MessageBox.Show("Response from web service isn't OK");
                }

                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                string responseLine = Reader.ReadToEnd();
                Reader.Close();

                return responseLine;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return "error";
        }


        public static bool ValidateServerCertificate(
                                                  object sender,
                                                  X509Certificate certificate,
                                                  X509Chain chain,
                                                  SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
