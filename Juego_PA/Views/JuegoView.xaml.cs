using Juego_PA.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
    /// Lógica de interacción para JuegoView.xaml
    /// </summary>
    public partial class JuegoView : UserControl
    {

        public JuegoView()
        {
            InitializeComponent();
            gridPadre.Focus();
            MediadorViewModel.IniciarJuegoEvent += MediadorViewModel_IniciarJuegoEvent;
        }
        private void MediadorViewModel_IniciarJuegoEvent()
        {
            gridPadre.Focus();
            tgbMenu.IsChecked = false;
        }
        private void gridPadre_KeyDown(object sender, KeyEventArgs e)
        {
            tgbMenu.Focusable = false;

            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                var avm = this.DataContext as JuegoViewModel;

                gridPadre.Focus();

                Movimientos movi = new();

                if (e.Key == Key.Left)
                    movi = Movimientos.Izquierda;
                if (e.Key == Key.Right)
                    movi = Movimientos.Derecha;
                if (e.Key == Key.Up)
                    movi = Movimientos.Arriba;
                if (e.Key == Key.Down)
                    movi = Movimientos.Abajo;

                avm.MoverCommand.Execute(movi);
            }
            else
            {
                MessageBox.Show("Tecla incorrecta");
            }

            tgbMenu.IsChecked = false;
        }

        //private void gridPadre_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    gridPadre.Focus();
        //}
    }
}
