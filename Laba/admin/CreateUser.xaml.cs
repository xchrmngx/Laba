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
using CheckTrue;

namespace Laba.admin
{
    /// <summary>
    /// Логика взаимодействия для CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        LaboratoryEntities13 db = new LaboratoryEntities13();
        List<Тип_пользователя> type = new List<Тип_пользователя>();
        public CreateUser()
        {
            InitializeComponent();
            type = db.Тип_пользователя.ToList();

            for (int i = 0; i < type.Count; i++)
            {
                TypeUser.Items.Add(type[i].Наименование);
            }

            cb1.Visibility = Visibility.Hidden;
            cb2.Visibility = Visibility.Hidden;
            cb3.Visibility = Visibility.Hidden;
            cb4.Visibility = Visibility.Hidden;
            cb5.Visibility = Visibility.Hidden;
            cb6.Visibility = Visibility.Hidden;
            cb7.Visibility = Visibility.Hidden;
            cb8.Visibility = Visibility.Hidden;
            cb9.Visibility = Visibility.Hidden;
            cb10.Visibility = Visibility.Hidden;
            cb11.Visibility = Visibility.Hidden;
            cb12.Visibility = Visibility.Hidden;
            cb13.Visibility = Visibility.Hidden;
            cb14.Visibility = Visibility.Hidden;
            cb15.Visibility = Visibility.Hidden;
            cb16.Visibility = Visibility.Hidden;
            cb17.Visibility = Visibility.Hidden;
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

        private void button3_Click(object sender, RoutedEventArgs e)//Добавить сотрудника
        {
            if (FIO.Text != "" && Phone.Text != "" && Email.Text != "" && Login.Text != "" && Password.Text != "")
            {
                List<servi> ser = new List<servi>();
                users us = db.users.OrderByDescending(b => b.id).FirstOrDefault();

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



                if (CheckTrue.CheckTrue.Check_Mail(Email.Text) && CheckTrue.CheckTrue.Check_Login(Login.Text) && CheckTrue.CheckTrue.Check_Password(Password.Text))
                {
                    users user = new users
                    {
                        name = FIO.Text,
                        login = Login.Text,
                        password = Password.Text,
                        services = us.id + 1,
                        type = TypeUser.SelectedIndex + 1,
                        number = Phone.Text,
                        Email = Email.Text
                    };
                    db.users.Add(user);
                    db.SaveChanges();

                    for (int i = 0; i < ser.Count; i++)
                    {
                        Услуги_Пользова yp = new Услуги_Пользова
                        {
                            id_services = ser[i].id,
                            id_users = us.id + 1,
                        };
                        db.Услуги_Пользова.Add(yp);
                        db.SaveChanges();
                    }
                }
                else
                    MessageBox.Show("Данные введены не по правилам!");
            }
        }

        private void TypeUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeUser.SelectedIndex == 0 || TypeUser.SelectedIndex == 1)
            {
                cb1.Visibility = Visibility.Visible;
                cb2.Visibility = Visibility.Visible;
                cb3.Visibility = Visibility.Visible;
                cb4.Visibility = Visibility.Visible;
                cb5.Visibility = Visibility.Visible;
                cb6.Visibility = Visibility.Visible;
                cb7.Visibility = Visibility.Visible;
                cb8.Visibility = Visibility.Visible;
                cb9.Visibility = Visibility.Visible;
                cb10.Visibility = Visibility.Visible;
                cb11.Visibility = Visibility.Visible;
                cb12.Visibility = Visibility.Visible;
                cb13.Visibility = Visibility.Visible;
                cb14.Visibility = Visibility.Visible;
                cb15.Visibility = Visibility.Visible;
                cb16.Visibility = Visibility.Visible;
                cb17.Visibility = Visibility.Visible;

                cb1.IsChecked = false;
                cb2.IsChecked = false;
                cb3.IsChecked = false;
                cb4.IsChecked = false;
                cb5.IsChecked = false;
                cb6.IsChecked = false;
                cb7.IsChecked = false;
                cb8.IsChecked = false;
                cb9.IsChecked = false;
                cb10.IsChecked = false;
                cb11.IsChecked = false;
                cb12.IsChecked = false;
                cb13.IsChecked = false;
                cb14.IsChecked = false;
                cb15.IsChecked = false;
                cb16.IsChecked = false;
                cb17.IsChecked = false;
            }
            else
            {
                cb1.Visibility = Visibility.Hidden;
                cb2.Visibility = Visibility.Hidden;
                cb3.Visibility = Visibility.Hidden;
                cb4.Visibility = Visibility.Hidden;
                cb5.Visibility = Visibility.Hidden;
                cb6.Visibility = Visibility.Hidden;
                cb7.Visibility = Visibility.Hidden;
                cb8.Visibility = Visibility.Hidden;
                cb9.Visibility = Visibility.Hidden;
                cb10.Visibility = Visibility.Hidden;
                cb11.Visibility = Visibility.Hidden;
                cb12.Visibility = Visibility.Hidden;
                cb13.Visibility = Visibility.Hidden;
                cb14.Visibility = Visibility.Hidden;
                cb15.Visibility = Visibility.Hidden;
                cb16.Visibility = Visibility.Hidden;
                cb17.Visibility = Visibility.Hidden;

                cb1.IsChecked = false;
                cb2.IsChecked = false;
                cb3.IsChecked = false;
                cb4.IsChecked = false;
                cb5.IsChecked = false;
                cb6.IsChecked = false;
                cb7.IsChecked = false;
                cb8.IsChecked = false;
                cb9.IsChecked = false;
                cb10.IsChecked = false;
                cb11.IsChecked = false;
                cb12.IsChecked = false;
                cb13.IsChecked = false;
                cb14.IsChecked = false;
                cb15.IsChecked = false;
                cb16.IsChecked = false;
                cb17.IsChecked = false;
            }
        }
    }
}
