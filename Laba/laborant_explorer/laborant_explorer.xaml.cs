using Microsoft.Win32;
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

namespace Laba.laborant_explorer
{
    /// <summary>
    /// Логика взаимодействия для laborant_explorer.xaml
    /// </summary>
    public partial class laborant_explorer : Window
    {
        public int Houers = 0, Minutes = 0, Seconds = 0;
        private DispatcherTimer timer;

        LaboratoryEntities13 db = new LaboratoryEntities13();
        users user = new users();
        public laborant_explorer(users user)
        {
            InitializeComponent();
            this.user = user;
            FIO.Content = user.name;
            timer = new DispatcherTimer();

            LoadTimeFromRegistry();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            users NewUser = db.users.Where(x => x.id == user.id).FirstOrDefault();
            NewUser.lastenter = DateTime.Today;
            db.SaveChanges();
        }

        private void button1_Click(object sender, RoutedEventArgs e)//Использовать анализатор
        {
            UseAnalisator ua = new UseAnalisator(user);
            SaveTimeToRegistry();
            ua.Show();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)//Выйти в меню входа
        {
            string subKeyName = "Timer";
            RegistryKey key = Registry.CurrentUser;
            if (key.OpenSubKey(subKeyName) != null)
                key.DeleteSubKeyTree(subKeyName);

            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
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
            {
                SetBlock();

                Application.Current.Shutdown();
            }
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

        private int secSave = 0;
        private int minSave = 0;
        private int hourSave = 0;

        private void SetBlock()
        {
            secSave = GetSeconds();
            minSave = GetMinutes();
            hourSave = GetHours();
            CreateRegistryTime(secSave, minSave, hourSave);
        }
        private void CreateRegistryTime(int sec, int min, int hour)
        {
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey GIBDD_KEY = currentUserKey.CreateSubKey("GIBDD_TIME");
            GIBDD_KEY.SetValue("seconds", sec.ToString());
            GIBDD_KEY.SetValue("minutes", min.ToString());
            GIBDD_KEY.SetValue("hours", hour.ToString());
            GIBDD_KEY.Close();
        }

        private int GetSeconds()
        {
            return Convert.ToInt32(DateTime.Now.ToString().Substring((DateTime.Now.ToString().Length - 2), 2)); ;
        }
        private int GetMinutes()
        {
            return Convert.ToInt32(DateTime.Now.ToString().Substring((DateTime.Now.ToString().Length - 5), 2));
        }
        private int GetHours()
        {
            return Convert.ToInt32(DateTime.Now.ToString().Substring((DateTime.Now.ToString().Length - 8), 2));
        }
    }
}
