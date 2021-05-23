using FisioterapiaCuerposano.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web.Http;
using System.Configuration;

namespace FisioterapiaCuerposano.View
{
    /// <summary>
    /// Lógica de interacción para ElectionWindow.xaml
    /// </summary>
    public partial class CitasWindow : Window
    {
        ObservableCollection<Cita> citas = new ObservableCollection<Cita>();

        //List<Cita> citas = new List<Cita>();
        Cita cita = new Cita();
        Cliente c = new Cliente();

        FirebaseApp fisio;

        string path = "./resources/fisioterapiacuerposano-firebase-adminsdk-4wy3x-de4bd57ca1.json";

        public static Model.ApiRestService _apiRest;

        public static string Api_Web_Key = "AIzaSyAyMHC1Ji---BX9RQMPkOCzVE7zHIBlizc";
        public static string ClaveDelServidor = "AAAAG_kc2KE:APA91bE4ooM-rpQS9z8uXdoERsfCGEXLrYmIqPYuDOoK9_-JScz3uYnKsybQlgtdzZuOkTcpL4m_eEtcxWrXLAJqpXKjJ3wBmZut0XaIjdLjr-QEtvJycG4wRpcYiNhmNoes1kSiakza";
        public static string par_De_Clave_Publica = "BK_0B-g2PlMrqQAV5q8t8oicOqkxm9tkoH11S9nTndNKJ4ycNnbJg3Azc20lRDVs1wo7pJ14_r1WlcazFyJHUqo";
        public static string Numero_De_Proyecto = "120143534241";
        public static string Sender_Id = "120143534241";
        public static string userToken = "Rr4MMgmj3xVKRFsfFFehXXYDJG22";


        public CitasWindow()
        {
            InitializeComponent();

            ActualizaCitas();

            


            //do
            //{
            //    Thread.Sleep(2000);
            //    ActualizaCitas();
            //} while (true);
        }

        private void AnyadirClienteButtonClick(object sender, RoutedEventArgs e)
        {
            View.NuevoClienteWindow hija = new View.NuevoClienteWindow();

            hija.Owner = this;

            hija.ShowInTaskbar = false;
            hija.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            hija.Show();
        }

        private void AceptarCitaButton_Click(object sender, RoutedEventArgs e)
        {
            Cita c = (Cita)ContenedorCitas.SelectedItem;
            // CONSULTA PARA REALIZADA DE ESTA CITA ACEPTADA = TRUE
            //ViewModel.AdministrarVM.borrarCita(cita.IdCita);
            //Cita nuevaCita = new Cita(cita.IdCita, cita.IdCliente, cita.Fecha, 1, 0);
            //ViewModel.AdministrarVM.PostCita(nuevaCita);
            //MessageBox.Show("La cita " + ((Cita)ContenedorCitas.SelectedItem).IdCita + " ha sido ACEPTADA.");
            ViewModel.AdministrarVM.cambiarAceptadaCita(c);

            //ActualizaCitas();
        }

        private void EliminarCitaButton_Click(object sender, RoutedEventArgs e)
        {
            // CONSULTA PARA DELETE DE ESTA CITA ACEPTADA = TRUE

            cita = (Cita)ContenedorCitas.SelectedItem;
            ViewModel.AdministrarVM.borrarCita(((Cita)ContenedorCitas.SelectedItem).IdCita);
            //MessageBox.Show("La cita " + ((Cita)ContenedorCitas.SelectedItem).IdCita + " ha sido ELIMINADA.");
            ActualizaCitas();
        }

        private void RealizarCitaButton_Click(object sender, RoutedEventArgs e)
        {
            Cita c = (Cita)ContenedorCitas.SelectedItem;
            // CONSULTA PARA REALIZADA DE ESTA CITA ACEPTADA = TRUE
            //ViewModel.AdministrarVM.borrarCita(cita.IdCita);
            cita.Aceptada = 1;
            //cita = ((Cita)ContenedorCitas.SelectedItem);
            ViewModel.AdministrarVM.cambiarRealizadaCita((Cita)ContenedorCitas.SelectedItem);
            //MessageBox.Show("La cita " + ((Cita)ContenedorCitas.SelectedItem).IdCita + " ha sido REALIZADA.");
            ActualizaCitas();
        }

