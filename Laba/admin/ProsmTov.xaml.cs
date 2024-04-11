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

namespace Laba.admin
{
    /// <summary>
    /// Логика взаимодействия для ProsmTov.xaml
    /// </summary>
    public partial class ProsmTov : Window
    {
        public ProsmTov()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, RoutedEventArgs e)//Вернутся назад
        {
            admin ad = new admin();
            ad.Show();
            Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)//Выйти в меню входа
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }
    }
}
