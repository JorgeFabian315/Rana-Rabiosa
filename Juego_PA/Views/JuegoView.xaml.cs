using Juego_PA.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            MediadorViewModel.PintarBordesEvent += MediadorViewModel_PintarBordesEvent;
        }

        private void MediadorViewModel_PintarBordesEvent()
        {
            ColorearBordes();
        }

        private void MediadorViewModel_IniciarJuegoEvent()
        {
            gridPadre.Focus();
            tgbMenu.IsChecked = false;
            ColorearBordes();
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

                int columnRana = Grid.GetColumn(rana);
                int rowRana = Grid.GetRow(rana);

                foreach (var borde in gridPadre.Children.OfType<Border>())
                {
                    var colorBorder = borde.Background;

                    var columnBorde = Grid.GetColumn(borde);
                    var rowBorde = Grid.GetRow(borde);

                    if (columnRana == columnBorde && rowRana == rowBorde || columnBorde == 0 && rowBorde == 0 )
                    {
                        borde.Background = Brushes.Green;
                    }
                }

            }
            else
            {
                MessageBox.Show("Tecla incorrecta");
            }

            tgbMenu.IsChecked = false;
        }
        public void ColorearBordes()
        {
            foreach (var borde in gridPadre.Children.OfType<Border>())
            {
                var colorBorder = borde.Background;

                var colorAzul = new BrushConverter().ConvertFrom("#afddec");

                var columnBorde = Grid.GetColumn(borde);
                var rowBorde = Grid.GetRow(borde);

                if (rowBorde == 0 || rowBorde == 2)
                {
                    if(columnBorde % 2 == 0)
                        borde.Background = (SolidColorBrush)colorAzul;
                    else
                        borde.Background = Brushes.SkyBlue;

                }
                else
                {
                    if (columnBorde % 2 == 0)
                        borde.Background = Brushes.SkyBlue;
                    else
                        borde.Background = (SolidColorBrush)colorAzul;
                }
            }
        }
       
    }
}
