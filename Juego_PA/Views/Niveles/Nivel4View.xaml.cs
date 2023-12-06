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
        Image Serpiente1 = new Image();
        Image Serpiente2 = new Image();
        Image Serpiente3 = new Image();
        Image Serpiente4 = new Image();
        Image Serpiente5 = new Image();
        public Nivel4View()
        {
            InitializeComponent();

            CrearTablero(10, 10, Tablero, escenarioCesped);

            MediadorViewModel.IniciarJuegoNivel4Event += MediadorViewModel_IniciarJuegoNivel4Event;
            MediadorViewModel.ConseguirTodasEstrellasEvent += MediadorViewModel_ConseguirTodasEstrellasEvent;
            Serpiente1 = CrearMoverSerpeinte(8, 1);
            Serpiente2 = CrearMoverSerpeinte(8, 4);
            Serpiente3 = CrearMoverSerpeinte(1, 6);
            Serpiente4 = CrearMoverSerpeinte(8, 8);
            Serpiente5 = CrearMoverSerpeinte(9, 9);

            CrearRocas();
        }

        private void MediadorViewModel_ConseguirTodasEstrellasEvent()
        {
            foreach (var img in Tablero.Children.OfType<Image>())
            {
                if ((string)img.Tag == "Enemigo")
                {
                    img.Visibility = Visibility.Collapsed;
                    MoverImagenes(img, 9, 3);

                }
            }
        }

        private void MediadorViewModel_IniciarJuegoNivel4Event()
        {
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += Timer_Tick; ;
            timer.Start();

            Estrella1.Visibility = Visibility.Visible;
            Estrella2.Visibility = Visibility.Visible;
            Estrella3.Visibility = Visibility.Visible;

            MoverImagenes(Estrella1, 9, 0);
            MoverImagenes(Estrella2, 6, 4);
            MoverImagenes(Estrella3, 0, 9);

            foreach (var img in Tablero.Children.OfType<Image>())
            {
                if ((string)img.Tag == "Enemigo")
                {
                    img.Visibility = Visibility.Visible;
                }
            }

            MoverImagenes(Serpiente1, 8, 1);
            MoverImagenes(Serpiente2,8, 4);
            MoverImagenes(Serpiente3,1, 6);
            MoverImagenes(Serpiente4,8, 8);
            MoverImagenes(Serpiente5 ,9, 9);


        }

        Random r = new();
        int contador = 0;
        int contadorC = 0;
        int generar = 0;
        bool tocadoC = false;
        bool tocadoM = false;
        private void Timer_Tick(object? sender, EventArgs e)
        {
            var vm = this.DataContext as JuegoViewModel;
            contador++;
            contadorC++;
            if (contador == 20)
            {
                tocadoM = false;
                MoverImagenes(Mosca, r.Next(1, 10), r.Next(1, 10));
                contador = 0;
            }
            if (contadorC == 40)
            {
                tocadoC = false;
                MoverImagenes(Cocodrilo, r.Next(1, 9), r.Next(1, 8));
                contadorC = 0;
            }

            if (columnRana == Grid.GetColumn(Cocodrilo) && rowRana == Grid.GetRow(Cocodrilo) && tocadoC == false)
            {
                tocadoC = true;
                vm?.RestarVidaRanaCommand.Execute(null);
            }
            if (columnRana == Grid.GetColumn(Mosca) && rowRana == Grid.GetRow(Mosca) && tocadoM == false)
            {
                tocadoM = true;
                vm?.SumarVidaRanaCommand.Execute(null);
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
                vm?.ConseguirEstrellaCommand.Execute(null);

            }
            if (columnRana == Grid.GetColumn(Estrella2) && rowRana == Grid.GetRow(Estrella2))
            {
                Estrella2.Visibility = Visibility.Collapsed;
                MoverImagenes(Estrella2, 9, 3);
                vm?.ConseguirEstrellaCommand.Execute(null);

            }
            if (columnRana == Grid.GetColumn(Estrella3) && rowRana == Grid.GetRow(Estrella3))
            {
                Estrella3.Visibility = Visibility.Collapsed;
                MoverImagenes(Estrella3, 9, 3);
                vm?.ConseguirEstrellaCommand.Execute(null);
            }

            var cFlor = Grid.GetColumn(Flor);
            var rFlor = Grid.GetRow(Flor);
            if (columnRana == cFlor && rowRana == rFlor)
            {
                timer.Stop();
                vm?.GanasteJuegoCommand.Execute(null);
            }



            if ((Grid.GetColumn(Serpiente1) == columnRana && Grid.GetRow(Serpiente1) == rowRana) ||
                (Grid.GetColumn(Serpiente2) == columnRana && Grid.GetRow(Serpiente2) == rowRana) ||
                (Grid.GetColumn(Serpiente3) == columnRana && Grid.GetRow(Serpiente3) == rowRana) ||
                (Grid.GetColumn(Serpiente4) == columnRana && Grid.GetRow(Serpiente4) == rowRana) ||
                (Grid.GetColumn(Serpiente5) == columnRana && Grid.GetRow(Serpiente5) == rowRana)
                )
            {
                vm?.RegresarteEnemigosCommand.Execute(null);
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

        #region MOVIMIENTO CON LOS BOTONES  
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

        #endregion

        public void CrearRocas()
        {
            for (int i = 2; i <= 9; i++)
            {
                var roca = Roca();
                MoverImagenes(roca, i, 3);
                Tablero.Children.Add(roca);
            }
            CrearMoverRoca(5, 0);
            CrearMoverRoca(2, 1);
            CrearMoverRoca(3, 1);
            CrearMoverRoca(5, 1);
            CrearMoverRoca(7, 1);
            CrearMoverRoca(7, 2);
            CrearMoverRoca(9, 2);
            CrearMoverRoca(9, 4);
            CrearMoverRoca(7, 4);
            CrearMoverRoca(5, 5);
            CrearMoverRoca(0, 5);
            CrearMoverRoca(1, 5);
            CrearMoverRoca(6, 5);
            CrearMoverRoca(7, 5);
            CrearMoverRoca(9, 5);
            CrearMoverRoca(9, 6);
            CrearMoverRoca(3, 7);
            CrearMoverRoca(4, 7);
            CrearMoverRoca(5, 7);
            CrearMoverRoca(6, 7);
            CrearMoverRoca(8, 7);
            CrearMoverRoca(9, 7);
            CrearMoverRoca(0, 8);
            CrearMoverRoca(6, 8);
            CrearMoverRoca(1, 8);
            CrearMoverRoca(0, 2);
            CrearMoverRoca(0, 3);
            CrearMoverRoca(0, 4);
            CrearMoverRoca(0, 5);
            CrearMoverRoca(0, 5);
            CrearMoverRoca(3, 5);


        }
        public Image Roca()
        {
            Image roca = new()
            {
                Tag = "Roca",
                Margin = new Thickness(5),
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Nivel3/roca.png")),
            };
            return roca;
        }
        public Image Serpiente()
        {
            Image serpeiente = new()
            {
                Tag = "Enemigo",
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/serpiente.png")),
            };
            return serpeiente;
        }
        public Image CrearMoverSerpeinte(int x, int y)
        {
            var ser = Serpiente();
            MoverImagenes(ser, x, y);
            Tablero.Children.Add(ser);
            return ser;
        }

        public void CrearMoverRoca(int x, int y)
        {
            var roca = Roca();
            MoverImagenes(roca, x, y);
            Tablero.Children.Add(roca);
        }

        private void btnRegresarClick(object sender, RoutedEventArgs e)
        {
            timer.Stop();
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
