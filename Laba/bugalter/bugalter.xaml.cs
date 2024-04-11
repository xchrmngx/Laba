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
    /// Логика взаимодействия для bugalter.xaml
    /// </summary>
    public partial class bugalter : Window
    {
        LaboratoryEntities13 db = new LaboratoryEntities13();
        users user = new users();
        public bugalter(users user)
        {
            InitializeComponent();
            this.user = user;
            FIO.Content = user.name;

            users NewUser = db.users.Where(x => x.id == user.id).FirstOrDefault();
            NewUser.lastenter = DateTime.Today;
            db.SaveChanges();
        }

        private void button1_Click(object sender, RoutedEventArgs e)//Посмотреть отчеты
        {
            AllОтчет all = new AllОтчет(user);
            all.Show();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)//Сформировать счет страховой компании
        {
            SformComp sc = new SformComp(user);
            sc.Show();
            Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)//Выйти
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }
    }
}
