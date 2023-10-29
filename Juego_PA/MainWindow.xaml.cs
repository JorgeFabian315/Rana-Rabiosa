using Juego_PA.ViewModel;
using Juego_PA.Views;
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

namespace Juego_PA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new();  
        public MainWindow()
        {
            InitializeComponent();

            //timer.Interval = TimeSpan.FromSeconds(1.5);
            //timer.Tick += Timer_Tick;
            //timer.Start();


        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            //PrincipallView principal = new();
            //principal.Show();
            //this.Close();
            //timer.Stop();
        }
    }
}
