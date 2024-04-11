using Laba.laborant;
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
using CheckTrue;

namespace Laba.pacients
{
    /// <summary>
    /// Логика взаимодействия для AddPacients.xaml
    /// </summary>
    public partial class AddPacients : Window
    {
        public int Houers = 0, Minutes = 0, Seconds = 0;
        private DispatcherTimer timer;

        users user = new users();

        LaboratoryEntities13 db = new LaboratoryEntities13();
        List<Данные_о_страховых_компаниях> str = new List<Данные_о_страховых_компаниях>();
        List<TypePolis> pol = new List<TypePolis>();

        public AddPacients(users user)
        {
            InitializeComponent();
            this.user = user;
            str = db.Данные_о_страховых_компаниях.ToList();
            for (int i = 0; i < str.Count; i++)
            {
                StrCom.Items.Add(str[i].Название_страховой_компании);
            }
            pol = db.TypePolis.ToList();
            for (int i = 0; i < pol.Count; i++)
            {
                TipeStrah.Items.Add(pol[i].Name);
            }

            timer = new DispatcherTimer();

            LoadTimeFromRegistry();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void button1_Click(object sender, RoutedEventArgs e)//Выйти в меню входа
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)//Вернутся назад
        {
            TakeBiomaterial tb = new TakeBiomaterial(user);
            SaveTimeToRegistry();
            tb.Show();
            Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)//Добавить пациента
        {
            if (FIO.Text != "" && BirDay.Text != "" && Seria.Text != "" && Number.Text != "" && Phone.Text != "" && Polis.Text != "")
            {//НАДО РЕШИТЬ ПРОБЛЕМУ С БИБЛИОТЕКОЙ КЛАССОВ
                Данные_пациентов paci = new Данные_пациентов
                {
                    ФИО = FIO.Text,
                    Дата_рождения = BirDay.SelectedDate,
                    Серия_паспорта = Seria.Text,
                    Номер_паспорта = Number.Text,
                    Номер_телефона = Phone.Text,
                    Электронная_почта = Email.Text,
                    Номер_страхового_полиса = Polis.Text,
                    Тип_страхового_полиса = TipeStrah.SelectedIndex + 1,
                    Страховая_компания = StrCom.SelectedIndex + 1
                };
                db.Данные_пациентов.Add(paci);
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Данные были введены не верно!");
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
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Timer");
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
