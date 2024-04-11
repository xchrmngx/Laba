using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Laba.admin;
using Laba.bugalter;
using Laba.laborant;
using Laba.laborant_explorer;
using Microsoft.Win32;
using CheckTrue;

namespace Laba
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LaboratoryEntities14 db = new LaboratoryEntities14();
        bool IsTrue = true;
        public string captchaText = "";
        public MainWindow()
        {
            InitializeComponent();
            textBox2.FontFamily = new System.Windows.Media.FontFamily("Wingdings 2");
            Captcha.Source = GenerateCaptcha(100, 100);

            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += timer1_Tick;
            timer1.Start();
            DeleteTimeUser();
            GetRegistryTime();
        }
        private void button1_Click(object sender, RoutedEventArgs e) //Скрывалка
        {

            if (IsTrue)
            {
                textBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
                IsTrue = false;
            }
            else
            {
                textBox2.FontFamily = new System.Windows.Media.FontFamily("Wingdings 2");
                IsTrue = true;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e) //Вход
        {
            var user = db.users.FirstOrDefault(x => x.login == textBox1.Text);
            var admin = db.Администратор.FirstOrDefault(x => x.Логин == textBox1.Text && x.Пароль == textBox2.Text);

            if (user != null)
            {
                if (textBox12.Text == captchaText || Captcha.Visibility == Visibility.Hidden)
                {
                    if (user.password == textBox2.Text)
                    {
                        if (user.type == 1 && IsCanLab == true)
                        {
                            user.TryEnter = "Успешно";
                            laborant.laborant lab = new laborant.laborant(user);
                            lab.Show();
                            Close();
                        }
                        else if (user.type == 2 && IsCanLab == true)
                        {
                            user.TryEnter = "Успешно";
                            laborant_explorer.laborant_explorer labisl = new laborant_explorer.laborant_explorer(user);
                            labisl.Show();
                            Close();
                        }
                        else if (user.type == 3)
                        {
                            user.TryEnter = "Успешно";
                            bugalter.bugalter bux = new bugalter.bugalter(user);
                            bux.Show();
                            Close();
                        }
                        else
                            MessageBox.Show("30 минут еще не прошло!");
                    }
                    else
                    {
                        user.TryEnter = "Не успешно";
                        MessageBox.Show("Не верный пароль");
                    }
                }
                else
                {
                    MessageBox.Show("Капча была введена неправильно, попробуйте через 10 секунд!");
                    textBox12.Text = "";
                    CaptchaBan();
                }
            }
            else if (admin != null)
            {
                admin.admin ad = new admin.admin();
                ad.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Такого пользователя не существует, введите капчу!");
                Captcha.Source = GenerateCaptcha(100, 100);
                textBox12.Visibility = Visibility.Visible;
                label1.Visibility = Visibility.Visible;
                Captcha.Visibility = Visibility.Visible;
                Captchabutton.Visibility = Visibility.Visible;
            }
        }

//___________________________________________ Капча ____________________________________________________________________________________
        private const string CaptchaText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly Random _random = new Random();

        public BitmapSource GenerateCaptcha(int width, int height)
        {
            using (var bitmap = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var font = new Font("Arial", 20, System.Drawing.FontStyle.Bold);
                var brush = new SolidBrush(System.Drawing.Color.Black);
                var pen = new System.Drawing.Pen(System.Drawing.Color.Black, 2);

                // Generate random captcha text
                var captchaChars = new char[6];
                for (var i = 0; i < captchaChars.Length; i++)
                {
                    captchaChars[i] = CaptchaText[_random.Next(CaptchaText.Length)];
                }
                var captchaText = new string(captchaChars);

                // Draw captcha text
                for (int i = 0; i < captchaChars.Length; i++)
                {
                    // Randomly choose the vertical position for each character within a specified range
                    int minHeight = (int)(0.2 * height); // Minimum height for character position
                    int maxHeight = (int)(0.8 * height); // Maximum height for character position
                    float charHeight = _random.Next(minHeight, maxHeight);

                    graphics.DrawString(captchaChars[i].ToString(), font, brush, i * font.Size, charHeight);
                }

                // Add noise lines
                for (var i = 0; i < 5; i++)
                {
                    graphics.DrawLine(pen, _random.Next(width), _random.Next(height), _random.Next(width), _random.Next(height));
                }

                // Add noise effect (stripes)
                for (var x = 0; x < bitmap.Width; x++)
                {
                    for (var y = 0; y < bitmap.Height; y++)
                    {
                        if (_random.Next(10) < 3) // Adjust the number for density of noise
                        {
                            bitmap.SetPixel(x, y, System.Drawing.Color.Black);
                        }
                    }
                }

                // Convert bitmap to BitmapSource
                var handle = bitmap.GetHbitmap();
                try
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(handle);
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Captcha.Source = GenerateCaptcha(100, 100);
        }
//_________________________________________ Для проверки блокировки ___________________________________________________________

        DispatcherTimer timer1 = new DispatcherTimer();
        private int secSave = 0;
        private int minSave = 0;
        private int hourSave = 0;
        private int secCur = 0;
        private int minCur = 0;
        private int hourCur = 0;
        private int lastSeconds = 0;
        bool IsCanLab = true;

        private void timer1_Tick(object sender, EventArgs e)
        {
            secCur = GetSeconds();
            minCur = GetMinutes();
            hourCur = GetHours();
            lastSeconds = 1800 - ((hourCur * 3600 + minCur * 60 + secCur) - (hourSave * 3600 + minSave * 60 + secSave));
            if (lastSeconds <= 0)
            {
                IsCanLab = true;

                string subKeyName = "BanTimer";
                RegistryKey key = Registry.CurrentUser;
                if (key.OpenSubKey(subKeyName) != null)
                    key.DeleteSubKeyTree(subKeyName);
            }
        }

        private void DeleteTimeUser()
        {
            string subKeyName = "Timer";
            RegistryKey key = Registry.CurrentUser;
            
            if (key.OpenSubKey(subKeyName) != null)
            {
                key.DeleteSubKeyTree(subKeyName);
            }
        }

        private void GetRegistryTime()
        {
            if (Registry.CurrentUser.OpenSubKey("BanTimer") == null)
            {
                return;
            }
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey GIBDD_KEY = currentUserKey.OpenSubKey("BanTimer");
            secSave = Convert.ToInt32(GIBDD_KEY.GetValue("seconds"));
            minSave = Convert.ToInt32(GIBDD_KEY.GetValue("minutes"));
            hourSave = Convert.ToInt32(GIBDD_KEY.GetValue("hours"));
            GIBDD_KEY.Close();
            secCur = GetSeconds();
            minCur = GetMinutes();
            hourCur = GetHours();

            lastSeconds = 1800 - ((hourCur * 3600 + minCur * 60 + secCur) - (hourSave * 3600 + minSave * 60 + secSave));
            if (lastSeconds > 0)
            {
                IsCanLab = false;
            }
            else
            {
                IsCanLab = true;

                string subKeyName = "BanTimer";
                RegistryKey key = Registry.CurrentUser;
                if (key.OpenSubKey(subKeyName) != null)
                    key.DeleteSubKeyTree(subKeyName);
            }
        }

        private int GetSeconds()
        {
            return Convert.ToInt32(DateTime.Now.ToString().Substring((DateTime.Now.ToString().Length - 2), 2));
        }
        private int GetMinutes()
        {
            return Convert.ToInt32(DateTime.Now.ToString().Substring((DateTime.Now.ToString().Length - 5), 2));
        }
        private int GetHours()
        {
            return Convert.ToInt32(DateTime.Now.ToString().Substring((DateTime.Now.ToString().Length - 8), 2));
        }

        //_________________________________________________ Таймер для Капчи _______________________________________________________________________________________

        private DispatcherTimer timerCaptcha;
        private int n;

        private void Captcha_Tick(object sender, EventArgs e)
        {
            n++;
            if (n == 10)
            {
                CaptchaUnban();
            }
        }

        private void CaptchaUnban()
        {
            button2.IsEnabled = true;
            timerCaptcha.Stop();
        }

        private void CaptchaBan()
        {
            button2.IsEnabled = false;
            n = 0;
            timerCaptcha.Start();
        }
    }
}
