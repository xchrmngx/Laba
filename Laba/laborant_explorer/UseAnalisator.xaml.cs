using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
    /// Логика взаимодействия для UseAnalisator.xaml
    /// </summary>
    public partial class UseAnalisator : Window
    {
        public int Houers = 0, Minutes = 0, Seconds = 0;
        private DispatcherTimer timer;
        private DispatcherTimer analizator;

        users user = new users();

        LaboratoryEntities13 db = new LaboratoryEntities13();
        List<Заказ> order = new List<Заказ>();
        List<services> servi = new List<services>();
        List<Biomaterials> bio = new List<Biomaterials>();
        List<StatusServices> ser;
        Заказ ord = new Заказ();

        GetAnalizator getAnalizator = new GetAnalizator();

        string name = "";
        public UseAnalisator(users user)
        {
            InitializeComponent();
            this.user = user;
            timer = new DispatcherTimer();
            analizator = new DispatcherTimer();

            LoadTimeFromRegistry();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            order = db.Заказ.ToList();
            for (int i = 0; i < order.Count; i++) 
            {
                cb1.Items.Add(order[i].Biomaterials.Code);
            }
            servi = db.servi.ToList();
            bio = db.Biomaterials.ToList();
        }

        private void listAdd()
        {
            listServices.Items.Clear();
            Biomaterials b = bio.Find(x => x.Code == cb1.Text);
            ord = db.Заказ.Where(x => x.КодБиоматериала == b.id).FirstOrDefault();
            ser = db.StatusServices.Where(x => x.idOrder == ord.ID_заказа).ToList();
            for (int i = 0; i <  ser.Count; i++)
            {
                int a = Convert.ToInt32(ser[i].ServicesCode);
                listServices.Items.Add($"Услуга: {servi[a - 1].Service} Статус: {ser[i].Статус_Услуги.Наименование}");
            }
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
            laborant_explorer la = new laborant_explorer(user);
            SaveTimeToRegistry();
            la.Show();
            Close();
        }

        private void cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            listAdd();
        }

        private void button4_Click(object sender, RoutedEventArgs e)//Отправить на анализатор
        {
            Import();
        }

        private void button5_Click(object sender, RoutedEventArgs e)//Одобрить результат
        {
            listServices.Items.Add("Все результаты сохранены!");
        }

        private void Import()
        {
            List<Services> serviFirst = new List<Services>();
            List<Services> serviSecond = new List<Services>();

            if (rb1.IsChecked == true)
                name = rb1.Content.ToString();
            else
                name = rb2.Content.ToString();

            for (int i = 0; i < ser.Count; i++)
            {
                int analiz = ser[i].servi.Приборы.id;
                if (analiz == 1 || analiz == 3)
                {
                    int code = Convert.ToInt32(servi[Convert.ToInt32(ser[i].ServicesCode) - 1].Code);
                    Services service = new Services();
                    service.serviceCode = code;
                    serviFirst.Add(service);
                }
                else
                {
                    int code = Convert.ToInt32(servi[Convert.ToInt32(ser[i].ServicesCode) - 1].Code);
                    Services service = new Services();
                    service.serviceCode = code;
                    serviSecond.Add(service);
                }
            }
            string pacient = ord.idPacient.ToString();

            if (serviFirst != null && rb1.IsChecked == true)
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5000/api/analyzer/Ledetect");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    List<Services> services = serviFirst;
                    string patient = pacient;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            patient,
                            services
                        });
                        streamWriter.Write(json);
                    }

                    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        MessageBox.Show("Услуги успешно отправленны!");
                    else
                        MessageBox.Show("Ошибка отправки!");

                    analizator.Interval = TimeSpan.FromSeconds(30);
                    analizator.Tick += Analizator_Tick;
                    analizator.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Такой запрос не нужен!");

            if (serviSecond != null && rb2.IsChecked == true)
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5000/api/analyzer/Biorad");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    List<Services> services = serviSecond;
                    string patient = pacient;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            patient,
                            services
                        });
                        streamWriter.Write(json);
                    }

                    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        MessageBox.Show("Услуги успешно отправленны!");
                    else
                        MessageBox.Show("Ошибка отправки!");

                    analizator.Interval = TimeSpan.FromSeconds(30);
                    analizator.Tick += Analizator_Tick;
                    analizator.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Такой запрос не нужен!");
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            listServices.Items.Clear();
            listServices.Items.Add("Повторная отправка!");
            Import();
        }

        private void GetInfo()
        {
            if (rb1.IsChecked == true)
                name = rb1.Content.ToString();
            else
                name = rb2.Content.ToString();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5000/api/analyzer/{name}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string json = reader.ReadToEnd();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        getAnalizator = serializer.Deserialize<GetAnalizator>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (getAnalizator != null)
            {
                listServices.Items.Add($"Пациент: {getAnalizator.patient}");

                for (int i = 0; i < getAnalizator.services.Count; i++)
                {
                    if (getAnalizator.services[i].result != null)
                    {
                        listServices.Items.Add($"Услуга: {getAnalizator.services[i].serviceCode}");
                        listServices.Items.Add($"Результат: {getAnalizator.services[i].result}");

                        int index = ord.ID_заказа;
                        int indexServices = getAnalizator.services[i].serviceCode;
                        servi s = servi.Find(x => x.Code == indexServices);
                        StatusServices ss = db.StatusServices.Where(x => x.idOrder == index && x.ServicesCode == s.id).FirstOrDefault();
                        ss.Result = getAnalizator.services[i].result;
                        ss.Status = 1;
                        db.SaveChanges();
                        analizator.Stop();
                    }
                    else
                    {
                        listServices.Items.Add($"Услуга {getAnalizator.services[i].serviceCode} не готова");
                    }
                }
                if (getAnalizator.progress == 100)
                {
                    analizator.Stop();
                    rb1.IsEnabled = true;
                    rb2.IsEnabled = true;
                }
            }
        }

        //____________________________________________ Таймер для анализатора_____________________________________________________________________________

        private void Analizator_Tick(object sender, EventArgs e)
        {
            GetInfo();
        }

        //____________________________________________ Таймер для лаборанта_______________________________________________________________________________
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
