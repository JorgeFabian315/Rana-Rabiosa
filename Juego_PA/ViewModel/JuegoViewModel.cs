using GalaSoft.MvvmLight.Command;
using Juego_PA.Models;
using Juego_PA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Juego_PA.ViewModel
{
    public class JuegoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> ListaMovimientos { get; set; } = new();

        Regex? regex;

        private string _patron = "";

        //private Rana _rana;

        //public Rana Rana
        //{
        //    get { return _rana; }
        //    set
        //    {
        //        _rana = value;
        //        OnPropertyChanged("Rana");
        //    }
        //}
        public Rana Rana { get; set; } = new();

        // public Rana Rana { get; set; } = new();
        public bool Ganaste { get; set; } = false;
        public bool GameOver { get; set; } = false;
        public Vista Vista { get; set; } = Vista.Inicio;
        public int LimiteMovimientos { get; set; }

        public ICommand MoverCommand { get; set; }
        public ICommand IniciarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public JuegoViewModel()
        {
            Rana = new();
            MoverCommand = new RelayCommand<Movimientos>(Mover);
            IniciarCommand = new RelayCommand(IniciarJuego);
            CambiarVistaCommand = new RelayCommand<Vista>(CambiarVista);

        }

        private void CambiarVista(Vista vista)
        {
            Vista = vista;
            OnPropertyChanged();
        }

        int _posicionXAnterior = 0;
        int _posicionYAnterior = 0;
        private void Mover(Movimientos movimiento)
        {
            regex = new(@"^(DDBB|BBDD|DDBDBI)$");
            /* 
             * D = DERECHA
             * I = IZQUIERA
             * B = ABAJO
             * A = ARRIBA
             */


            if (LimiteMovimientos > 0)
            {
                if (movimiento == Movimientos.Derecha)
                {
                    Rana.X += 1;
                    _patron += "D";
                }
                else if (movimiento == Movimientos.Izquierda)
                {
                    Rana.X -= 1;
                    _patron += "I";

                }

                else if (movimiento == Movimientos.Arriba)
                {
                    Rana.Y -= 1;
                    _patron += "A";
                }
                else if (movimiento == Movimientos.Abajo)
                {
                    Rana.Y += 1;
                    _patron += "B";
                }

                _posicionXAnterior = Rana.X;
                _posicionYAnterior = Rana.Y;

                LimiteMovimientos -= 1;

            }


            if ((Rana.X == 1 && Rana.Y == 1) || (Rana.X == 3 && Rana.Y == 0))
            {
                Rana.X = 0;
                Rana.Y = 0;
                LimiteMovimientos = 6;
                _patron = "";
            }
            if (regex.IsMatch(_patron))
            {
                Ganaste = true;

            }

            else if (LimiteMovimientos <= 0)
            {
                GameOver = true;
            }

            OnPropertyChanged();
            //Thread.Sleep(1000);
        }


        public void IniciarJuego()
        {
            Rana.X = 0;
            Rana.Y = 0;

            Vista = Vista.Juego;

            Ganaste = false;

            GameOver = false;

            MediadorViewModel.IniciarJuego();

            _patron = "";

            LimiteMovimientos = 6;

            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        }

    }
}
