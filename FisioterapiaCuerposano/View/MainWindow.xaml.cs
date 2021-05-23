//using MySql.Data.MySqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FisioterapiaCuerposano
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string passWord = "Patata";
        //MySqlConnection mySqlConnection = new MySqlConnection("Server=localhost; Database=balmis_pdam04b; Uid=balmis_pdam04b: Pwd=asdf1234");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Registrar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (passWord == Password_Box.Password.ToString())
            {
                //try
                //{
                //    mySqlConnection.Open();
                    MessageBox.Show("Contraseña correcta.\nConectados");
                //    mySqlConnection.Close();
                //}
                //catch (Exception)
                //{

                //MessageBox.Show("Contraseña correcta.\nError:" + e.ToString());
                //    throw;
                //}
                Password_Box.Clear();

                View.CitasWindow hija = new View.CitasWindow();

                hija.Owner = this;

                hija.ShowInTaskbar = true;
                hija.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                hija.Show();
                

            }
            else
            {
                MessageBox.Show("Contraseña incorrecta.");
            }
        }
    }
}
