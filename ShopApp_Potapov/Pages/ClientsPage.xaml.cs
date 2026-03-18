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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShopApp_Potapov.Windows;

namespace ShopApp_Potapov.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {

        private ShopApp_PotapovEntities _context;

        private Users _currentUser;
        public ClientsPage(ShopApp_PotapovEntities context, Users user)
        {
            InitializeComponent();
            _context = context;
            _currentUser = user;
            LoadClients();
            CheckPer();
        }

        private void CheckPer()
        {
            if (_currentUser.RoleId != 1)
            {
                BtnEdit.Visibility = Visibility.Collapsed;
                BtnDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadClients()
        {
            var clients = _context.Clients  
                .Include("Users.Roles") 
                .ToList();

     
            dgClients.ItemsSource = clients;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem is Clients client)
            {
              
                var user = client.Users; 

                if (user == null && client.UserId.HasValue)
                {
                    user = _context.Users.FirstOrDefault(u => u.Id == client.UserId);
                }

                if (user != null)
                {
                    var editWindow = new ClientEditWindow(_context, client, user);
                    if (editWindow.ShowDialog() == true)
                    {
                        _context.SaveChanges();
                        LoadClients();
                        MessageBox.Show("Данные обновлены", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem is Clients client)
            {
                var result = MessageBox.Show($"Удалить клиента \"{client.FullName}\"?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Clients.Remove(client);
                    _context.SaveChanges();

                    MessageBox.Show("Клиент удалён");
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления");
            }
        }

     
    
    }
}
