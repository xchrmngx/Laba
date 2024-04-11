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
    /// Логика взаимодействия для UserRedact.xaml
    /// </summary>
    public partial class UserRedact : Window
    {
        LaboratoryEntities13 db = new LaboratoryEntities13();
        List<users> users = new List<users>();
        public UserRedact()
        {
            InitializeComponent();
            users = db.users.ToList();
            listUsers.ItemsSource = users;
        }

        private void button2_Click(object sender, RoutedEventArgs e)//Выйти в меню входа
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)//Вернутся назад
        {
            admin ad = new admin();
            ad.Show();
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = Log.Text;
            if (Log.Text != "")
            {
                var foundUsers = users.Where(x => x.login.StartsWith(search)).ToList();
                listUsers.ItemsSource = foundUsers;
            }
            else
            {
                listUsers.ItemsSource = users;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)//Добавить сотрудника
        {
            CreateUser cu = new CreateUser();
            cu.Show();
            Close();
        }
    }
}
