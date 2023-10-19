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
        DispatcherTimer gameTimer = new();
        int _velocidadPescador = 10;
        Jugador _jugador = new();
        Movimientos movi = new();
        public Nivel3View()
        {
            InitializeComponent();
            IniciarJuego();
        }

        public void IniciarJuego()
        {
            CrearTablero(6, 6, Tablero);
            CrearEnemigos();
            MediadorViewModel.IniciarJuegoNivel3Event += MediadorViewModel_IniciarJuegoNivel3Event;
            CrearMoneda();
        }

        private void MediadorViewModel_IniciarJuegoNivel3Event()
        {
            EliminarTodasHojas();
        }


        #region CREAR COLUMNAS Y FILAS GRID
        public void CrearTablero(int numColum, int numFila, Grid tablero)
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
                    Border _borderFondo = new();
                    if (fila % 2 != 0)
                    {
                        if (columna % 2 == 0)
                            _borderFondo.Background = Brushes.SkyBlue;
                        else
                            _borderFondo.Background = Brushes.DeepSkyBlue;
                    }
                    else
                    {
                        if (columna % 2 == 0)
                            _borderFondo.Background = Brushes.DeepSkyBlue;
                        else
                            _borderFondo.Background = Brushes.SkyBlue;
                    }
                    Grid.SetColumn(_borderFondo, columna);

                    Grid.SetRow(_borderFondo, fila);

                    tablero.Children.Add(_borderFondo);
                }
            }
        }

        #endregion

       

        private async void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                dock.Focusable = false;
                this.Focus();
                this.Focusable = true;

                var avm = this.DataContext as JuegoViewModel;

                switch (e.Key)
                {
                    case Key.Left:
                        movi = Movimientos.Izquierda;
                        break;
                    case Key.Right:
                        movi = Movimientos.Derecha;
                        break;
                    case Key.Up:
                        movi = Movimientos.Arriba;
                        break;
                    case Key.Down:
                        movi = Movimientos.Abajo;
                        break;
                }

                _jugador.Movimiento = movi;
                _jugador.Nivel = 3;

                if (avm != null)
                    avm.MoverCommand.Execute(_jugador);


                // CREAR HOJA EN LA MISMA POSICION DE LA RANA
                int columnRana = Grid.GetColumn(rana);
                int rowRana = Grid.GetRow(rana);

                //if ((columnRana == 0 && rowRana == 1) || (columnRana == 3 && rowRana == 0)
                //    || (columnRana == 2 && rowRana == 1) || (columnRana == 1 && rowRana == 3))
                //{
                //    await Task.Delay(300); // Pausa de un 0.5 segundos
                //    EliminarHojas();
                //}
                //else


                //ANIMACION ENEMIGOS PLANTA 
                if (columnRana == 2 && rowRana == 2)
                {
                    rana.Visibility = Visibility.Collapsed;
                    IniciarAnimacionPlantas();
                    await Task.Delay(500); // Pausa de un 0.5 segundos
                    TerminarAnimacionPlantas();
                }
                else
                {

                    CrearHoja(columnRana, rowRana);
                }

            }
            else
            {
                MessageBox.Show("Tecla incorrecta");
            }
        }

        #region CREAR MONEDA 
        public void CrearMoneda()
        {
            Image moneda1 = new()
            {
                Tag = "Moneda",
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/moneda.png")),
                Margin = new Thickness(7)

            };
            Image moneda2 = new()
            {
                Tag = "Moneda",
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/moneda.png")),
                Margin = new Thickness(7)

            };

            MoverAgregarImagenes(moneda1, 1, 5);
            MoverAgregarImagenes(moneda2, 5, 1);


        }
        #endregion

        #region CREAR ENEMIGOS
        public void CrearEnemigos()
        {
            Image _pirana1 = new()
            {
                Tag = "Enemigo",
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/pirana.png")),
                Margin = new Thickness(7)

            };
            Image _pirana2 = new()
            {
                Tag = "Enemigo",
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/pirana.png")),
                Margin = new Thickness(7)

            };

            Image _serpiente1 = new()
            {
                Tag = "Enemigo",
                Source = new BitmapImage(new Uri("/Juego_PA;component/Assets/serpiente.png", UriKind.Relative)),
                Margin = new Thickness(7)

            };
            Image _serpiente2 = new()
            {
                Tag = "Enemigo",
                Source = new BitmapImage(new Uri("/Juego_PA;component/Assets/serpiente.png", UriKind.Relative)),
                Margin = new Thickness(7)

            };

            MoverAgregarImagenes(_pirana1, 0, 2);
            MoverAgregarImagenes(_pirana2, 5, 0);
            MoverAgregarImagenes(_serpiente1, 0, 5);
            MoverAgregarImagenes(_serpiente2, 5, 4);

        }
        #endregion

        #region MOVER ENEMIGOS Y IMAGENES
        public void MoverAgregarImagenes(Image enemigo, int x, int y)
        {
            Grid.SetColumn(enemigo, x);
            Grid.SetRow(enemigo, y);

            Tablero.Children.Add(enemigo);
        }


        #endregion

        #region CREAR HOJA DE LOTO
        public void CrearHoja(int colum, int row)
        {
            Image loto = new()
            {
                Width = 70,
                Height = 70,
                Tag = "Loto"

            };
            loto.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/loto.png"));
            Grid.SetColumn(loto, colum);
            Grid.SetRow(loto, row);
            Tablero.Children.Add(loto);

        }

        #endregion

        #region ELIMINAR HOJAS LOTO
        public void EliminarTodasHojas()
        {
            foreach (var image in Tablero.Children.OfType<Image>())
            {
                if ((string)image.Tag == "Loto")
                    image.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region ANIMACIONES PLANTAS CARNIVORAS
        private void IniciarAnimacionPlantas()
        {
            // Crea una nueva instancia de la animación para cambiar el tamaño de la imagen FlorMataRana.
            DoubleAnimation animacionFlorMataRana = new DoubleAnimation();
            animacionFlorMataRana.To = 100; // Altura final deseada para FlorMataRana
            animacionFlorMataRana.Duration = TimeSpan.FromSeconds(0.5); // Duración de la animación (en segundos)
            // Asigna la animación al elemento de imagen FlorMataRana y comienza la animación.
            FlorMataRana.BeginAnimation(Image.HeightProperty, animacionFlorMataRana);

            // Crea una nueva instancia de la animación para cambiar el tamaño de la imagen FlorRoja.
            DoubleAnimation animacionFlorRoja = new DoubleAnimation();
            animacionFlorRoja.To = 0; // Altura final deseada para FlorRoja
            animacionFlorRoja.Duration = TimeSpan.FromSeconds(0.5); // Duración de la animación (en segundos)
            // Asigna la animación al elemento de imagen FlorRoja y comienza la animación.
            FlorRoja.BeginAnimation(Image.HeightProperty, animacionFlorRoja);
        }
        private void TerminarAnimacionPlantas()
        {

            rana.Visibility = Visibility.Visible;
            EliminarTodasHojas();
            var avm = this.DataContext as JuegoViewModel;
            avm?.MoverRanaOrigen.Execute(null);

            // Crea una nueva instancia de la animación para cambiar el tamaño de la imagen FlorMataRana.
            DoubleAnimation animacionFlorMataRana = new DoubleAnimation();
            animacionFlorMataRana.To = 0; // Altura final deseada para FlorMataRana
            animacionFlorMataRana.Duration = TimeSpan.FromSeconds(0.5); // Duración de la animación (en segundos)
            // Asigna la animación al elemento de imagen FlorMataRana y comienza la animación.
            FlorMataRana.BeginAnimation(Image.HeightProperty, animacionFlorMataRana);

            // Crea una nueva instancia de la animación para cambiar el tamaño de la imagen FlorRoja.
            DoubleAnimation animacionFlorRoja = new DoubleAnimation();
            animacionFlorRoja.To = 100; // Altura final deseada para FlorRoja
            animacionFlorRoja.Duration = TimeSpan.FromSeconds(0.5); // Duración de la animación (en segundos)
            // Asigna la animación al elemento de imagen FlorRoja y comienza la animación.
            FlorRoja.BeginAnimation(Image.HeightProperty, animacionFlorRoja);
        }
        #endregion ANIMACIONES PLANTAS CARNIVORAS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }
    }
}
