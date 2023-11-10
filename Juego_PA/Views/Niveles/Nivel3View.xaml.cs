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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Juego_PA.Views.Niveles
{
    /// <summary>
    /// Lógica de interacción para Nivel3View.xaml
    /// </summary>
    public partial class Nivel3View : UserControl
    {
        string escenarioAgua = "pack://application:,,,/Assets/Nivel3/Agua.jpg";
        string escenarioLava = "pack://application:,,,/Assets/Nivel3/lava.jpg";
        string slimeLavaRuta = "pack://application:,,,/Assets/Nivel3/slime-fuego.png";
        string slimeAguaRuta = "pack://application:,,,/Assets/Nivel3/slime-agua-2.png";
        private Jugador _jugador = new();
        private Movimientos movimiento = new();
        DispatcherTimer timer = new();
        public Nivel3View()
        {
            InitializeComponent();
            MediadorViewModel.CambiarEscenarioLavaEvent += MediadorViewModel_CambiarEscenarioLavaEvent;
            MediadorViewModel.IniciarTimerEvent += MediadorViewModel_IniciarTimerEvent;
            MediadorViewModel.IniciarJuegoNivel3Event += MediadorViewModel_IniciarJuegoNivel3Event;
        }

        private void MediadorViewModel_IniciarJuegoNivel3Event()
        {
            CrearTablero(5, 5, Tablero, escenarioAgua);
            slime1.Source = new BitmapImage(new Uri(slimeAguaRuta));
            slime2.Source = new BitmapImage(new Uri(slimeAguaRuta));
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();
            EliminarHojas();
        }

       

        Random random = new();
        int columnRana;
        int rowRana;
        int contador = 0;
        int filaAleatoria = 10;
        int columnaAleatoria = 10;
        int filaAleatoria2 = 10;
        int columnaAleatoria2 = 10;
        JuegoViewModel? vm2;
        private void Timer_Tick(object? sender, EventArgs e)
        {
            vm2 = this.DataContext as JuegoViewModel;
            
            filaAleatoria = random.Next(0, 5);
            filaAleatoria2 = random.Next(0, 5);
            columnaAleatoria = random.Next(1, 5);
            columnaAleatoria2 = random.Next(1, 5);
            MoverImagenes(slime1, columnaAleatoria, filaAleatoria);
            MoverImagenes(slime2, columnaAleatoria2, filaAleatoria2);

            columnRana = Grid.GetColumn(rana);
            rowRana = Grid.GetRow(rana);

            if (columnaAleatoria == columnRana && filaAleatoria == rowRana)
                vm2?.RestarVidaRanaCommand.Execute(null);

            if (columnaAleatoria2 == columnRana && filaAleatoria2 == rowRana)
                vm2?.RestarVidaRanaCommand.Execute(null);

        }

        private void MediadorViewModel_CambiarEscenarioLavaEvent()
        {
            CrearTablero(5, 5, Tablero, escenarioLava);
            slime1.Source = new BitmapImage(new Uri(slimeLavaRuta));
            slime2.Source = new BitmapImage(new Uri(slimeLavaRuta));
            remolino.Visibility = Visibility.Collapsed;

            foreach (var img in Tablero.Children.OfType<Image>())
            {
                if ((string)img.Tag == "Escenario_1")
                {
                    img.Visibility = Visibility.Collapsed;
                }
                if ((string)img.Tag == "Escenario_2")
                {
                    img.Visibility = Visibility.Visible;
                }
            }
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


        #region MOVER ENEMIGOS Y IMAGENES
        public void MoverImagenes(Image enemigo, int x, int y)
        {
            Grid.SetColumn(enemigo, x);
            Grid.SetRow(enemigo, y);
        }


        #endregion

        #region ELIMINAR HOJAS LOTO
        public void EliminarHojas()
        {
            foreach (var image in Tablero.Children.OfType<Image>())
            {
                if ((string)image.Tag == "Loto")
                    image.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

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

                // CREAR HOJA EN LA MISMA POSICION DE LA RANA
                MoverRana(movimiento);

            }
            else
            {
                if (vm != null)
                    vm.OcultarTeclaIncorrectaCommand.Execute("mostrar");
                timer.Stop();
            }
        }

        private void MoverRana(Movimientos movi)
        {
            var vm = this.DataContext as JuegoViewModel;

            _jugador.Movimiento = movi;
            _jugador.Nivel = 3;

            if (vm != null)
                vm.MoverCommand.Execute(_jugador);

            columnRana = Grid.GetColumn(rana);
            rowRana = Grid.GetRow(rana);

            //if ((columnRana == 0 && rowRana == 1) || (columnRana == 3 && rowRana == 0)
            //    || (columnRana == 2 && rowRana == 1) || (columnRana == 1 && rowRana == 3))
            //{
            //    await Task.Delay(300); // Pausa de un 0.5 segundos
            //    EliminarHojas();
            //}
            //else
            CrearHoja(columnRana, rowRana);
        }

        #region CREAR HOJA DE LOTO
        public void CrearHoja(int colum, int row)
        {
            Image loto = new()
            {
                Width = 85,
                Height = 85,
                Tag = "Loto"

            };
            loto.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/loto.png"));
            Grid.SetColumn(loto, colum);
            Grid.SetRow(loto, row);
            Tablero.Children.Add(loto);

        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }
        private void btnAbajo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDerecha_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnIzquierda_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnArriba_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MediadorViewModel_IniciarTimerEvent()
        {
            timer.Start();
        }
    }
}
