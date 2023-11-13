using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Juego_PA.ViewModel
{
    public static class MediadorViewModel
    {
        public static event Action? EliminarHojasEvent;
        public static void EliminarHojas()
        {
            EliminarHojasEvent?.Invoke();
        }
        public static event Action? Nivel2Event;
        public static void Nivel2()
        {
            Nivel2Event?.Invoke();
        }

        public static event Action? RegresarEvent;
        public static void Regresar()
        {
            RegresarEvent?.Invoke();
        }
        public static event Action? IniciarJuegoNivel1Event;

        public static void IniciarJuegoNivel1()
        {
            IniciarJuegoNivel1Event?.Invoke();
        }

        public static event Action? IniciarJuegoNivel2Event;

        public static void IniciarJuegoNivel2()
        {
            IniciarJuegoNivel2Event?.Invoke();
        }

        public static event Action? IniciarJuegoNivel3Event;

        public static void IniciarJuegoNivel3()
        {
            IniciarJuegoNivel3Event?.Invoke();
        }
        public static event Action? IniciarJuegoNivel4Event;

        public static void IniciarJuegoNivel4()
        {
            IniciarJuegoNivel4Event?.Invoke();
        }

        public static event Action? CambiarEscenarioLavaEvent;

        public static void CambiarEscenarioLava()
        {
            CambiarEscenarioLavaEvent?.Invoke();
        }
        public static event Action? CambiarEscenarioAguaEvent;

        public static void CambiarEscenarioAgua()
        {
            CambiarEscenarioAguaEvent?.Invoke();
        }

        public static event Action<bool>? EstadoTimerEvent;

        public static void EstadoTimer(bool estado)
        {
            EstadoTimerEvent?.Invoke(estado);
        }





    }
}
