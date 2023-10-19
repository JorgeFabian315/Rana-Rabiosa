﻿using GalaSoft.MvvmLight.Command;
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Juego_PA.ViewModel
{
    public class JuegoViewModel : INotifyPropertyChanged
    {
        Regex? regex;

        private string _patron = "";
        public Rana Rana { get; set; } = new();
        public bool Ganaste { get; set; } = false;
        public bool GameOver { get; set; } = false;

        public int NivelActual { get; set; }

        public Vista Vista { get; set; } = Vista.Inicio;

        public ICommand MoverCommand { get; set; }
        public ICommand IniciarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand Nivel2Command { get; set; }
        public ICommand MoverRanaOrigen => new RelayCommand(MoverRana);

        private void MoverRana()
        {
            Rana.X = 0;
            Rana.Y = 0;
            Rana.Vida -= 2;
            OnPropertyChanged(nameof(Rana));
        }

        int _posicionXAnterior = 0;
        int _posicionYAnterior = 0;
        string _movimientoPermitido = "";
        string _movimiento = "";
        bool _quedarseInmovil = false;
        public JuegoViewModel()
        {
            MoverCommand = new RelayCommand<Jugador>(RealizarMovimiento);
            Nivel2Command = new RelayCommand(Nivel2);
            IniciarCommand = new RelayCommand<string>(IniciarJuego);
            CambiarVistaCommand = new RelayCommand<Vista>(CambiarVista);

        }
        public void RealizarMovimiento(Jugador _jugador)
        {
            Rana.LimiteMovimientos -= 1;

            if (Rana.LimiteMovimientos > 0 && !_quedarseInmovil)
            {
                _posicionXAnterior = Rana.X;
                _posicionYAnterior = Rana.Y;

                if (_jugador.Movimiento == Movimientos.Derecha)
                {
                    Rana.X += 1;
                    _movimientoPermitido = "D";
                    _movimiento += "D";
                }
                else if (_jugador.Movimiento == Movimientos.Izquierda)
                {
                    Rana.X -= 1;
                    _movimientoPermitido = "I";
                    _movimiento += "I";
                }
                else if (_jugador.Movimiento == Movimientos.Arriba)
                {
                    Rana.Y -= 1;
                    _movimientoPermitido = "A";
                    _movimiento += "A";
                }
                else if (_jugador.Movimiento == Movimientos.Abajo)
                {
                    Rana.Y += 1;
                    _movimientoPermitido = "B";
                    _movimiento += "B";
                }

                if (_jugador.Nivel == 1)
                {
                    NivelActual = 1;
                    Nivel1();
                }
                else if (_jugador.Nivel == 2)
                {
                    NivelActual = 2;
                    Nivel2();
                }
                else if (_jugador.Nivel == 3)
                {
                    NivelActual = 3;
                    Nivel3();
                }
                else if (_jugador.Nivel == 4)
                {
                    NivelActual = 4;
                    Nivel4();
                }

            }

            if (Rana.LimiteMovimientos == 0)
            {
                Vista = Vista.Ganaste;
                GameOver = true;
            }

            OnPropertyChanged();
        }


        public void IniciarJuego(string nivel)
        {

            Rana.X = 0;
            Rana.Y = 0;
            Vista = Vista.Nivel1;
            Ganaste = false;
            GameOver = false;
            _patron = "";

            if (nivel == "1")
            {
                MediadorViewModel.IniciarJuegoNivel1();
                Rana.LimiteMovimientos = 10;
                Rana.Puntaje = 0;
                Vista = Vista.Nivel1;
            }
            else if (nivel == "2")
            {
                MediadorViewModel.IniciarJuegoNivel2();
                Vista = Vista.Nivel2;
                Rana.Vida = 6;
                Rana.LimiteMovimientos = 20;
            }
            else if (nivel == "3")
            {
                Vista = Vista.Nivel3;
                Rana.Vida = 6;
                MediadorViewModel.IniciarJuegoNivel3();
                Rana.LimiteMovimientos = 20;
            }
            else if (nivel == "4")
            {
                Vista = Vista.Nivel4;
                Rana.Vida = 6;
                MediadorViewModel.IniciarJuegoNivel4();
                Rana.LimiteMovimientos = 20;
            }


            OnPropertyChanged();
        }
        private async void Nivel1()
        {
            regex = new(@"^(DDBB|BBDD|DDBDBI)$");
            if ((_posicionXAnterior >= Rana.X && _posicionYAnterior >= Rana.Y)
                && _patron != "DDBD" && _movimientoPermitido != "I")
            {
                MediadorViewModel.Regresar();
                OnPropertyChanged();
                Rana.X = _posicionXAnterior;
                Rana.Y = _posicionYAnterior;
            }
            else
            {
                _patron += _movimientoPermitido;
            }

            // OBSTACULOS
            if ((Rana.X == 1 && Rana.Y == 1) || (Rana.X == 3 && Rana.Y == 0))
            {
                OnPropertyChanged("Rana");
                await Task.Delay(300); // Pausa de un 0.5 segundos
                Rana.X = 0;
                Rana.Y = 0;
                Rana.LimiteMovimientos = 10;
                _patron = "";
            }

            // GANASTE EL JUEGO
            if (regex.IsMatch(_patron) || ((Rana.X == 2 && Rana.Y == 2)))
            {
                OnPropertyChanged("Rana");
                await Task.Delay(300); // Pausa de un 0.5 segundos
                Vista = Vista.Ganaste;
                Rana.Puntaje += 100;
                NivelActual = 1;
                Rana.LimiteMovimientos = 0;
            }


            OnPropertyChanged();
        }
        public async void Nivel2()
        {
            regex = new(@"^(DBBDDB)$");

            // GANASTE
            if (Rana.X == 3 && Rana.Y == 3)
            {
                OnPropertyChanged("Rana");
                await Task.Delay(300); // Pausa de un 0.5 segundos
                Vista = Vista.Ganaste;
                // Si cumple la expresion regular se suma 100  y si no 80 y si hace 15 movimientos se sumara 50
                Rana.Puntaje += regex.IsMatch(_movimiento) ? 100 : _movimiento.Length > 15 ? 50 : 80;
                Rana.Puntaje += (Rana.Vida * 10); // Se le sumara el puntaje dependiendo de su vida 
                Rana.LimiteMovimientos = 0;
                NivelActual = 2;
            }

            if (((Rana.X == 0 && Rana.Y == 1) || (Rana.X == 3 && Rana.Y == 0)
                || (Rana.X == 1 && Rana.Y == 3) || (Rana.X == 2 && Rana.Y == 1)) && !_quedarseInmovil)
            {
                OnPropertyChanged("Rana");
                _quedarseInmovil = true;
                await Task.Delay(300); // Pausa de un 0.5 segundos
                _quedarseInmovil = false;
                Rana.Vida -= 1;
                Rana.X = 0;
                Rana.Y = 0;
                Rana.LimiteMovimientos = 20;
                _movimiento = "";
            }


            ///////////

            if (Rana.Vida == 0)
            {
                 PerdistePorVidas();
            }
            OnPropertyChanged();
        }
        private void Nivel3()
        {

            if (Rana.Vida == 0)
            {
                PerdistePorVidas();
            }








            OnPropertyChanged();
        }
        private void Nivel4()
        {
            OnPropertyChanged();

        }


        private async void PerdistePorVidas()
        {
            OnPropertyChanged(nameof(Rana));
            await Task.Delay(300); // Pausa de un 0.5 segundos
            Vista = Vista.Ganaste;
            GameOver = true;
        }



        #region CAMBIAR VISTA
        private void CambiarVista(Vista vista)
        {
            Vista = vista;
            OnPropertyChanged();
        }

        #endregion

        #region Notificar OnpropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        }
        #endregion

    }
}
