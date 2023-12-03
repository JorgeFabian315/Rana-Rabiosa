using Juego_PA.Models;
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
using System.Windows.Threading;

namespace Juego_PA.Views.Niveles
{
    /// <summary>
    /// Lógica de interacción para Nivel4View.xaml
    /// </summary>
    public partial class Nivel4View : UserControl
    {
        string escenarioCesped = "pack://application:,,,/Assets/Nivel4/cesped.png";
        Jugador jugador = new();
        JuegoViewModel? vm2;
        int columnRana;
        int rowRana;
        private Movimientos movimiento = new();
        DispatcherTimer timer = new();

        public Nivel4View()
        {
            InitializeComponent();

            CrearTablero(10, 10, Tablero, escenarioCesped);

            MediadorViewModel.IniciarJuegoNivel4Event += MediadorViewModel_IniciarJuegoNivel4Event;
            MediadorViewModel.ConseguirTodasEstrellasEvent += MediadorViewModel_ConseguirTodasEstrellasEvent;
        }

        private void MediadorViewModel_ConseguirTodasEstrellasEvent()
        {
            EnemigoBarrera.Visibility = Visibility.Collapsed;
            EnemigoBarrera2.Visibility = Visibility.Collapsed;
            EnemigoBarrera3.Visibility = Visibility.Collapsed;

        }

        private void MediadorViewModel_IniciarJuegoNivel4Event()
        {
            Estrella1.Visibility = Visibility.Visible;
            Estrella2.Visibility = Visibility.Visible;
            Estrella3.Visibility = Visibility.Visible;

            MoverImagenes(Estrella1, 9, 0);
            MoverImagenes(Estrella2, 6, 4);
            MoverImagenes(Estrella3, 0, 9);
        }

        #region CREAR COLUMNAS Y FILAS GRID
        public void CrearTablero(int numColum, int numFila, Grid tablero, string imagenEscenario)
        {

            tablero.ColumnDefinitions.Clear();
            tablero.RowDefinitions.Clear();

            // Crear columnas y filas en el grid
            for (int i = 0; i < numColum; i++)
            {
                tablero.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int j = 0; j < numFila; j++)
            {
                tablero.RowDefinitions.Add(new RowDefinition());
            }

            for (int fila = 0; fila < numFila; fila++)
            {
                for (int columna = 0; columna < numColum; columna++)
                {
                    Image fondoEscenario = new()
                    {
                        Stretch = Stretch.Fill,
                        Source = new BitmapImage(new Uri(imagenEscenario))
                    };

                    Grid.SetColumn(fondoEscenario, columna);

                    Grid.SetRow(fondoEscenario, fila);

                    tablero.Children.Add(fondoEscenario);
                }
            }
        }

        #endregion

        #region EVENTO KEY DOWN USERCONTROL 
        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            dock.Focusable = false;
            this.Focus();
            this.Focusable = true;
            var vm = this.DataContext as JuegoViewModel;

            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                if (e.Key == Key.Left)
                    movimiento = Movimientos.Izquierda;
                if (e.Key == Key.Right)
                    movimiento = Movimientos.Derecha;
                if (e.Key == Key.Up)
                    movimiento = Movimientos.Arriba;
                if (e.Key == Key.Down)
                    movimiento = Movimientos.Abajo;

                MoverRana(movimiento);
            }
            else
            {
                if (vm != null)
                    vm.OcultarTeclaIncorrectaCommand.Execute("mostrar");
            }
        }
        #endregion

        #region MOVER RANA
        private void MoverRana(Movimientos movi)
        {
            var vm = this.DataContext as JuegoViewModel;
            jugador.Movimiento = movi;
            jugador.Nivel = 4;

            if (vm != null)
            {
                vm.RealizarMovimientoNivel4Command.Execute(jugador);
            }

            columnRana = Grid.GetColumn(rana);
            rowRana = Grid.GetRow(rana);


            if (columnRana == Grid.GetColumn(Estrella1) && rowRana == Grid.GetRow(Estrella1))
            {
                Estrella1.Visibility = Visibility.Collapsed;
                MoverImagenes(Estrella1, 9, 3);
                vm.ConseguirEstrellaCommand.Execute(null);

            }
            if (columnRana == Grid.GetColumn(Estrella2) && rowRana == Grid.GetRow(Estrella2))
            {
                Estrella2.Visibility = Visibility.Collapsed;
                MoverImagenes(Estrella2, 9, 3);
                vm.ConseguirEstrellaCommand.Execute(null);

            }
            if (columnRana == Grid.GetColumn(Estrella3) && rowRana == Grid.GetRow(Estrella3))
            {
                Estrella3.Visibility = Visibility.Collapsed;
                MoverImagenes(Estrella3, 9, 3);
                vm.ConseguirEstrellaCommand.Execute(null);

            }




        }
        #endregion

        #region MOVER ENEMIGOS Y IMAGENES
        public void MoverImagenes(Image enemigo, int x, int y)
        {
            Grid.SetColumn(enemigo, x);
            Grid.SetRow(enemigo, y);
        }
        #endregion
























        private void btnDerecha_Click(object sender, RoutedEventArgs e)
        {
            MoverRana(Movimientos.Derecha);

        }

        private void btnAbajo_Click(object sender, RoutedEventArgs e)
        {
            MoverRana(Movimientos.Abajo);

        }

        private void btnIzquierda_Click(object sender, RoutedEventArgs e)
        {
            MoverRana(Movimientos.Izquierda);

        }

        private void btnArriba_Click(object sender, RoutedEventArgs e)
        {
            MoverRana(Movimientos.Arriba);

        }

        private void btnRegresarClick(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Aviso.Visibility = Visibility.Collapsed;
        }
    }
}
