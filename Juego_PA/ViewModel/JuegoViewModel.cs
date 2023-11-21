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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Juego_PA.ViewModel
{
    public class JuegoViewModel : INotifyPropertyChanged
    {
        Regex? regex;

        private string _patron = "";


        public string Llaves { get; set; } = "";
        public bool LlavesConseguidas { get; set; } = false;
        public string MensajeLlaves { get; set; } = "";
        public bool MostrarMensaje { get; set; } = false;



        public Rana Rana { get; set; } = new();
        public Jugador Jugador { get; set; } = new();
        public bool Ganaste { get; set; } = false;
        public bool GameOver { get; set; } = false;
        public bool IniciarJuegoPropiedad { get; set; } = false;
        public bool Escenario2 { get; set; } = false;
        public int NivelActual { get; set; }
        public Vista Vista { get; set; } = Vista.Inicio;
        public ICommand MoverCommand { get; set; }
        public ICommand IniciarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand Nivel2Command { get; set; }
        public ICommand MoverRanaOrigen => new RelayCommand(MoverRana);
        public ICommand OcultarTeclaIncorrectaCommand => new RelayCommand<string>(OcultarTeclaIncorrectaMetodo);
        public ICommand RestarVidaRanaCommand => new RelayCommand(RestarVidaRana);
        public ICommand SumarVidaRanaCommand => new RelayCommand(SumarVidaRana);
        public ICommand SumarPuntajeCommand => new RelayCommand(SumarPuntaje);
        public ICommand ConseguirLlaveCommand => new RelayCommand(ConseguirLlave);
        public ICommand MensajeLLaveCommand => new RelayCommand(MensajeLLave);

        private void MensajeLLave()
        {
            MostrarMensaje = false;
            MediadorViewModel.EstadoTimer(true);
            OnPropertyChanged();
        }

        private void ConseguirLlave()
        {
            if (Llaves != "LLL")
            {
                Llaves += "L";
                MensajeLlaves = "Has conseguido " + (Llaves.Length == 1 ? "una llave." :
                Llaves.Length == 2 ? "dos llaves. \n  Ya puedes ir al siguiente mapa." : "todas las llaves \n Ya puedes conseguir la recompensa final.");

                if (Llaves == "LLL")
                {
                    MediadorViewModel.DesbloquearFlor();
                    LlavesConseguidas = true;
                }

                MostrarMensaje = true;
            }

            OnPropertyChanged();
        }














        private void SumarPuntaje()
        {
            Rana.Puntaje += 100;
            OnPropertyChanged(nameof(Rana));
        }

        private void SumarVidaRana()
        {
            if (Rana.Vida < 6)
                Rana.Vida++;
            OnPropertyChanged(nameof(Rana));
        }

        private void RestarVidaRana()
        {
            Rana.Vida--;
            if (Rana.Vida == 0)
            {
                PerdistePorVidas();
                MediadorViewModel.EstadoTimer(false);

            }
            OnPropertyChanged(nameof(Rana));
        }

        public bool OcultarTeclaIncorrecta { get; set; } = true;

        private void OcultarTeclaIncorrectaMetodo(string p)
        {
            if (p == "ocultar")
            {
                MediadorViewModel.EstadoTimer(true);
                OcultarTeclaIncorrecta = true;
            }
            else
            {
                MediadorViewModel.EstadoTimer(false);
                OcultarTeclaIncorrecta = false;
            }

            OnPropertyChanged();
        }

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



        public async void RealizarMovimiento(Jugador jugador)
        {
            Rana.LimiteMovimientos -= 1;

            if (Rana.LimiteMovimientos > 0 && !_quedarseInmovil)
            {
                _posicionXAnterior = Rana.X;
                _posicionYAnterior = Rana.Y;

                if (jugador.Movimiento == Movimientos.Derecha)
                {
                    Rana.X += 1;
                    _movimientoPermitido = "D";
                    _movimiento += "D";
                }
                else if (jugador.Movimiento == Movimientos.Izquierda)
                {
                    Rana.X -= 1;
                    _movimientoPermitido = "I";
                    _movimiento += "I";
                }
                else if (jugador.Movimiento == Movimientos.Arriba)
                {
                    Rana.Y -= 1;
                    _movimientoPermitido = "A";
                    _movimiento += "A";
                }
                else if (jugador.Movimiento == Movimientos.Abajo)
                {
                    Rana.Y += 1;
                    _movimientoPermitido = "B";
                    _movimiento += "B";
                }

                if (Rana.X >= 5)
                    Rana.X = 4;
                if (Rana.Y >= 5)
                    Rana.Y = 4;



                if (jugador.Nivel == 3 && jugador.Escenario == 1)
                {

                    if ((Rana.X == 1 && Rana.Y == 0) ||
                        (Rana.X == 3 && Rana.Y == 0) ||
                        (Rana.X == 4 && Rana.Y == 0) ||
                        (Rana.X == 1 && Rana.Y == 1) ||
                        (Rana.X == 0 && Rana.Y == 4) ||
                        (Rana.X == 3 && Rana.Y == 2) ||
                        (Rana.X == 4 && Rana.Y == 4) ||
                        (Rana.X == 1 && Rana.Y == 3)
                        )
                    {
                        Rana.X = _posicionXAnterior;
                        Rana.Y = _posicionYAnterior;
                    }
                    NivelActual = 3;
                    Nivel3();
                }
                if (jugador.Nivel == 3 && jugador.Escenario == 2)
                {

                    if ((Rana.Y == 0 && Llaves != "LLL") ||
                      (((Rana.X == 2 && Rana.Y == 1) ||
                      (Rana.X == 2 && Rana.Y == 2) ||
                      (Rana.X == 2 && Rana.Y == 3) ||
                      (Rana.X == 2 && Rana.Y == 4) ||
                      (Rana.X == 3 && Rana.Y == 4) ||
                      (Rana.X == 4 && Rana.Y == 4) || Rana.Y == 0) && Llaves == "LLL"))
                    {
                        Nivel3();
                        NivelActual = 3;


                    }
                    else
                    {
                        Rana.X = _posicionXAnterior;
                        Rana.Y = _posicionYAnterior;
                    }
                }

                switch (jugador.Nivel)
                {
                    case 1:
                        NivelActual = 1;
                        Nivel1();
                        break;
                    case 2:
                        NivelActual = 2;
                        Nivel2();
                        break;
                    case 4:
                        NivelActual = 4;
                        Nivel4();
                        break;
                }
                //CAMBIO DE ESCENARIO LAVA
                if (jugador.Nivel == 3 && Rana.X == 4 && Rana.Y == 3 && jugador.Escenario != 2)
                {
                    if (Llaves.Length == 2)
                    {
                        _quedarseInmovil = true;
                        await Task.Delay(50); // Pausa de un 0.5 segundos
                        Rana.X = 0;
                        Rana.Y = 0;
                        _quedarseInmovil = false;
                        jugador.Escenario = 2;
                        Jugador = jugador;
                        MediadorViewModel.CambiarEscenarioLava();
                    }
                    else
                    {
                        MensajeLlaves = "No puedes avanzar aún te " + (Llaves.Length == 1 ? "falta una llave" : "faltan dos llaves");
                        MostrarMensaje = true;
                    }
                }

            }
            // PERDISTE POR USAR TODOS LOS MOVIMIENTOS
            if (Rana.LimiteMovimientos == 0)
            {
                Vista = Vista.Ganaste;
                GameOver = true;
                IniciarJuegoPropiedad = false;
                MediadorViewModel.EstadoTimer(false);
            }

            OnPropertyChanged();
        }


        public void IniciarJuego(string nivel)
        {

            Rana.X = 0;
            Rana.Y = 0;
            Rana.Puntaje = 0;
            Ganaste = false;
            GameOver = false;
            _patron = "";

            IniciarJuegoPropiedad = true;

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
                MediadorViewModel.CambiarEscenarioAgua();
                Rana.LimiteMovimientos = 50;
                Jugador.Escenario = 1;
                MostrarMensaje = false;
                Llaves = "";

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
                IniciarJuegoPropiedad = false;

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
                IniciarJuegoPropiedad = false;

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
        private async void Nivel3()
        {

            if (Rana.Vida == 0)
            {
                PerdistePorVidas();
                MediadorViewModel.EstadoTimer(false);
            }


            if (Rana.X == 4 && Rana.Y == 4 && Jugador.Escenario == 2)
            {
                OnPropertyChanged("Rana");
                await Task.Delay(200); // Pausa de un 0.5 segundos
                Vista = Vista.Ganaste;
                Rana.LimiteMovimientos = 0;
                NivelActual = 3;
                IniciarJuegoPropiedad = false;
            }





            OnPropertyChanged();
        }
        private void Nivel4()
        {
            OnPropertyChanged();

        }


        private void PerdistePorVidas()
        {

            Vista = Vista.Ganaste;
            GameOver = true;
            IniciarJuegoPropiedad = false;
        }



        #region CAMBIAR VISTA
        private void CambiarVista(Vista vista)
        {
            Vista = vista;

            if (vista == Vista.Nivel1 || vista == Vista.Nivel2 || vista == Vista.Nivel3)
                IniciarJuegoPropiedad = true;
            else
                IniciarJuegoPropiedad = false;

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
