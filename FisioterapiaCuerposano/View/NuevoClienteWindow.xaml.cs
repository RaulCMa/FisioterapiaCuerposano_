//using FirebaseAdmin;
//using FirebaseAdmin.Auth;
using FisioterapiaCuerposano.Model;
//using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FisioterapiaCuerposano.View
{
    /// <summary>
    /// Lógica de interacción para NuevoClienteWindow.xaml
    /// </summary>
    public partial class NuevoClienteWindow : Window
    {
        //string urlFB = "https://console.firebase.google.com/u/0/project/fisioterapiacuerposano/authentication/users?hl=es";

        public NuevoClienteWindow()
        {
            InitializeComponent();
        }

        private void ValidarButton_Click(object sender, RoutedEventArgs e)
        {
            //nuevoCliente.Nombre = NombreNuevoCliente.Text;
            //nuevoCliente.Apellidos = ApellidosNuevoCliente.Text;
            //nuevoCliente.Dni = dniNuevoCliente.Text;
            //nuevoCliente.Telefono = Int32.Parse(TelefonoNuevoCliente.Text);
            //nuevoCliente.FechaNacimiento = /*DateTime.Parse(^*/fechaNacimientoNuevoCliente.Text.ToString();
            //nuevoCliente.Correo = correoNuevoCliente.Text;
            //nuevoCliente.Contraseña = dniNuevoCliente.Text;
            //nuevoCliente.PrimeraEntrada = 0;
            //MessageBox.Show(nuevoCliente.FechaNacimiento/*.ToShortDateString()*/);

            Cliente nuevoCliente = new Cliente(NombreNuevoCliente.Text, ApellidosNuevoCliente.Text, dniNuevoCliente.Text, Int32.Parse(TelefonoNuevoCliente.Text), fechaNacimientoNuevoCliente.Text.ToString(),
                                    correoNuevoCliente.Text, dniNuevoCliente.Text, 0, "");

            Console.Write(nuevoCliente.Dni);

            string patata = nuevoCliente.toString();

            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.FromFile("Resources/fisioterapiacuerposano-firebase-adminsdk-4wy3x-87767b10ef.json"),
            //});


            //if (nuevoCliente == null)
            //{
            //    MessageBox.Show("Error al añadir el cliente.");
            //} else {
            // Se ha añadido + user en firebase añadido correcto
            if (ViewModel.AdministrarVM.getCantidadClientesConDni(nuevoCliente.Dni) == 0)
            {
                ViewModel.AdministrarVM.PostCliente(nuevoCliente);
                //CreateUserAsync(nuevoCliente);
                MessageBox.Show("Cliente añadido", "Nuevo cliente", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else MessageBox.Show("Ya existe un cliente con Dni " + nuevoCliente.Dni);
            //}
        }



        //internal static async Task CreateUserAsync(Cliente nuevoCliente)
        //{
        //    // [START create_user]
        //    UserRecordArgs args = new UserRecordArgs()
        //    {
        //        Email = nuevoCliente.Correo,
        //        EmailVerified = false,
        //        PhoneNumber = nuevoCliente.Telefono.ToString(),
        //        Password = nuevoCliente.Contraseña,
        //        DisplayName = nuevoCliente.Nombre,
        //        PhotoUrl = "",
        //        Disabled = false,
        //    };
        //    UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
        //    // See the UserRecord reference doc for the contents of userRecord.
        //    Console.WriteLine($"Successfully created new user: {userRecord.Uid}");
        //    // [END create_user]
        //}
    }
}
