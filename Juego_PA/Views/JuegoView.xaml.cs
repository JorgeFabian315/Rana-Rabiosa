using Juego_PA.Models;
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
    /// 
    public partial class JuegoView : UserControl
    {
        Jugador _jugador = new();
        public JuegoView()
        {
            InitializeComponent();
            this.Focus();
            CrearNivel();
            //MediadorViewModel.IniciarJuegoEvent += MediadorViewModel_IniciarJuegoEvent;
            //MediadorViewModel.RegresarEvent += MediadorViewModel_RegresarEvent;
            //MediadorViewModel.EliminarHojasEvent += MediadorViewModel_EliminarHojasEvent;
            //MediadorViewModel.Nivel2Event += EmpezarNivel2;
        }

        private void MediadorViewModel_EliminarHojasEvent()
        {
            EliminarHojas();
        }

        private void MediadorViewModel_RegresarEvent()
        {
            MessageBox.Show("No puedes regresarte");
        }
      
        private void MediadorViewModel_IniciarJuegoEvent()
        {
            //this.Focusable = true;
            //this.Focus();
            //gridPadre.Focusable = true;
            //gridPadre.Focus();
            //CrearNivel();
            //btnTutorial.Focusable = false;
        }
        private void gridPadre_KeyDown(object sender, KeyEventArgs e)
        {
            this.Focus();
            gridPadre.Focusable = true;
            btnTutorial.Focusable = false;

            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                var avm = this.DataContext as JuegoViewModel;

                Movimientos movi = new();
                if (e.Key == Key.Left)
                    movi = Movimientos.Izquierda;
                if (e.Key == Key.Right)
                    movi = Movimientos.Derecha;
                if (e.Key == Key.Up)
                    movi = Movimientos.Arriba;
                if (e.Key == Key.Down)
                    movi = Movimientos.Abajo;

                _jugador.Movimiento = movi;

                if (avm != null)
                    avm.MoverCommand.Execute(_jugador);


                // CREAR HOJA EN LA MISMA POSICION DE LA RANA
                int columnRana = Grid.GetColumn(rana);
                int rowRana = Grid.GetRow(rana);
                CrearHoja(columnRana, rowRana);

            }
            else
            {
                MessageBox.Show("Tecla incorrecta");
            }

        }
        public void CrearNivel(int numColum = 4, int numFila = 3, Nivel _nivel = Nivel.Nivel1)
        {
            #region CREACION DE ENEMIGOS COCODRILOS
            Image _cocodrilo1 = new()
            {
                Tag = "Enemigo",
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/cocodrilo.png")),
                Margin = new Thickness(20)

            };

            Image _cocodrilo2 = new()
            {
                Tag = "Enemigo",
                Source = new BitmapImage(new Uri("/Juego_PA;component/Assets/cocodrilo.png", UriKind.Relative)),
                Margin = new Thickness(20)

            };
            #endregion

            #region ELIMINAR IMAGENES
            EliminarImagenes();
            #endregion


            if (_nivel == Nivel.Nivel1)
            {
                _jugador.Nivel = 1;

                //CrearTablero(numColum, numFila);
                MoverImagenes(_cocodrilo1, 1, 1);
                MoverImagenes(_cocodrilo2, 3, 0);
            }
            else if (_nivel == Nivel.Nivel2)
            {
                _jugador.Nivel = 2;

                //CrearTablero(numColum, numFila);
                MoverImagenes(flor, 4, 4);

                Image _tornado1 = new()
                {
                    Tag = "Tornado",
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/tornado.png")),
                    Margin = new Thickness(20)
                };

                Image _tornado2 = new()
                {
                    Tag = "Tornado",
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/tornado.png")),
                    Margin = new Thickness(20)
                };


                MoverImagenes(_cocodrilo1, 1, 3);
                MoverImagenes(_cocodrilo2, 4, 2);
                MoverImagenes(_tornado1, 3, 1);
                MoverImagenes(_tornado2, 2, 4);

                gridPadre.Children.Add(_tornado1);
                gridPadre.Children.Add(_tornado2);


            }

            gridPadre.Children.Add(_cocodrilo1);
            gridPadre.Children.Add(_cocodrilo2);
        }


        #region MOVER ENEMIGOS Y IMAGENES
        public void MoverImagenes(Image enemigo, int x, int y)
        {
            Grid.SetColumn(enemigo, x);
            Grid.SetRow(enemigo, y);
        }
        #endregion

        #region CREAR COLUMNAS Y FILAS GRID
        public void CrearTablero(int numColum, int numFila, Grid tablero)
        {

            gridPadre.ColumnDefinitions.Clear();
            gridPadre.RowDefinitions.Clear();

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

        #region ELIMINAR IMAGENES
        public void EliminarImagenes()
        {
            foreach (var image in gridPadre.Children.OfType<Image>())
            {
                if ((string)image.Tag != "Rana" && (string)image.Tag != "Flor")
                    image.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region CREAR HOJA DE LOTO
        public void CrearHoja(int colum, int row)
        {
            Image loto = new()
            {
                Width = 130,
                Height = 130,
                Tag = "Loto"

            };
            loto.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/loto.png"));
            Grid.SetColumn(loto, colum);
            Grid.SetRow(loto, row);
            gridPadre.Children.Add(loto);

        }

        #endregion
        
        #region ELIMINAR HOJAS LOTO
        public void EliminarHojas()
        {
            foreach (var image in gridPadre.Children.OfType<Image>())
            {
                if ((string)image.Tag == "Loto")
                    image.Visibility = Visibility.Collapsed;
            }
        }

        #endregion


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            PantallaTutorial.Visibility = Visibility.Collapsed;
        }

        private void btnTutorial_Click(object sender, RoutedEventArgs e)
        {
            PantallaTutorial.Visibility = Visibility.Visible;

        }

        private void EmpezarNivel2()
        {
            CrearNivel(5, 5, Nivel.Nivel2);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            this.Focusable = true;
            gridPadre.Focusable = true;
            gridPadre.Focus();
        }

       
    }
}
