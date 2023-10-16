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
    /// Lógica de interacción para Nivel3View.xaml
    /// </summary>
    public partial class Nivel3View : UserControl
    {
        DispatcherTimer gameTimer = new();
        int _velocidadPescador = 10;
        public Nivel3View()
        {
            InitializeComponent();
            CrearTablero(6, 6, Tablero);
            CrearEnemigos();
            gameTimer.Interval = TimeSpan.FromMilliseconds(50);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            var getTopPescador = Canvas.GetTop(pescador);
            var getLeftPescador = Canvas.GetLeft(pescador);
            double limiteAbajo = Mycanvas.ActualHeight;
            double limiteIzquierda = Mycanvas.ActualWidth;

            if(getTopPescador < limiteAbajo - pescador.Height)
            {
                Canvas.SetTop(pescador, getTopPescador + _velocidadPescador);
            }
            else if(getLeftPescador <= limiteIzquierda - pescador.Width)
            {
                Canvas.SetLeft(pescador, getLeftPescador + _velocidadPescador);
            }
            







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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }


        #region CREAR ENEMIGOS
        public void CrearEnemigos()
        {
            //Image _cocodrilo1 = new()
            //{
            //    Tag = "Enemigo",
            //    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/cocodrilo.png")),
            //    Margin = new Thickness(20)

            //};

            //Image _cocodrilo2 = new()
            //{
            //    Tag = "Enemigo",
            //    Source = new BitmapImage(new Uri("/Juego_PA;component/Assets/cocodrilo.png", UriKind.Relative)),
            //    Margin = new Thickness(20)

            //};
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
            //MoverImagenes(_cocodrilo1, 0, 1);
            //MoverImagenes(_cocodrilo2, 3, 0);
            MoverImagenes(_serpiente1, 0, 5);
            MoverImagenes(_serpiente2, 5, 4);

            //Tablero.Children.Add(_cocodrilo1);
            //Tablero.Children.Add(_cocodrilo2);
            Tablero.Children.Add(_serpiente1);
            Tablero.Children.Add(_serpiente2);

        }
        #endregion

        #region MOVER ENEMIGOS Y IMAGENES
        public void MoverImagenes(Image enemigo, int x, int y)
        {
            Grid.SetColumn(enemigo, x);
            Grid.SetRow(enemigo, y);
        }


        #endregion


    }
}
