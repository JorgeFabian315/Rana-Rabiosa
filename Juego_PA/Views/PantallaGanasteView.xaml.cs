using Juego_PA.ViewModel;
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

namespace Juego_PA.Views
{
    /// <summary>
    /// Lógica de interacción para PantallaGanasteView.xaml
    /// </summary>
    public partial class PantallaGanasteView : UserControl
    {
        public PantallaGanasteView()
        {
            InitializeComponent();
        }

        private void btnNivel2_Click(object sender, RoutedEventArgs e)
        {
            MediadorViewModel.Nivel2();
        }
    }
}