        private void Vista_Filter(object sender, FilterEventArgs e)
        {
            Cita item = (Cita)e.Item;

            if (EscogerDiaTextBox.Text == "")
                e.Accepted = true;
            else
            {
                
                string fechad = item.Fecha.Day + "" + item.Fecha.Month + "" + item.Fecha.Year;
                if (item.Fecha.ToShortDateString().Equals(DateTime.Parse(EscogerDiaTextBox.Text).ToShortDateString()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void BuscarDiaButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateEscogida = DateTime.Parse(EscogerDiaTextBox.Text);
            if (dateEscogida.ToString() != "")
            {
                ObservableCollection<Cita> citas2 = new ObservableCollection<Cita>();
                foreach (Cita c in citas)
                {
                    if (c.Fecha.Day == dateEscogida.Day && c.Fecha.Month == dateEscogida.Month && c.Fecha.Year == dateEscogida.Year)
                        citas2.Add(c);
                    citas2 = new ObservableCollection<Cita>(citas2.OrderBy(x => x.Fecha));
                    ContenedorCitas.ItemsSource = citas2;
                }
            }
            else MessageBox.Show("No es el formato correcto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void informacionButton_Click(object sender, RoutedEventArgs e)
        {
            c = ViewModel.AdministrarVM.getCliente(ViewModel.AdministrarVM.getCita(((Cita)ContenedorCitas.SelectedItem).IdCita).IdCliente);

            View.clienteVista hija = new View.clienteVista(c);
            hija.Owner = this;
            hija.ShowInTaskbar = false;
            hija.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            hija.Show();

        }

        private void ActualizarButton_Click(object sender, RoutedEventArgs e)
        {
            ActualizaCitas();
        }

        private void ActualizaCitas()
        {
            this.DataContext = new ViewModel.AdministrarVM();
            citas = (this.DataContext as ViewModel.AdministrarVM).citas;
            citas = new ObservableCollection<Cita>(citas.OrderBy(x => x.Fecha));
            //foreach(Cita ccc in citas)
            //{
            //    ccc.Fecha = ccc.Fecha.ToLocalTime();
            //}
            ContenedorCitas.ItemsSource = citas;
        }

        private void LimpiarBusquedaButton_Click(object sender, RoutedEventArgs e)
        {
            EscogerDiaTextBox.Text = "";
            ActualizaCitas();
        }

        public void SendMessage()
        {
            var data = new
            {
                to = "fpz9X-lCRt2EhTDvkzUDsj:APA91bEl4vUNjD1vYGKzAWab0as5bZRZgqmBqrAmQ0nmDo1YfhCy4fqvSRfQPi67kR1DMBBGgbqg1omGtoWIUup3Ip8gJ0MjX42FrxrJjj7Rcj23JtdOUQUBEnloS3ujnWVB2Y1iyA4n",
                data = new
                {
                    Title ="test",
                    Body = "test"
                }
            };


            var json = JsonConvert.SerializeObject(data);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            try 
            { 
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add($"Authorization: key={"AAAAG_kc2KE:APA91bE4ooM-rpQS9z8uXdoERsfCGEXLrYmIqPYuDOoK9_-JScz3uYnKsybQlgtdzZuOkTcpL4m_eEtcxWrXLAJqpXKjJ3wBmZut0XaIjdLjr-QEtvJycG4wRpcYiNhmNoes1kSiakza"}");
                tRequest.Headers.Add($"Sender: id={120143534241}");

                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tresponse = tRequest.GetResponse();
                dataStream = tresponse.GetResponseStream();
                StreamReader treader = new StreamReader(dataStream);

                string sResponseFromServer = treader.ReadToEnd();

                treader.Close();
                dataStream.Close();
                tresponse.Close();

                MessageBox.Show(json.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR");
            }
        }

        //private void sendNotification(object data)
        //{
        //    var json = JsonConvert.SerializeObject(data);
        //    Byte[] byteArray = Encoding.UTF8.GetBytes(json);

        //    sendNotification2(byteArray);
        //}

        //private void sendNotification2(byte[] byteArray)
        //{
        //    try
        //    {
        //        //string server_api_key = ConfigurationManager.AppSettings["SERVER_API_KEY"];
        //        //string sender_id = ConfigurationManager.AppSettings["SENDER_ID"];

        //        WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //        tRequest.Method = "post";
        //        tRequest.ContentType = "application/json";
        //        tRequest.Headers.Add($"Authorization: key={ClaveDelServidor}");
        //        tRequest.Headers.Add($"Sender: id={Sender_Id}");

        //        tRequest.ContentLength = byteArray.Length;
        //        Stream dataStream = tRequest.GetRequestStream();
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();

        //        WebResponse tresponse = tRequest.GetResponse();
        //        dataStream = tresponse.GetResponseStream();
        //        StreamReader treader = new StreamReader(dataStream);

        //        string sResponseFromServer = treader.ReadToEnd();

        //        treader.Close();
        //        dataStream.Close();
        //        tresponse.Close();
        //    }
        //    catch(Exception e)
        //    {
        //        MessageBox.Show("ERROR");
        //    }
        //}

        //private async Task notificacionButton_ClickAsync(object sender, RoutedEventArgs e)
        //{
        //    FirebaseApp.Create(new AppOptions()
        //    {
        //        Credential = GoogleCredential.GetApplicationDefault()
        //    });

        //    // This registration token comes from the client FCM SDKs.
        //    var registrationToken = "YOUR_REGISTRATION_TOKEN";

        //    // See documentation on defining a message payload.
        //    var message = new Message()
        //    {
        //        Data = new Dictionary<string, string>()
        //            {
        //                { "score", "850" },
        //                { "time", "2:45" },
        //            },
        //        Token = registrationToken,
        //    };

        //    // Send a message to the device corresponding to the provided
        //    // registration token.
        //    string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        //    // Response is a message ID string.
        //    Console.WriteLine("Successfully sent message: " + response);
        //}

        private void notificacionButton_Click(object sender, EventArgs e)
        {
            Cita cita = ((Cita)ContenedorCitas.SelectedItem);
            Cliente cliente = ViewModel.AdministrarVM.getCliente(cita.IdCliente);

            string BrowserAPIKey = "AAAAG_kc2KE:APA91bE4ooM-rpQS9z8uXdoERsfCGEXLrYmIqPYuDOoK9_-JScz3uYnKsybQlgtdzZuOkTcpL4m_eEtcxWrXLAJqpXKjJ3wBmZut0XaIjdLjr-QEtvJycG4wRpcYiNhmNoes1kSiakza";

            string message = "Tienes una cita hoy.";
            string contentTitle = "Entra para ver a que hora.";
            MessageBox.Show(cliente.toString());
            string client = cliente.UidToken;
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

        //private async void notificacionButton_Click(object sender, RoutedEventArgs e)
        //{

        //    //FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(fisio);

        //    cita = (Cita)ContenedorCitas.SelectedItem;
        //    Cliente c = (ViewModel.AdministrarVM.getCliente(cita.IdCliente));

        //    SendMessage();

        //    //SendNotificationFromFirebaseCloud();

        //    //MessageBox.Show(messaging.ToString());



        //    //// This registration token comes from the client FCM SDKs.
        //    //var registrationToken = "MS4AsSB9qPQcdostoyM2hf25KVY2";

        //    //// See documentation on defining a message payload.
        //    //Message message = new Message()
        //    //{
        //    //    Data = new Dictionary<string, string>()
        //    //    {
        //    //        { "score", "850" },
        //    //        { "time", "2:45" },
        //    //    },
        //    //    Token = registrationToken,
        //    //};


        //    //message.Notification.Title="testc";
        //    //message.Notification.Body="testc";


        //    //// Send a message to the device corresponding to the provided
        //    //// registration token.
        //    //string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        //    //// Response is a message ID string.
        //    //MessageBox.Show(response);



        //    //Message m = new Message()
        //    //{

        //    //};
        //    //m.Token = "MS4AsSB9qPQcdostoyM2hf25KVY2";
        //    //m.Android = new AndroidConfig();

        //    //m.FcmOptions = new FcmOptions();
        //    //Notification nnn = new Notification();
        //    //nnn.Body = "Test";
        //    //nnn.Title = "Test";


        //    //m.Notification = nnn;
        //    //MessageBox.Show("" + m.Token.ToString() + " " + m.Notification.Title.ToString() + " " + m.Notification.Body);

        //    //await messaging.SendAsync(m);

        //    //try
        //    //{
        //    //    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //    //    tRequest.Method = "POST";
        //    //    tRequest.Headers.Add("Authorization", $"Key={"AIzaSyAyMHC1Ji---BX9RQMPkOCzVE7zHIBlizc"}");
        //    //    //tRequest.Headers.Add(string.Format($"Sender: id={120143534241}"));
        //    //    tRequest.ContentType = "application/json";

        //    //    var payload = new
        //    //    {
        //    //        to = "MS4AsSB9qPQcdostoyM2hf25KVY2",
        //    //        priority = "high",
        //    //        content_available = true,
        //    //        notification = new
        //    //        {
        //    //            body = "Test",
        //    //            title = "Test"
        //    //        },
        //    //        data = new
        //    //        {
        //    //            key1 = "value1",
        //    //            key2 = "value2"
        //    //        }
        //    //    };
        //    //    string postBody = JsonConvert.SerializeObject(payload).ToString();
        //    //    Byte[] byteArray = Encoding.UTF8.GetBytes(postBody);
        //    //    tRequest.ContentLength = byteArray.Length;
        //    //    using (Stream dataStream = tRequest.GetRequestStream())
        //    //    {
        //    //        dataStream.Write(byteArray, 0, byteArray.Length);
        //    //        using (WebResponse tResponse = tRequest.GetResponse())
        //    //        {
        //    //            using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //    //            {
        //    //                if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //    //                    {
        //    //                        String sRensponseFromServer = tReader.ReadToEnd();
        //    //                    }
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show(ex.ToString(), "Excepcio");
        //    //}

        //}

        //static void SendMessage()
        //{
        //    string serverKey = "AAAAG_kc2KE:APA91bE4ooM-rpQS9z8uXdoERsfCGEXLrYmIqPYuDOoK9_-JScz3uYnKsybQlgtdzZuOkTcpL4m_eEtcxWrXLAJqpXKjJ3wBmZut0XaIjdLjr-QEtvJycG4wRpcYiNhmNoes1kSiakza";

        //    try
        //    {
        //        var result = "-1";
        //        var webAddr = "https://fcm.googleapis.com/fcm/send";

        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
        //        httpWebRequest.Method = "POST";

        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            string json = "{\"to\": \"MS4AsSB9qPQcdostoyM2hf25KVY2\",\"data\": {\"message\": \"This is a Firebase Cloud Messaging Topic Message!\",}}";
        //            streamWriter.Write(json);
        //            streamWriter.Flush();
        //        }

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            result = streamReader.ReadToEnd();
        //        }
        //        MessageBox.Show(result);
        //        // return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        public static String SendNotificationFromFirebaseCloud()
        {
            string serverKey = "AAAAG_kc2KE:APA91bE4ooM-rpQS9z8uXdoERsfCGEXLrYmIqPYuDOoK9_-JScz3uYnKsybQlgtdzZuOkTcpL4m_eEtcxWrXLAJqpXKjJ3wBmZut0XaIjdLjr-QEtvJycG4wRpcYiNhmNoes1kSiakza";

            var result = "";
            var webAddr = "https://fcm.googleapis.com/fcm/send";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization:key = " + serverKey);
            httpWebRequest.Method = "POST";

            string token = "MS4AsSB9qPQcdostoyM2hf25KVY2";
            string Your_Notif_Title = "test";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"to\":\"" + token + "\",\"notification\": {\"body\": \"New news added in application!\",\"title\":\"" + Your_Notif_Title + "\",}}";
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            MessageBox.Show(result);
            return result;
        }
    }
}
