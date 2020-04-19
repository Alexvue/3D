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
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Class
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool is_pressed_W = false;
        bool is_pressed_A = false;
        bool is_pressed_S = false;
        bool is_pressed_D = false;
        bool is_pressed_LeftShift = false;
        bool is_pressed_Space = false;
        double latitude, longitude;
        Point prev_mouse_pos;

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 25)
            };
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
            SetupCam();
        }

        private void SetupCam()
        {
            double x = cam.LookDirection.X;
            double y = cam.LookDirection.Y;
            double z = cam.LookDirection.Z;
                
            double r = Math.Sqrt(x * x + y * y + z * z);
            latitude = Math.Acos(y / r) * 180 / Math.PI;
            longitude = Math.Atan(x / z) * 180 / Math.PI + 180;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            double rot = 0;
            double up_down = 0;

            if (is_pressed_A)
            {
                rot = 90;
            }
            if (is_pressed_D)
            {
                rot = -90;
            }
            if (is_pressed_W)
            {
                rot = rot / 2;
            }
            if (is_pressed_S)
            {
                rot = 180 - rot / 2;
            }
            if (is_pressed_LeftShift)
            {
                up_down = 1;
            }
            if (is_pressed_Space)
            {
                up_down = -1;
            }

            rot = (longitude - rot) % 360;

            if (is_pressed_W || is_pressed_S || is_pressed_A || is_pressed_D)
            {
                double r_x = Math.Cos(rot * (Math.PI / 180));
                double r_z = Math.Sin(rot * (Math.PI / 180));
                cam.Position = new Point3D(cam.Position.X + r_x / 10,
                cam.Position.Y + up_down / 10,
                cam.Position.Z + r_z / 10);
            }else if (is_pressed_LeftShift || is_pressed_Space)
            {
               cam.Position = new Point3D(cam.Position.X,
               cam.Position.Y + up_down / 10,
               cam.Position.Z);
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W) is_pressed_W = true;
            if (e.Key == Key.S) is_pressed_S = true;
            if (e.Key == Key.A) is_pressed_A = true;
            if (e.Key == Key.D) is_pressed_D = true;
            if (e.Key == Key.LeftShift) is_pressed_LeftShift = true;
            if (e.Key == Key.Space) is_pressed_Space = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            prev_mouse_pos = e.GetPosition(this);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point curr_mouse_pos = e.GetPosition(this);

                double dx = curr_mouse_pos.X - prev_mouse_pos.X;
                double dy = curr_mouse_pos.Y - prev_mouse_pos.Y;

                longitude = (longitude + dx) % 360;
                latitude = latitude + dy;
                if (latitude < 0)
                {
                    latitude = 0;
                }
                if (latitude > 180)
                {
                    latitude = 180;
                }

                double p_x = Math.Sin(latitude * (Math.PI / 180)) * Math.Cos(longitude * (Math.PI / 180));
                double p_y = Math.Sin(latitude * (Math.PI / 180)) * Math.Sin(longitude * (Math.PI / 180));
                double p_z = Math.Cos(latitude * (Math.PI / 180));
                cam.LookDirection = new Vector3D(p_x, p_z, p_y);

                prev_mouse_pos = curr_mouse_pos;
            }
                
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W) is_pressed_W = false;
            if (e.Key == Key.S) is_pressed_S = false;
            if (e.Key == Key.A) is_pressed_A = false;
            if (e.Key == Key.D) is_pressed_D = false;
            if (e.Key == Key.LeftShift) is_pressed_LeftShift = false;
            if (e.Key == Key.Space) is_pressed_Space = false;
        }
    }
}
