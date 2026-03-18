using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopApp_Potapov.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrPage.xaml
    /// </summary>
    public partial class RegistrPage : Page
    {
        private ShopApp_PotapovEntities _context;
        public RegistrPage()
        {
            InitializeComponent();
            _context = new ShopApp_PotapovEntities();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;
            string repPassword = txtPasswordrRep.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string fio = txtClientFullName.Text;
            string phoneNum = txtClientPhone.Text;
            string email = txtClientEmail.Text;

            if (string.IsNullOrEmpty(fio) || string.IsNullOrEmpty(phoneNum) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Заполните данные о клиенте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != repPassword)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new ShopApp_PotapovEntities())
            {
                var existingUser = context.Users.FirstOrDefault(u => u.Login == login);
                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newUser = new Users
                {
                    Login = login,
                    Password = password,
                    RoleId = 3 
                };

                context.Users.Add(newUser);
                context.SaveChanges();  

                var newClient = new Clients
                {
                    UserId = newUser.Id,  
                    FullName = fio,
                    Phone = phoneNum,
                    Email = email
                };

                context.Clients.Add(newClient);
                context.SaveChanges();

                MessageBox.Show(
                    $"Регистрация успешна {login}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);


                NavigationService.Navigate(new MainPage(newUser));
            }

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
