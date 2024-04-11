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

namespace Laba.bugalter
{
    /// <summary>
    /// Логика взаимодействия для AllОтчет.xaml
    /// </summary>
    public partial class AllОтчет : Window
    {
        users user = new users();
        public AllОтчет(users user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void button1_Click(object sender, RoutedEventArgs e)//Выйти в меню входа
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)//Вернутся назад
        {
            bugalter bg = new bugalter(user);
            bg.Show();
            Close();
        }
    }
}
