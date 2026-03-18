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
using System.Data.Entity;


namespace ShopApp_Potapov.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private ShopApp_PotapovEntities _context;
        private Users currentUser;

        public OrdersPage(ShopApp_PotapovEntities context, Users user)
        {
            InitializeComponent();

            _context = context;
            currentUser = user;


            LoadOrders();

        }

        private void LoadOrders()
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("Контекст базы данных не инициализирован!");
                    return;
                }

                var orders = _context.Orders
                    .Include("Clients")
                    .Include("Users")
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                dgOrders.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка загрузки заказов:\n{ex.Message}\n\n" +
                    $"Внутренняя ошибка:\n{ex.InnerException?.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrders.SelectedItem is Orders order)
            {
                var statusWindow = new OrdersWindow(order.Status);
                if (statusWindow.ShowDialog() == true)
                {
                    order.Status = statusWindow.NewStatus;
                    _context.SaveChanges();
                    LoadOrders();
                    MessageBox.Show("Статус изменён");
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ");
            }
        }

        private void cmbFilterStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFilterStatus.SelectedItem is ComboBoxItem item)
            {
                string status = item.Content.ToString();

                if (status == "Все заказы")
                {
                    LoadOrders();
                }
                else
                {
                    var orders = _context.Orders
                        .Include("Clients")
                        .Include("Users")
                        .Where(o => o.Status == status)
                        .OrderByDescending(o => o.DateCreated)
                        .ToList();

                    dgOrders.ItemsSource = orders;
                }
            }
        }
    }
}
