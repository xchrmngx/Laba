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
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        public admin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)//Создать отчет
        {
            CreateОтчет co = new CreateОтчет(null);
            co.Show();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)//Управление пользователя
        {
            UserRedact ur = new UserRedact();
            ur.Show();
            Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)//Просмотр товаров о расходных материалов
        {
            ProsmTov pt = new ProsmTov();
            pt.Show();
            Close();
        }

        private void button4_Click(object sender, RoutedEventArgs e)//Выйти в меню входа
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }
    }
}
