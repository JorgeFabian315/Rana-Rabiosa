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
        #region VARIABLES 
        string escenarioAgua = "pack://application:,,,/Assets/Nivel3/Agua.jpg";
        string escenarioLava = "pack://application:,,,/Assets/Nivel3/lava.jpg";
        string escenarioTierra = "pack://application:,,,/Assets/Nivel3/tierra.jpeg";
        string slimeLavaRuta = "pack://application:,,,/Assets/Nivel3/slime-fuego.png";
        string slimeAguaRuta = "pack://application:,,,/Assets/Nivel3/slime-agua-2.png";
        Jugador jugador = new();
        Random random = new();
        JuegoViewModel? vm2;
        int columnRana;
        int rowRana;
        int contador = 0;
        int filaAleatoria = 1;
        int columnaAleatoria = 2;
        int filaAleatoria2 = 4;
        int columnaAleatoria2 = 4;
        bool tocado = false;
        bool tocado2 = false;
        private Movimientos movimiento = new();
        DispatcherTimer timer = new();
        #endregion
        public Nivel3View()
        {
            InitializeComponent();
            MediadorViewModel.CambiarEscenarioLavaEvent += MediadorViewModel_CambiarEscenarioLavaEvent;
            MediadorViewModel.EstadoTimerEvent += MediadorViewModel_EstadoTimerEvent;
            MediadorViewModel.IniciarJuegoNivel3Event += MediadorViewModel_IniciarJuegoNivel3Event;
            MediadorViewModel.CambiarEscenarioAguaEvent += MediadorViewModel_CambiarEscenarioAguaEvent;
            MediadorViewModel.PuenteEvent += MediadorViewModel_PuenteEvent;
        }

        private void MediadorViewModel_PuenteEvent()
        {
            Puente.Visibility = Visibility.Visible;
        }

        private void MediadorViewModel_IniciarJuegoNivel3Event()
        {
            slime1.Source = new BitmapImage(new Uri(slimeAguaRuta));
            slime2.Source = new BitmapImage(new Uri(slimeAguaRuta));
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += Timer_Tick;
            timer.Start();
            contador = 0;
            MoverImagenes(Corazon, 0, 3);
            EliminarHojas();
        }



        private void Timer_Tick(object? sender, EventArgs e)
        {
            vm2 = this.DataContext as JuegoViewModel;
            columnRana = Grid.GetColumn(rana);
            rowRana = Grid.GetRow(rana);

            contador++;

            if (contador == 10)
            {
                (filaAleatoria, columnaAleatoria) = (random.Next(1, 5), random.Next(1, 5));
                (filaAleatoria2, columnaAleatoria2) = (random.Next(1, 5), random.Next(1, 5));
                MoverImagenes(slime1, columnaAleatoria, filaAleatoria);
                MoverImagenes(slime2, columnaAleatoria2, filaAleatoria2);
                contador = 0;
                tocado = false;
                tocado2 = false;
            }

            if (columnaAleatoria == columnRana && filaAleatoria == rowRana && tocado == false)
            {
                vm2?.RestarVidaRanaCommand.Execute(null);
                tocado = true;
            }
            if (columnaAleatoria2 == columnRana && filaAleatoria2 == rowRana && tocado2 == false)
            {
                vm2?.RestarVidaRanaCommand.Execute(null);
                tocado2 = true;
            }

        }
        #region CAMBIAR ESCENARIO LAVA
        private void MediadorViewModel_CambiarEscenarioLavaEvent()
        {
            CrearTablero(5, 5, Tablero, escenarioLava);
            slime1.Source = new BitmapImage(new Uri(slimeLavaRuta));
            slime2.Source = new BitmapImage(new Uri(slimeLavaRuta));
            remolino.Visibility = Visibility.Collapsed;
            jugador.Escenario = 2;
            Corazon.Visibility = Visibility.Collapsed;
            Llave1.Visibility = Visibility.Collapsed;
            Llave2.Visibility = Visibility.Collapsed;
            MoverImagenes(Llave3, 4, 0);

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
        #endregion

        #region CAMBIAR ESCENARIO AGUA
        private void MediadorViewModel_CambiarEscenarioAguaEvent()
        {
            CrearTablero(5, 5, Tablero, escenarioAgua);
            slime1.Source = new BitmapImage(new Uri(slimeAguaRuta));
            slime2.Source = new BitmapImage(new Uri(slimeAguaRuta));
            remolino.Visibility = Visibility.Visible;
            Corazon.Visibility = Visibility.Visible;
            MoverImagenes(Corazon, 0, 3);
            jugador.Escenario = 1;
            Puente.Visibility = Visibility.Collapsed;

            Llave1.Visibility = Visibility.Visible;
            Llave2.Visibility = Visibility.Visible;

            MoverImagenes(Llave1, 2, 0);
            MoverImagenes(Llave2, 3, 4);


            foreach (var img in Tablero.Children.OfType<Image>())
            {
                if ((string)img.Tag == "Escenario_1")
                {
                    img.Visibility = Visibility.Visible;
                }
                if ((string)img.Tag == "Escenario_2")
                {
                    img.Visibility = Visibility.Collapsed;
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

            if (vm != null)
                vm.MensajeLLaveCommand.Execute("mostrar");

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
            }
        }
        #endregion

        #region MOVER RANA
        private async void MoverRana(Movimientos movi)
        {
            var vm = this.DataContext as JuegoViewModel;
            jugador.Movimiento = movi;
            jugador.Nivel = 3;

            if (vm != null)
                vm.MoverCommand.Execute(jugador);

            columnRana = Grid.GetColumn(rana);
            rowRana = Grid.GetRow(rana);

            #region OBTENER VIDA EXTRA
            int columnCorazon = Grid.GetColumn(Corazon);
            int filaCorazon = Grid.GetRow(Corazon);

            if (columnCorazon == columnRana && rowRana == filaCorazon)
            {
                vm?.SumarVidaRanaCommand.Execute(null);
                MoverImagenes(Corazon, 4, 1);
                if (columnCorazon == 4 && filaCorazon == 1)
                {
                    Corazon.Visibility = Visibility.Collapsed;
                    MoverImagenes(Corazon, 4, 4);
                }
            }
            #endregion




            #region LLAVES
            int columnLlave1 = Grid.GetColumn(Llave1);
            int filaLlave1 = Grid.GetRow(Llave1);

            if (columnLlave1 == columnRana && rowRana == filaLlave1 && jugador.Escenario == 1)
            {
                await Task.Delay(150); // Pausa de un 0.5 segundos
                vm?.ConseguirLlaveCommand.Execute(null);
                MoverImagenes(Llave1, 4, 4);
                Llave1.Visibility = Visibility.Collapsed;
                timer.Stop();
            }

            int columnLlave2 = Grid.GetColumn(Llave2);
            int filaLlave2 = Grid.GetRow(Llave2);

            if (columnLlave2 == columnRana && rowRana == filaLlave2 && jugador.Escenario == 1)
            {
                await Task.Delay(150); // Pausa de un 0.5 segundos
                vm?.ConseguirLlaveCommand.Execute(null);
                MoverImagenes(Llave2, 4, 4);
                Llave2.Visibility = Visibility.Collapsed;
                timer.Stop();
            }


            int columnLlave3 = Grid.GetColumn(Llave3);
            int filaLlave3 = Grid.GetRow(Llave3);

            if (columnLlave3 == columnRana && rowRana == filaLlave3 && jugador.Escenario == 2)
            {
                await Task.Delay(150); // Pausa de un 0.5 segundos
                vm?.ConseguirLlaveCommand.Execute(null);
                MoverImagenes(Llave3, 0, 4);
                Llave3.Visibility = Visibility.Collapsed;
                timer.Stop();
            }


            #endregion





            if (jugador.Escenario == 1)
                CrearHoja(columnRana, rowRana);
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
        private void MediadorViewModel_EstadoTimerEvent(bool estado)
        {
            if (estado == false)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }

        private void RegresarClick(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
