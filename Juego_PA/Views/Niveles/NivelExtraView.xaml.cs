using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para NivelExtraView.xaml
    /// </summary>
    public partial class NivelExtraView : UserControl
    {
        bool goRight, goLeft;

        List<System.Windows.Shapes.Rectangle> itemstoremove = new();

        int totalEnemigos = 0;
        int velocidadEnemigos = 6;
        int velocidadRana = 7;
        int bulletTimer;
        int bulletTimerLimit = 90;


        int enemyImages = 0;
        int totalEnemeis;
        int enemySpeed = 6;

        DispatcherTimer timer = new();
        public NivelExtraView()
        {
            InitializeComponent();
            timer.Interval += TimeSpan.FromMilliseconds(20);
            timer.Start();
            timer.Tick += Timer_Tick;
            this.Focus();
            makeEnemies(10);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Rect player = new Rect(Canvas.GetLeft(Rana), Canvas.GetTop(Rana), Rana.Width, Rana.Height);

            var IzquierdaRana = Canvas.GetLeft(Rana);
            if (goLeft && IzquierdaRana > 40)
            {
                Canvas.SetLeft(Rana, IzquierdaRana - 10);
            }
            if (goRight && IzquierdaRana + 120 < MyCanvas.ActualWidth)
            {
                Canvas.SetLeft(Rana, IzquierdaRana + 10);
            }


            bulletTimer -= 3;
            //if (bulletTimer < 0)
            //{
            //    enemyBulletMaker((Canvas.GetLeft(Rana) + 20), 10);
            //    bulletTimer = bulletTimerLimit;
            //}

            foreach (var x in MyCanvas.Children.OfType<System.Windows.Shapes.Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);
                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Canvas.GetTop(x) < MyCanvas.ActualHeight + 50)
                    {
                        itemstoremove.Add(x);
                    }
                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (bullet.IntersectsWith(enemy))
                            {
                                itemstoremove.Add(x);
                                itemstoremove.Add(y);
                                totalEnemeis -= 1;
                            }
                        }
                    }
                }
            }
        }

            private void btnArriba_Click(object sender, RoutedEventArgs e)
            {

            }

            private void btnIzquierda_Click(object sender, RoutedEventArgs e)
            {

            }

            private void btnDerecha_Click(object sender, RoutedEventArgs e)
            {

            }

            private void btnAbajo_Click(object sender, RoutedEventArgs e)
            {

            }

            private void UserControl_Loaded(object sender, RoutedEventArgs e)
            {
                this.Focus();
            }

            private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
            {

            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {

            }

            private void UserControl_KeyDown(object sender, KeyEventArgs e)
            {
                this.Focus();

                if (e.Key == Key.Left || e.Key == Key.A)
                {
                    goLeft = true;
                }
                if (e.Key == Key.Right || e.Key == Key.D)
                {
                    goRight = true;
                }
            }

            private void UserControl_KeyUp(object sender, KeyEventArgs e)
            {
                this.Focus();

                if (e.Key == Key.Left || e.Key == Key.A)
                {
                    goLeft = false;
                }
                if (e.Key == Key.Right || e.Key == Key.D)
                {
                    goRight = false;
                }

                if (e.Key == Key.Space)
                {
                    System.Windows.Shapes.Rectangle bala = new()
                    {
                        Tag = "bullet",
                        Height = 20,
                        Width = 5,
                        Fill = Brushes.White,
                        Stroke = Brushes.Red
                    };

                    Canvas.SetTop(bala, Canvas.GetTop(Rana) - bala.Height);
                    Canvas.SetLeft(bala, Canvas.GetLeft(Rana) + bala.Width / 2);

                    MyCanvas.Children.Add(bala);
                }

            }
            private void enemyBulletMaker(double x, double y)
            {
                System.Windows.Shapes.Rectangle newEnemyBullet = new()
                {
                    Tag = "enemyBullet",
                    Height = 40,
                    Width = 15,
                    Fill = Brushes.Yellow,
                    Stroke = Brushes.Black,
                    StrokeThickness = 5
                };
                Canvas.SetTop(newEnemyBullet, y);
                Canvas.SetLeft(newEnemyBullet, x);
                MyCanvas.Children.Add(newEnemyBullet);
            }
            private void makeEnemies(int limit)
            {
                int left = 700;
                totalEnemeis = limit;
                for (int i = 0; i < limit; i++)
                {
                    ImageBrush enemySkin = new ImageBrush();
                    System.Windows.Shapes.Rectangle newEnemy = new()
                    {
                        Tag = "enemy",
                        Height = 45,
                        Width = 45,
                        Fill = enemySkin,
                    };
                    Canvas.SetTop(newEnemy, 10);
                    Canvas.SetLeft(newEnemy, left);
                    MyCanvas.Children.Add(newEnemy);
                    left -= 60;
                    enemyImages++;
                    if (enemyImages > 8)
                    {
                        enemyImages = 1;
                    }

                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Assets/Nivel4/mosca.png"));
                }
            }
        }
    }

