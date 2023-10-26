﻿using Juego_PA.Models;
using Juego_PA.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Media;
using System.Security.Policy;
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

namespace Juego_PA.Views.Niveles
{
    /// <summary>
    /// Lógica de interacción para Nivel1View.xaml
    /// </summary>
    public partial class Nivel1View : UserControl
    {
		Jugador _jugador = new();
        SoundPlayer soundPlayer = new(@"C:Juego_PA\Juego_PA\Views\Niveles\SonidoSalto.mp3"); 
        public Nivel1View()
        {
            InitializeComponent();
            MediadorViewModel.IniciarJuegoNivel1Event += MediadorViewModel_IniciarJuegoEvent;
            MediadorViewModel.RegresarEvent += MediadorViewModel_RegresarEvent;

            CrearTablero(4, 3, Tablero);
            CrearCocodrilos();
        }

        private void MediadorViewModel_RegresarEvent()
        {
            Aviso.Visibility = Visibility.Visible;
        }

        private void MediadorViewModel_IniciarJuegoEvent()
        {
            EliminarHojas();
        }

        private async void Tablero_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                dock.Focusable = false;
                this.Focus();
                this.Focusable = true;

                var avm = this.DataContext as JuegoViewModel;
                
                
    //            soundPlayer.Load();
				//soundPlayer.Play();


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
                _jugador.Nivel = 1;
                if (avm != null)
                    avm.MoverCommand.Execute(_jugador);


                // CREAR HOJA EN LA MISMA POSICION DE LA RANA
                int columnRana = Grid.GetColumn(rana);
                int rowRana = Grid.GetRow(rana);

                if ((columnRana == 1 && rowRana == 1) || (columnRana == 3 && rowRana == 0))
                {
                    await Task.Delay(300); // Pausa de un 0.5 segundos
                    EliminarHojas();
                }
                else
                    CrearHoja(columnRana, rowRana);

            }
            else
            {
                TeclaIncorrectapantalla.Visibility = Visibility.Visible;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            this.Focusable = true;
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

        #region CREAR COCODRILOS
        public void CrearCocodrilos()
        {
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

            MoverImagenes(_cocodrilo1, 1, 1);
            MoverImagenes(_cocodrilo2, 3, 0);

            Tablero.Children.Add(_cocodrilo1);
            Tablero.Children.Add(_cocodrilo2);

        }
        #endregion

        #region MOVER ENEMIGOS Y IMAGENES
        public void MoverImagenes(Image enemigo, int x, int y)
        {
            Grid.SetColumn(enemigo, x);
            Grid.SetRow(enemigo, y);
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

        private void btnTutorial_Click(object sender, RoutedEventArgs e)
        {
            TutotialPantalla.Visibility = Visibility.Visible;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            TutotialPantalla.Visibility = Visibility.Collapsed;

        }

        private void OkRegresar_Click(object sender, RoutedEventArgs e)
        {
            Aviso.Visibility = Visibility.Collapsed;
        }

        private void btnOkTeclaIncorrecta_Click(object sender, RoutedEventArgs e)
        {
            TeclaIncorrectapantalla.Visibility = Visibility.Collapsed;

        }
    }
}
