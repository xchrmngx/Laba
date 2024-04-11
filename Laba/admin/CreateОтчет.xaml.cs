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
using System.Windows.Shapes;
using System.Windows.Threading;
using Laba.laborant;
using Microsoft.Win32;

namespace Laba.admin
{
    /// <summary>
    /// Логика взаимодействия для CreateОтчет.xaml
    /// </summary>
    public partial class CreateОтчет : Window
    {
        public int Houers = 0, Minutes = 0, Seconds = 0;
        private DispatcherTimer timer;

        users user = new users();
        public CreateОтчет(users user)
        {
            InitializeComponent();
            this.user = user;

            if (user != null)
            {
                LoadTimeFromRegistry();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)//Выйти в меню входа
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)//Вернутся назад
        {
            if (user != null)
            {
                laborant.laborant lb = new laborant.laborant(user);
                SaveTimeToRegistry();
                lb.Show();
                Close();
            }
            else
            {
                admin ad = new admin();
                ad.Show();
                Close();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Seconds++;
            if (Seconds == 60)
            {
                Minutes++;
                Seconds = 0;
            }
            if (Minutes == 60)
            {
                Houers++;
                Minutes = 0;
            }
            Timer.Content = $"Время за работой: {Houers}:{Minutes}:{Seconds}";

            if (Houers == 2 && Minutes == 15)
                MessageBox.Show("Осталось 15 минут!");

            if (Houers == 2 && Minutes == 30)
                Application.Current.Shutdown();
        }

        private void SaveTimeToRegistry()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Timer");
            key.SetValue("Houers", Houers);
            key.SetValue("Minutes", Minutes);
            key.SetValue("Seconds", Seconds);
            key.Close();
        }

        private void LoadTimeFromRegistry()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Timer");
            if (key != null)
            {
                Houers = (int)key.GetValue("Houers", 0);
                Minutes = (int)key.GetValue("Minutes", 0);
                Seconds = (int)key.GetValue("Seconds", 0);
                key.Close();
            }
        }
    }
}
