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
using Laba.pacients;
using BarcodeStandard;
using System.Data.Entity;

namespace Laba.laborant
{
    /// <summary>
    /// Логика взаимодействия для TakeBiomaterial.xaml
    /// </summary>
    public partial class TakeBiomaterial : Window
    {
        public int Houers = 0, Minutes = 0, Seconds = 0;
        private DispatcherTimer timer;
        Biomaterials NewBio = new Biomaterials();

        users user = new users();
        Заказ orderNow = new Заказ();

        LaboratoryEntities13 db = new LaboratoryEntities13();
        List<Данные_пациентов> pac = new List<Данные_пациентов>();
        public TakeBiomaterial(users user)
        {
            InitializeComponent();
            this.user = user;
            timer = new DispatcherTimer();

            LoadTimeFromRegistry();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            pac = db.Данные_пациентов.ToList();
            for (int i = 0; i < pac.Count; i++)
            {
                listPacients.Items.Add(pac[i].ФИО);
            }

            Biomaterials bio = db.Biomaterials.OrderByDescending(b => b.id).FirstOrDefault();

            long num = Convert.ToInt64(bio.Code) + 1;
            Code.Text = Convert.ToString(num);
        }

        private void button1_Click(object sender, RoutedEventArgs e)//Выйти в меню входа
        {
            string subKeyName = "Timer";
            RegistryKey key = Registry.CurrentUser;
            if (key.OpenSubKey(subKeyName) != null)
                key.DeleteSubKeyTree(subKeyName);

            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)//Вернутся назад
        {
            laborant lb = new laborant(user);
            SaveTimeToRegistry();
            lb.Show();
            Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            AddPacients add = new AddPacients(user);
            SaveTimeToRegistry();
            add.Show();
            Close();
        }

        private void button4_Click(object sender, RoutedEventArgs e)//Добавить заказ
        {
            List<servi> ser = new List<servi>();
            Заказ ord = db.Заказ.OrderByDescending(b => b.ID_заказа).FirstOrDefault();

            if (cb1.IsChecked == true)//КАКОЙ ТО ЛЮТЫЙ ГАВНО КОД ДЛЯ ПРОВЕРКИ УСЛУГ(потом надо что то с ним придумать)
            {
                servi se = db.servi.Where(x => x.id == 1).FirstOrDefault();
                ser.Add(se);
            }
            if (cb2.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 2).FirstOrDefault();
                ser.Add(se);
            }
            if (cb3.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 3).FirstOrDefault();
                ser.Add(se);
            }
            if (cb4.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 4).FirstOrDefault();
                ser.Add(se);
            }
            if (cb5.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 5).FirstOrDefault();
                ser.Add(se);
            }
            if (cb6.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 6).FirstOrDefault();
                ser.Add(se);
            }
            if (cb7.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 7).FirstOrDefault();
                ser.Add(se);
            }
            if (cb8.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 8).FirstOrDefault();
                ser.Add(se);
            }
            if (cb9.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 9).FirstOrDefault();
                ser.Add(se);
            }
            if (cb10.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 10).FirstOrDefault();
                ser.Add(se);
            }
            if (cb11.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 11).FirstOrDefault();
                ser.Add(se);
            }
            if (cb12.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 12).FirstOrDefault();
                ser.Add(se);
            }
            if (cb13.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 13).FirstOrDefault();
                ser.Add(se);
            }
            if (cb14.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 14).FirstOrDefault();
                ser.Add(se);
            }
            if (cb15.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 15).FirstOrDefault();
                ser.Add(se);
            }
            if (cb16.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 16).FirstOrDefault();
                ser.Add(se);
            }
            if (cb17.IsChecked == true)
            {
                servi se = db.servi.Where(x => x.id == 17).FirstOrDefault();
                ser.Add(se);
            }//НЕУЖЕЛИ ЭТО КОНЕЦ ГАВНО КОДА

            if (Code.Text != "" && listPacients.Text != "" && ser.Count != 0)
            {
                Заказ order = new Заказ
                {
                    Дата_создания = DateTime.Today,
                    Статус_заказа = 3,
                    КодБиоматериала = NewBio.id,
                    idPacient = listPacients.SelectedIndex + 1
                };
                orderNow = order;
                db.Заказ.Add(order);
                db.SaveChanges();
            }

            for (int i = 0; i < ser.Count; i++)
            {
                StatusServices os = new StatusServices
                {
                    idOrder = orderNow.ID_заказа,
                    ServicesCode = ser[i].id,
                    Status = 3
                };
                db.StatusServices.Add(os);
                db.SaveChanges();
            }
            MessageBox.Show("Заказ добавлен!");
        }

        private void button5_Click(object sender, RoutedEventArgs e)//Добавить Биоматериал
        {
            try
            {
                long num = Convert.ToInt64(Code.Text);

                if (num < 100000000000000)
                {
                    MessageBox.Show("Неправильный формат!");
                    return;
                }
                else
                {
                    Biomaterials bio = new Biomaterials
                    {
                        Code = Code.Text
                    };
                    NewBio = bio;
                    db.Biomaterials.Add(bio);
                    db.SaveChanges();

                    Code.IsEnabled = false;
                    button5.IsEnabled = false;
                }
            }
            catch
            {
                MessageBox.Show("Код для биоматериала введен неверно!");
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)//Штрих-код
        {
            System.Drawing.Image image = null;
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = System.Drawing.Color.Black;
            b.IncludeLabel = true;
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
            System.Drawing.Font font = new System.Drawing.Font("verdana", 10f); 
            b.LabelFont = font;
            b.Height = 100;
            b.Width = 200;
            image = b.Encode(BarcodeLib.TYPE.CODE128C, NewBio.Code);
            image.Save($@"C:\tmp\barcode{NewBio.id}.png");
            MessageBox.Show("Штрих-код был добавлен!");
        }

//___________________________________________________ Таймер тут ________________________________________________________________
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
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Timer");
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
