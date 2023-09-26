using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juego_PA.ViewModel
{
    public static class MediadorViewModel
    {
        public static event Action? IniciarJuegoEvent;

        public static void IniciarJuego()
        {
            IniciarJuegoEvent?.Invoke();
        }
        public static event Action? PintarBordesEvent;
        public static void PintarBordes()
        {
            PintarBordesEvent?.Invoke();
        }


        public static event Action? RegresarEvent;
        public static void Regresar()
        {
            RegresarEvent?.Invoke();
        }
    }
}
