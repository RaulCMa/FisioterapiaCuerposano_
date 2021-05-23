using FisioterapiaCuerposano.Model;
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
    /// Lógica de interacción para clienteVista.xaml
    /// </summary>
    public partial class clienteVista : Window
    {
        public clienteVista(Cliente c)
        {
            InitializeComponent();
            StackPanelConjunto.DataContext = c;
        }

        private void CerrarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
